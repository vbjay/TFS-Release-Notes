Imports System.Management.Automation
Imports Microsoft.TeamFoundation.WorkItemTracking.Client

Public MustInherit Class WorkItemCmdlet
    Inherits TFSCmdlet

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

    Protected currentWorkItems As WorkItem()
    Protected Overrides Sub BeginProcessing()
        MyBase.BeginProcessing()


        'No need to check for array length because PowerShell handles it for us
        WriteVerbose("Retrieving Work Items...")
        currentWorkItems = WorkItemIDs.Distinct.Select(Function(w) TFSCollection.WIT.GetWorkItem(w)).ToArray

        If GetSubWorkItems Then
            currentWorkItems = currentWorkItems.SelectMany(Function(w) GetChildWorkItems(w, TFSCollection)).DistinctBy(Function(w) w.Id).ToArray
        End If
        WriteVerbose("Retrieved Work Items...")
    End Sub
End Class
