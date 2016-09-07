Imports System.Management.Automation
Imports System.Text.RegularExpressions
Imports Microsoft.TeamFoundation.WorkItemTracking.Client

<Cmdlet(VerbsCommon.Get, "WorkItemSQLScripts")>
Public Class GetWorkItemSQLScripts
    Inherits WorkItemCmdlet

    Protected Overrides Sub BeginProcessing()
        MyBase.BeginProcessing()

    End Sub
    Protected Overrides Sub ProcessRecord()
        MyBase.ProcessRecord()

        Dim wa = wi.Select(Function(w) New WorkItemAttachmentsInfo With {.Workitem = w, .Attachments = w.Attachments.
                               Cast(Of Attachment).Where(Function(a) a.Extension.ToLower = ".sql").
                               OrderBy(Function(a) a.AttachedTime).ToArray}).Where(Function(w) w.Attachments.Any).
        OrderBy(Function(wc) wc.Attachments.Min(Function(a) a.AttachedTime)).ToArray

        For Each a In wa
            WriteObject(a)
        Next

    End Sub
End Class
