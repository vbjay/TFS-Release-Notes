Imports System.Management.Automation

<Cmdlet(VerbsCommon.Get, "ChangesetFiles")>
Public Class GetChangesetFiles
    Inherits ChangesetCmdlet

    Protected Overrides Sub ProcessRecord()
        MyBase.ProcessRecord()

        Dim changes = currentChangesets.SelectMany(Function(cs) cs.Changes)
        WriteVerbose("Generating list of Work Items and Changesets...")
        Dim changesetWorkItems = currentChangesets.Select(Function(cs) New With {.Changes =
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
