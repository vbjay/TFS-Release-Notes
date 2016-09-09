Imports System.Text.RegularExpressions
Imports Microsoft.TeamFoundation.WorkItemTracking.Client
Imports System.Management.Automation

<Cmdlet(VerbsCommon.Get, "WorkItemChangesets")>
Public Class GetWorkItemChangesets
    Inherits WorkItemCmdlet

    Protected Overrides Sub ProcessRecord()
        MyBase.ProcessRecord()

        Dim vcs = TFSCollection.VCS

        Dim links = currentWorkItems.SelectMany(Function(w) w.Links.Cast(Of Link).OfType(Of ExternalLink)())
        Dim changesetRegex As New Regex("vstfs:///VersionControl/Changeset/(\d+)", RegexOptions.Compiled)
        Dim changesetLinks = links.Where(Function(l) changesetRegex.IsMatch(l.LinkedArtifactUri))

        Dim changesets = changesetLinks.Select(Function(c) vcs.GetChangeset(CInt(changesetRegex.Match(c.LinkedArtifactUri).Groups(1).Value))).DistinctBy(Function(c) c.ChangesetId)

        For Each cs In changesets.OrderBy(Function(c) c.CreationDate)
            WriteObject(cs)
        Next
    End Sub
End Class
