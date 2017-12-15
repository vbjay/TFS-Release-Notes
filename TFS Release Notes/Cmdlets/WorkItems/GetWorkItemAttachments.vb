Imports System.Management.Automation
Imports System.Text.RegularExpressions
Imports Microsoft.TeamFoundation.WorkItemTracking.Client


<Cmdlet(VerbsCommon.Get, "WorkItemAttachments")>
Public Class GetWorkItemAttachments
    Inherits WorkItemCmdlet
    <Parameter(Mandatory:=False,
              ValueFromPipelineByPropertyName:=True,
              ValueFromPipeline:=False,
              HelpMessage:="The filter of file names to filter attachments.  Default:.*",
               Position:=2)>
    Property AttachmentFilter As String = ".*"
    Protected Attachments As WorkItemAttachmentsInfo()

    Protected Overrides Sub BeginProcessing()
        MyBase.BeginProcessing()
        Attachments = GetAttachements()

    End Sub
    Protected Overrides Sub ProcessRecord()
        MyBase.ProcessRecord()

        For Each a In Attachments
            WriteObject(a)

        Next
    End Sub

    Protected Function GetAttachements() As WorkItemAttachmentsInfo()
        Dim reg As New Regex(AttachmentFilter)

        Return currentWorkItems.Select(Function(w) New WorkItemAttachmentsInfo With {.Workitem = w,
                               .Attachments = w.Attachments.Cast(Of Attachment).Where(Function(a) reg.IsMatch(a.Name)).
                               OrderBy(Function(a) a.AttachedTime).ToArray}).Where(Function(w) w.Attachments.Any).
        OrderBy(Function(wc) wc.Attachments.Min(Function(a) a.AttachedTime)).ToArray

    End Function
End Class
