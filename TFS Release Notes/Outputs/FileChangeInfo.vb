Imports Microsoft.TeamFoundation.VersionControl.Client
Imports Microsoft.TeamFoundation.WorkItemTracking.Client
''' <summary>
''' <para type="description">An info object that describes a grouping of changes by serverpath and includes any workitems that are related to the changesets.</para>
''' </summary>
Public Class FileChangeInfo
    ''' <summary>
    ''' <para type="description">List of changes made to the file in ServerPath.</para>
    ''' </summary>
    Property Changes As IEnumerable(Of Change)
    ''' <summary>
    ''' <para type="description">Last time the file was checked in.</para>
    ''' </summary>
    Property LastCheckin As Date
    ''' <summary>
    ''' <para type="description">The server path of the file that has changes.</para>
    ''' </summary>
    Property ServerPath As String
    ''' <summary>
    ''' <para type="description">List of work items linked to the changesets.</para>
    ''' </summary>
    Property WorkItems As IEnumerable(Of WorkItem)
End Class
