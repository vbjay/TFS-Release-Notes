Imports System.Management.Automation
Imports Microsoft.TeamFoundation.WorkItemTracking.Client
<Cmdlet(VerbsCommon.Get, "WorkItemAttachments")>
Public Class GetWorkItemAttachments
    Inherits WorkItemCmdlet

    Protected Overrides Sub ProcessRecord()
        MyBase.ProcessRecord()

        Dim wa = wi.Select(Function(w) New WorkItemAttachmentsInfo With {.Workitem = w, .Attachments = w.Attachments.Cast(Of Attachment).
                               OrderBy(Function(a) a.AttachedTime).ToArray}).Where(Function(w) w.Attachments.Any).
        OrderBy(Function(wc) wc.Attachments.Min(Function(a) a.AttachedTime)).ToArray

        For Each w In wa
            WriteObject(w)
        Next
    End Sub
End Class
