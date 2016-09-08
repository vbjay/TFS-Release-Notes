﻿Imports System.Management.Automation
Imports System.Text.RegularExpressions
Imports Microsoft.TeamFoundation.WorkItemTracking.Client
Imports Microsoft.TeamFoundation.VersionControl.Client

<Cmdlet(VerbsCommon.Get, "UnlinkedChanges")>
Public Class GetUnlinkedChanges
    Inherits TFSCmdlet

    <Parameter(Mandatory:=True,
               ValueFromPipelineByPropertyName:=True,
               ValueFromPipeline:=True,
               Position:=1,
               HelpMessage:="The server path to the project folder to retrieve changesets from.  Ex: $/SomeProjRoot/")>
    Property ProjectPath As String

    Protected Overrides Sub BeginProcessing()
        MyBase.BeginProcessing()

    End Sub

    Protected Overrides Sub ProcessRecord()
        MyBase.ProcessRecord()

        Dim changes = TFSCollection.VCS.QueryHistory(ProjectPath, VersionSpec.Latest, 0, RecursionType.Full, Nothing, Nothing, Nothing, Int32.MaxValue, True, False).
            Cast(Of Changeset)().
            Where(Function(cs) cs.AssociatedWorkItems.Length = 0).
            GroupBy(Function(cs) cs.Committer)
        For Each c In changes

            WriteObject(c.Key)
            For Each ch In c.OrderByDescending(Function(chng) chng.CreationDate)
                WriteObject(ch)
            Next
        Next

    End Sub
End Class
