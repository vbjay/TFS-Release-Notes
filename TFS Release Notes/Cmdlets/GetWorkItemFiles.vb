Imports System.Management.Automation
Imports System.Text.RegularExpressions
Imports Microsoft.TeamFoundation.WorkItemTracking.Client

<Cmdlet(VerbsCommon.Get, "WorkItemFiles")>
Public Class GetWorkItemFiles
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
        Dim vcs = TFSCollection.VCS

        Dim links = wi.SelectMany(Function(w) w.Links.Cast(Of Link).OfType(Of ExternalLink)())
        Dim changesetRegex As New Regex("vstfs:///VersionControl/Changeset/(\d+)", RegexOptions.Compiled)
        Dim changesetLinks = links.Where(Function(l) changesetRegex.IsMatch(l.LinkedArtifactUri))

        Dim changesets = changesetLinks.Select(Function(c) vcs.GetChangeset(CInt(changesetRegex.Match(c.LinkedArtifactUri).Groups(1).Value))).DistinctBy(Function(c) c.ChangesetId)

        Dim changes = changesets.SelectMany(Function(cs) cs.Changes)

        Dim byItem = From c In changes Group By ServerPath = c.Item.ServerItem Into ItemChanges = Group
                     Select New FileChangeInfo With {.ServerPath = ServerPath, .Changes = ItemChanges, .LastCheckin = ItemChanges.Max(Function(c) c.Item.CheckinDate)}

        For Each item In byItem

            WriteObject(item)
        Next

    End Sub
End Class
