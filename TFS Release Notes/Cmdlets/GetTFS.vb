Imports System.Management.Automation
Imports Microsoft.TeamFoundation.Client

<Cmdlet(VerbsCommon.Get, "Tfs")>
Public Class GetTFS
    Inherits TFSCmdlet

    Protected Overrides Sub BeginProcessing()
        MyBase.BeginProcessing()
    End Sub

    Protected Overrides Sub ProcessRecord()
        MyBase.ProcessRecord()

        WriteObject(TFSCollection)
    End Sub

End Class
