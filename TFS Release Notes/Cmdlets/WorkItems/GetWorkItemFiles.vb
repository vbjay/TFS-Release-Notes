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

        Dim vcs = TFSCollection.VCS

        Dim links = currentWorkItems.SelectMany(Function(w) w.Links.Cast(Of Link).OfType(Of ExternalLink)())
        Dim changesetRegex As New Regex("vstfs:///VersionControl/Changeset/(\d+)", RegexOptions.Compiled)
        Dim changesetLinks = links.Where(Function(l) changesetRegex.IsMatch(l.LinkedArtifactUri))

        Dim changesets = changesetLinks.Select(Function(c) vcs.GetChangeset(CInt(changesetRegex.Match(c.LinkedArtifactUri).Groups(1).Value))).DistinctBy(Function(c) c.ChangesetId)

        Dim changes = changesets.SelectMany(Function(cs) cs.Changes)
        WriteVerbose("Generating list of Work Items and Changesets...")
        Dim changesetWorkItems = changesets.Select(Function(cs) New With {.Changes =
                                                       cs.Changes.Select(Function(c) c.Item.ItemId).ToArray,
                                                       .WorkItems = cs.WorkItems}).ToArray
        WriteVerbose("Generated list of Work Items and Changesets...")
        WriteVerbose("Generating file change list...")
        Dim byItem = (From c In changes Group By ServerPath = c.Item.ServerItem Into ItemChanges = Group
                      Select New FileChangeInfo With {.ServerPath = ServerPath,
                          .Changes = ItemChanges,
                          .LastCheckin = ItemChanges.Max(Function(ch) ch.Item.CheckinDate)}).ToList

        WriteVerbose("List generated...")
        For Each item In byItem
            'Getting this value here because it is slow.  It allows better progress than getting the value above.
            item.WorkItems = changesetWorkItems.Where(Function(ch) item.Changes.Select(Function(ic) ic.Item.ItemId).Intersect(ch.Changes).Any).
                SelectMany(Function(wc) wc.WorkItems).DistinctBy(Function(w) w.Id).ToArray

            WriteObject(item)
        Next

    End Sub

End Class
