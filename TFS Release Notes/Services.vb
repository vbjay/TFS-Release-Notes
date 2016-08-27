Imports Microsoft.TeamFoundation.Client
Imports Microsoft.TeamFoundation.WorkItemTracking.Client

Module Services

    Function GetTFSCollection(ServerURL As Uri) As TFSCollection

        Dim tfs As New TFSCollection(
        TfsTeamProjectCollectionFactory.GetTeamProjectCollection(ServerURL))

        tfs.ProjectCollection.Authenticate()

        Return tfs
    End Function

    Iterator Function GetChildWorkItems(WI As WorkItem, tfs As TFSCollection) As IEnumerable(Of WorkItem)

        Yield WI

        For Each item In WI.WorkItemLinks.Cast(Of WorkItemLink).Where(Function(l) l.LinkTypeEnd.Name = "Child")

            Dim tmp As WorkItem = tfs.WIT.GetWorkItem(item.TargetId)

            For Each tmpWi In GetChildWorkItems(tmp, tfs)

                Yield tmpWi
            Next
        Next
    End Function
End Module
