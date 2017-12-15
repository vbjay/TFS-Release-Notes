Imports Microsoft.TeamFoundation.Client
Imports Microsoft.TeamFoundation.WorkItemTracking.Client
Imports Microsoft.VisualStudio.Services.Common
Imports Microsoft.VisualStudio.Services.WebApi

Module Services

    Function GetTFSCollection(ServerURL As Uri) As TFSCollection

        Return GetTFSCollection(ServerURL, Nothing)
    End Function

    Function GetTFSCollection(ServerURL As Uri, Credentials As VssBasicCredential) As TFSCollection



        Dim projects As New TfsTeamProjectCollection(ServerURL, Credentials)
        projects.Authenticate()

        Dim tfs As New TFSCollection(
        projects)


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
