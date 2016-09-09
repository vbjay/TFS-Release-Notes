Imports System.Management.Automation
Imports Microsoft.TeamFoundation.WorkItemTracking.Client
Imports Microsoft.TeamFoundation.VersionControl.Client

Public MustInherit Class ChangesetCmdlet
    Inherits TFSCmdlet
    ''' <summary>
    ''' <para type="description">The IDs of the changesets to retrieve changed files from.</para>
    ''' </summary>
    <Parameter(Mandatory:=True,
               ValueFromPipelineByPropertyName:=True,
               ValueFromPipeline:=True,
               Position:=1,
               HelpMessage:="The IDs of the changesets to retrieve changed files from.")>
    Property ChangesetIDs As Integer()

    Protected currentChangesets As Changeset()
    Protected Overrides Sub BeginProcessing()
        MyBase.BeginProcessing()

        'No need to check for array length because PowerShell handles it for us
        WriteVerbose("Retrieving changesets...")
        currentChangesets = ChangesetIDs.Distinct.Select(Function(c) TFSCollection.VCS.GetChangeset(c)).ToArray

        WriteVerbose("Retrieved changesets...")
    End Sub
End Class
