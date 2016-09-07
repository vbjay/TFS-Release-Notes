Imports System.Management.Automation
Imports Microsoft.TeamFoundation.WorkItemTracking.Client

Public MustInherit Class WorkItemCmdlet
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
               HelpMessage:="The IDs of the work items to retrieve changed files from.")>
    Property WorkItemIDs As Integer()
    Private _GetSubWorkItems As Boolean = False
    <Parameter(Mandatory:=False,
               ValueFromPipelineByPropertyName:=True,
               ValueFromPipeline:=True,
               HelpMessage:="Add this switch to get all child work items recursively and get changes from all of them.")>
    Property GetSubWorkItems As SwitchParameter
        Get
            Return _GetSubWorkItems
        End Get
        Set(value As SwitchParameter)
            _GetSubWorkItems = value.ToBool
        End Set
    End Property

    Protected wi As WorkItem()
    Protected Overrides Sub BeginProcessing()
        MyBase.BeginProcessing()
        Select Case True

            Case TFSCollection IsNot Nothing
                'we are good
            Case TFSCollection Is Nothing AndAlso ServerURL IsNot Nothing

                TFSCollection = GetTFSCollection(ServerURL)

        End Select

        'No need to check for array length because PowerShell handles it for us
        WriteVerbose("Retrieving Work Items...")
        wi = WorkItemIDs.Distinct.Select(Function(w) TFSCollection.WIT.GetWorkItem(w)).ToArray

        If GetSubWorkItems Then
            wi = wi.SelectMany(Function(w) GetChildWorkItems(w, TFSCollection)).DistinctBy(Function(w) w.Id).ToArray
        End If
        WriteVerbose("Retrieved Work Items...")
    End Sub
End Class
