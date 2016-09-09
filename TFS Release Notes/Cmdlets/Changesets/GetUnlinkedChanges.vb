Imports System.Management.Automation
Imports System.Text.RegularExpressions
Imports Microsoft.TeamFoundation.WorkItemTracking.Client
Imports Microsoft.TeamFoundation.VersionControl.Client

<Cmdlet(VerbsCommon.Get, "UnlinkedChanges")>
Public Class GetUnlinkedChanges
    Inherits TFSCmdlet

    <Parameter(Mandatory:=True,
               ValueFromPipelineByPropertyName:=True,
               ValueFromPipeline:=True,
               Position:=1,
               HelpMessage:="The server path to the project folder to retrieve changesets from.  Ex: $/SomeProjRoot/")>
    Property ProjectPath As String

    <Parameter(Mandatory:=False,
            ValueFromPipelineByPropertyName:=True,
            ValueFromPipeline:=True,
            Position:=2,
            HelpMessage:="The date to search forward from for changesets.  Must be a value that can be converted to a DateTime.  Defaults to -7 days.")>
    Property FromDate As String

    <Parameter(Mandatory:=False,
           ValueFromPipelineByPropertyName:=True,
           ValueFromPipeline:=True,
           Position:=3,
           HelpMessage:="The date to search backwards from for changesets.  Must be a value that can be converted to a DateTime.  Defaults to current date.")>
    Property ToDate As String

    Protected Overrides Sub BeginProcessing()
        MyBase.BeginProcessing()
        If String.IsNullOrWhiteSpace(FromDate) Then
            FromDate = Now.AddDays(-7).ToShortDateString
        End If

        If String.IsNullOrWhiteSpace(ToDate) Then
            ToDate = Now.ToShortDateString
        End If
    End Sub

    Protected Overrides Sub ProcessRecord()
        MyBase.ProcessRecord()

        Dim dtFrom As Date = Date.Parse(FromDate)
        Dim dtTo As Date = Date.Parse(ToDate)

        Dim vfrom As VersionSpec = VersionSpec.ParseSingleSpec(String.Format("D{0:MM/dd/yyyy}", dtFrom), Nothing)
        Dim vTo As VersionSpec = VersionSpec.ParseSingleSpec(String.Format("D{0:MM/dd/yyyy}", dtTo), Nothing)

        Dim changes = TFSCollection.VCS.QueryHistory(ProjectPath,
                                                     VersionSpec.Latest,
                                                     0, RecursionType.Full,
                                                     Nothing,
                                                     vfrom,
                                                     vTo,
                                                     Int32.MaxValue,
                                                    True,
                                                    False).
                                                Cast(Of Changeset)().
                                                Where(Function(cs) cs.AssociatedWorkItems.Length = 0).
                                                GroupBy(Function(cs) New With {
                                                            Key .Committer = cs.Committer,
                                                            Key .Name = cs.CommitterDisplayName})
        For Each c In changes

            WriteObject(New With {.commiter = String.Format("{0}({1})", c.Key.Name, c.Key.Committer), .Changes = c})
            'For Each ch In c.OrderByDescending(Function(chng) chng.CreationDate)
            '    WriteObject(ch)
            'Next
        Next

    End Sub
End Class
