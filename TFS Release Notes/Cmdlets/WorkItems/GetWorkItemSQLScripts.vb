Imports System.Management.Automation
Imports System.Text.RegularExpressions
Imports Microsoft.TeamFoundation.WorkItemTracking.Client

<Cmdlet(VerbsCommon.Get, "WorkItemSQLScripts")>
Public Class GetWorkItemSQLScripts
    Inherits WorkItemCmdlet

    <Parameter(Mandatory:=False,
              ValueFromPipelineByPropertyName:=True,
              ValueFromPipeline:=True,
              HelpMessage:="Set this path to download a zip of all scripts.",
              Position:=3)>
    Property ZipPath As String = ""

    Protected Overrides Sub BeginProcessing()
        MyBase.BeginProcessing()

    End Sub
    Protected Overrides Sub ProcessRecord()
        MyBase.ProcessRecord()

        Dim wa = currentWorkItems.Select(Function(w) New WorkItemAttachmentsInfo With {.Workitem = w, .Attachments = w.Attachments.
                               Cast(Of Attachment).Where(Function(a) a.Extension.ToLower = ".sql").
                               OrderBy(Function(a) a.AttachedTime).ToArray}).Where(Function(w) w.Attachments.Any).
        OrderBy(Function(wc) wc.Attachments.Min(Function(a) a.AttachedTime)).ToArray

        Dim tmp As String = IO.Path.Combine(IO.Path.GetTempPath, String.Format("\{0}", Guid.NewGuid.ToString("D")))
        Dim downloaded As Boolean = True
        For Each a In wa
            WriteObject(a)
            If Not String.IsNullOrWhiteSpace(ZipPath) Then

                For Each at In a.Attachments
                    downloaded = DownloadFile(at.Uri, IO.Path.Combine(tmp, String.Format("{0}-{1}", a.Workitem.Id, at.Name))) And downloaded
                Next
            End If
        Next
        If Not String.IsNullOrWhiteSpace(ZipPath) Then

            If Not downloaded Then
                WriteWarning("Not all files were downloaded.  Cannot make a zip with missing files.")
            Else

                System.IO.Compression.ZipFile.CreateFromDirectory(tmp, ZipPath)

            End If
        End If

    End Sub

End Class
