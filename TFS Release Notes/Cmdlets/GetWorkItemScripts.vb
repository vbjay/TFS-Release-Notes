Imports System.Management.Automation
Imports System.Text.RegularExpressions
Imports Microsoft.TeamFoundation.WorkItemTracking.Client

<Cmdlet(VerbsCommon.Get, "WorkItemScripts")>
Public Class GetWorkItemScripts
    Inherits WorkItemCmdlet

    Protected Overrides Sub BeginProcessing()
        MyBase.BeginProcessing()

    End Sub
    Protected Overrides Sub ProcessRecord()
        MyBase.ProcessRecord()

        'No need to check for array length because PowerShell handles it for us

        Dim wi As WorkItem() = WorkItemIDs.Distinct.Select(Function(w) TFSCollection.WIT.GetWorkItem(w)).ToArray

        If GetSubWorkItems Then
            wi = wi.SelectMany(Function(w) GetChildWorkItems(w, TFSCollection)).DistinctBy(Function(w) w.Id).ToArray
        End If

        Dim attachments = wi.SelectMany(Function(w) w.Attachments.Cast(Of Attachment)()).Where(Function(a) a.Extension.ToLower = ".sql")

        For Each a In attachments
            WriteObject(a)
        Next

    End Sub
End Class
