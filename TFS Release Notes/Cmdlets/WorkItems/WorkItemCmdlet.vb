Imports System.Management.Automation
Imports Microsoft.TeamFoundation.WorkItemTracking.Client
Imports Microsoft.TeamFoundation.WorkItemTracking.Proxy

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
    Protected ItemServer As WorkItemServer
    Protected Overrides Sub BeginProcessing()
        MyBase.BeginProcessing()


        'No need to check for array length because PowerShell handles it for us
        WriteVerbose("Retrieving Work Items...")
        currentWorkItems = WorkItemIDs.Distinct.Select(Function(w) TFSCollection.WIT.GetWorkItem(w)).ToArray

        If GetSubWorkItems Then
            WriteVerbose($"Getting all children workitems of ({String.Join(",", currentWorkItems.Select(Function(w) w.Id))}).")
            currentWorkItems = currentWorkItems.SelectMany(Function(w) GetChildWorkItems(w, TFSCollection)).DistinctBy(Function(w) w.Id).ToArray
        End If
        WriteVerbose("Retrieved Work Items...")
        WriteVerbose("Getting workitemserver")
        ItemServer = TFSCollection.ProjectCollection.GetService(Of WorkItemServer)

    End Sub
    Protected Function DownloadAttachement(AttachmentID As Integer, LocalPath As String) As Boolean

        Try
            If Not IO.Directory.Exists(IO.Path.GetDirectoryName(LocalPath)) Then
                IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(LocalPath))
            End If

            Dim pth As String = ItemServer.DownloadFile(AttachmentID)
            IO.File.Copy(pth, LocalPath)
            Return True
        Catch ex As Exception

        End Try
        Return False
    End Function

End Class
