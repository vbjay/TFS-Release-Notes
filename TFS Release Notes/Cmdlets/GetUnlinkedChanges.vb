Imports System.Management.Automation
Imports System.Text.RegularExpressions
Imports Microsoft.TeamFoundation.WorkItemTracking.Client

<Cmdlet(VerbsCommon.Get, "UnlinkedChanges")>
Public Class GetUnlinkedChanges
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

    '<Parameter(Mandatory:=True,
    '           ValueFromPipelineByPropertyName:=True,
    '           ValueFromPipeline:=True,
    '           Position:=1,
    '           HelpMessage:="The IDs of the work items to retrieve changed files from.")>
    'Property WorkItemIDs As Integer()

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

        'Dim changesetRegex As New Regex("vstfs:///VersionControl/Changeset/(\d+)", RegexOptions.Compiled)
        'Dim changesetLinks = links.Where(Function(l) changesetRegex.IsMatch(l.LinkedArtifactUri))

        'Dim changesets = changesetLinks.Select(Function(c) vcs.GetChangeset(CInt(changesetRegex.Match(c.LinkedArtifactUri).Groups(1).Value))).DistinctBy(Function(c) c.ChangesetId)

        'Dim changes = changesets.SelectMany(Function(cs) cs.Changes)

        'Dim byItem = From c In changes Group By ServerPath = c.Item.ServerItem Into ItemChanges = Group
        '             Select New FileChangeInfo With {.ServerPath = ServerPath, .Changes = ItemChanges, .LastCheckin = ItemChanges.Max(Function(c) c.Item.CheckinDate)}

        'For Each item In byItem

        '    For Each ch In item.Changes.OrderByDescending(Function(c) c.Item.CheckinDate)
        '        WriteObject(item)
        '    Next
        'Next

        '    service.QueryHistory("$/TeamProject/", VersionSpec.Latest, 0, RecursionType.Full, userName, null, null, Int32.MaxValue, True, False)
        '    .Cast<Changeset>()
        '.Where(cs=>cs.AssociatedWorkItems.Length==0)

        WriteObject(TFSCollection.ProjectCollection.Name)

    End Sub
End Class
