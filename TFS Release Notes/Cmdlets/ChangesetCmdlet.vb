Imports System.Management.Automation
Imports Microsoft.TeamFoundation.WorkItemTracking.Client
Imports Microsoft.TeamFoundation.VersionControl.Client

Public MustInherit Class ChangesetCmdlet
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
               HelpMessage:="The IDs of the changesets to retrieve changed files from.")>
    Property ChangesetIDs As Integer()

    Protected cs As Changeset()
    Protected Overrides Sub BeginProcessing()
        MyBase.BeginProcessing()
        Select Case True

            Case TFSCollection IsNot Nothing
                'we are good
            Case TFSCollection Is Nothing AndAlso ServerURL IsNot Nothing

                TFSCollection = GetTFSCollection(ServerURL)

        End Select

        'No need to check for array length because PowerShell handles it for us
        WriteVerbose("Retrieving changesets...")
        cs = ChangesetIDs.Distinct.Select(Function(c) TFSCollection.VCS.GetChangeset(c)).ToArray

        WriteVerbose("Retrieved changesets...")
    End Sub
End Class
