Imports System.Management.Automation
Imports System.Text.RegularExpressions
Imports Microsoft.TeamFoundation.WorkItemTracking.Client

<Cmdlet(VerbsCommon.Get, "WorkItemFiles")>
Public Class GetWorkItemFiles
    Inherits PSCmdlet

    <Parameter(Mandatory:=True,
               ValueFromPipelineByPropertyName:=True,
               ValueFromPipeline:=True,
               Position:=0,
               ParameterSetName:="URL",
               HelpMessage:="The URL of the TFS collection to access.")>
    Property ServerURL As Uri

    <Parameter(Mandatory:=True,
               ValueFromPipelineByPropertyName:=True,
               ValueFromPipeline:=True,
               Position:=0,
               ParameterSetName:="Collection",
               HelpMessage:="The TFS collection to use.")>
    Property TFSCollection As TFSCollection

    <Parameter(Mandatory:=True,
               ValueFromPipelineByPropertyName:=True,
               ValueFromPipeline:=True,
               Position:=1,
               HelpMessage:="The ID of the workitem to retrieve changed files from.")>
    Property WorkItemID As Integer

    <Parameter(Mandatory:=False,
               ValueFromPipelineByPropertyName:=True,
               ValueFromPipeline:=True,
               HelpMessage:="Add this switch to get all child workitems recursively and get changes from all of them.")>
    Property GetSubWorkItems As SwitchParameter
        Get
            Return _GetSubWorkItems
        End Get
        Set(value As SwitchParameter)
            _GetSubWorkItems = value.ToBool
        End Set
    End Property

    Private _GetSubWorkItems As Boolean = False

    Protected Overrides Sub BeginProcessing()
        MyBase.BeginProcessing()
        Select Case True

            Case TFSCollection IsNot Nothing
                'we are good
            Case TFSCollection Is Nothing AndAlso ServerURL IsNot Nothing

                TFSCollection = GetTFSCollection(ServerURL)

        End Select

    End Sub

    Protected Overrides Sub ProcessRecord()
        MyBase.ProcessRecord()

        Dim wi As WorkItem() = {TFSCollection.WIT.GetWorkItem(WorkItemID)}

        If _GetSubWorkItems Then
            wi = GetCHildWorkItems(wi.First, TFSCollection).ToArray
        End If
        Dim vcs = TFSCollection.VCS

        Dim links = wi.SelectMany(Function(w) w.Links.Cast(Of Link).OfType(Of ExternalLink))
        Dim changesetRegex As New Regex("vstfs:///VersionControl/Changeset/(\d+)", RegexOptions.Compiled)
        Dim changesetLinks = links.Where(Function(l) changesetRegex.IsMatch(l.LinkedArtifactUri))

        Dim changesets = changesetLinks.Select(Function(c) vcs.GetChangeset(CInt(changesetRegex.Match(c.LinkedArtifactUri).Groups(1).Value))).DistinctBy(Function(c) c.ChangesetId)

        Dim changes = changesets.SelectMany(Function(cs) cs.Changes)

        Dim byItem = From c In changes Group By ServerPath = c.Item.ServerItem Into ItemChanges = Group
                     Select New FileChangeInfo With {.ServerPath = ServerPath, .Changes = ItemChanges, .LastCheckin = ItemChanges.Max(Function(c) c.Item.CheckinDate)}

        For Each item In byItem

            For Each ch In item.Changes.OrderByDescending(Function(c) c.Item.CheckinDate)
                WriteObject(item)
            Next
        Next

    End Sub
End Class
