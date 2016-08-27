Imports Microsoft.TeamFoundation.VersionControl.Client
Public Class FileChangeInfo
    Property Changes As IEnumerable(Of Change)
    Property LastCheckin As Date
    Property ServerPath As String
End Class
