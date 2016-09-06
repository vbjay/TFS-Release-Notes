Imports Microsoft.TeamFoundation.VersionControl.Client
Imports Microsoft.TeamFoundation.WorkItemTracking.Client

Public Class FileChangeInfo
    Property Changes As IEnumerable(Of Change)
    Property LastCheckin As Date
    Property ServerPath As String
    Property WorkItems As IEnumerable(Of WorkItem)
End Class
