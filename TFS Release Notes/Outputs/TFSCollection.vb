Imports Microsoft.TeamFoundation.Client
Imports System.ComponentModel
Public Class TFSCollection
    Public Sub New(Project As TfsTeamProjectCollection)
        _ProjectCollection = Project
    End Sub
    Private _ProjectCollection As TfsTeamProjectCollection
    ReadOnly Property ProjectCollection As TfsTeamProjectCollection
        Get
            Return _ProjectCollection
        End Get
    End Property
    <Description("Work Item Tracking")>
    ReadOnly Property WIT As Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemStore
        Get
            Dim service As Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemStore =
           CType(ProjectCollection.GetService(GetType(Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemStore)),
           Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemStore)

            Return service
        End Get
    End Property
    <Description("Version Control Server")>
    ReadOnly Property VCS As Microsoft.TeamFoundation.VersionControl.Client.VersionControlServer
        Get
            Dim service As Microsoft.TeamFoundation.VersionControl.Client.VersionControlServer =
                       CType(ProjectCollection.GetService(GetType(Microsoft.TeamFoundation.VersionControl.Client.VersionControlServer)),
                       Microsoft.TeamFoundation.VersionControl.Client.VersionControlServer)

            Return service
        End Get
    End Property

End Class
