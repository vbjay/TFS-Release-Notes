Imports System.Management.Automation
Imports System.Text.RegularExpressions
Imports Microsoft.TeamFoundation.WorkItemTracking.Client


<Cmdlet(VerbsCommon.Get, "WorkItemAttachments")>
Public Class GetWorkItemAttachments
    Inherits WorkItemCmdlet
    <Parameter(Mandatory:=False,
              ValueFromPipelineByPropertyName:=True,
              ValueFromPipeline:=False,
              HelpMessage:="Set this path to download a zip of all attachments. Must be a full file path.")>
    Property ZipPath As String = ""

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

        Dim tmp As String = IO.Path.Combine(IO.Path.GetTempPath, String.Format("{0}", Guid.NewGuid.ToString("D")))
        IO.Directory.CreateDirectory(tmp)
        Dim downloaded As Boolean = True

        For Each a In Attachments
            WriteObject(a)
            If Not String.IsNullOrWhiteSpace(ZipPath) Then

                For Each at In a.Attachments
                    downloaded = DownloadAttachement(at.Id, IO.Path.Combine(tmp, String.Format("{0}-{1}", a.Workitem.Id, at.Name))) And downloaded
                Next
            End If
        Next
        If Not String.IsNullOrWhiteSpace(ZipPath) Then

            If Not downloaded Then
                WriteWarning("Not all files were downloaded.  Cannot make a zip with missing files.")
            Else
                If IO.File.Exists(ZipPath) Then IO.File.Delete(ZipPath)
                IO.Compression.ZipFile.CreateFromDirectory(tmp, ZipPath)

            End If
        End If
    End Sub

    Protected Function GetAttachements() As WorkItemAttachmentsInfo()
        Dim reg As New Regex(AttachmentFilter)

        Return currentWorkItems.Select(Function(w) New WorkItemAttachmentsInfo With {.Workitem = w,
                               .Attachments = w.Attachments.Cast(Of Attachment).Where(Function(a) reg.IsMatch(a.Name)).
                               OrderBy(Function(a) a.AttachedTime).ToArray}).Where(Function(w) w.Attachments.Any).
        OrderBy(Function(wc) wc.Attachments.Min(Function(a) a.AttachedTime)).ToArray

    End Function
End Class
