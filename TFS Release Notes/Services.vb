Imports Microsoft.TeamFoundation.Client
Imports Microsoft.TeamFoundation.WorkItemTracking.Client

Module Services

    Function GetTFSCollection(ServerURL As Uri) As TFSCollection

        Dim tfs As New TFSCollection(
        TfsTeamProjectCollectionFactory.GetTeamProjectCollection(ServerURL))

        tfs.ProjectCollection.Authenticate()

        Return tfs
    End Function
End Module
