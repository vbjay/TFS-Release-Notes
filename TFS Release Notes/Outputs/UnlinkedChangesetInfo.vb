Imports Microsoft.TeamFoundation.VersionControl.Client

Public Class UnlinkedChangesetInfo

    Property CommitterName As String
    Property Committer As String
    Property CommitterDisplay As String
    Property Changesets As IEnumerable(Of Changeset)

End Class
