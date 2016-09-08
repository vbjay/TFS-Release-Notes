Imports System.Management.Automation
Imports Microsoft.TeamFoundation.WorkItemTracking.Client
Imports Microsoft.TeamFoundation.VersionControl.Client

Public MustInherit Class ChangesetCmdlet
    Inherits TFSCmdlet

    <Parameter(Mandatory:=True,
               ValueFromPipelineByPropertyName:=True,
               ValueFromPipeline:=True,
               Position:=1,
               HelpMessage:="The IDs of the changesets to retrieve changed files from.")>
    Property ChangesetIDs As Integer()

    Protected cs As Changeset()
    Protected Overrides Sub BeginProcessing()
        MyBase.BeginProcessing()

        'No need to check for array length because PowerShell handles it for us
        WriteVerbose("Retrieving changesets...")
        cs = ChangesetIDs.Distinct.Select(Function(c) TFSCollection.VCS.GetChangeset(c)).ToArray

        WriteVerbose("Retrieved changesets...")
    End Sub
End Class
