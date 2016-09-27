Imports Microsoft.TeamFoundation.Client
Imports Microsoft.TeamFoundation.WorkItemTracking.Client

Module Services

    Function GetTFSCollection(ServerURL As Uri) As TFSCollection

        Return GetTFSCollection(ServerURL, Nothing)
    End Function

    Function GetTFSCollection(ServerURL As Uri, Credentials As Net.ICredentials) As TFSCollection

        Dim tfs As New TFSCollection(
        TfsTeamProjectCollectionFactory.GetTeamProjectCollection(ServerURL))
        If Credentials IsNot Nothing Then tfs.ProjectCollection.Credentials = Credentials
        tfs.ProjectCollection.EnsureAuthenticated()

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

    Function DownloadFile(URI As Uri, LocalPath As String) As Boolean

        Try
            If Not IO.Directory.Exists(IO.Path.GetDirectoryName(LocalPath)) Then
                IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(LocalPath))
            End If
            My.Computer.Network.DownloadFile(URI.ToString, LocalPath)
            Return True
        Catch ex As Exception

        End Try
        Return False
    End Function
End Module
