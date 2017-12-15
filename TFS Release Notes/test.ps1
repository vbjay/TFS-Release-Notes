Import-Module ".\TFS Release Notes.psd1" -Verbose
#$cred = New-Object Microsoft.VisualStudio.Services.Common.VssBasicCredential('','yourPAT')
#$tfs=get-tfs -ServerURL https://xxx.visualstudio.com/DefaultCollection -Credentials $cred
$tfs=get-tfs -ServerURL https://xxx.visualstudio.com/DefaultCollection -VSTSToken yourPAT

$tfs
 $tfs.ProjectCollection.HasAuthenticated
 $folderExists=test-path .\release
 if($folderExists -eq $False)
 {
	 mkdir release
 }

#generate changelog
Get-WorkItemChangesets -TFSCollection $tfs -WorkItemIDs 3052 -GetSubWorkItems |sort -Property CreationDate |select @{N='Date';E={$_.CreationDate}},@{N='ID';E={$_.ChangesetId}},@{N='Author';E={$_.CommitterDisplayName}},@{N='Description';E={$_.Comment}} >.\release\changelog.txt

#generate list of files touched by workitems
Get-WorkItemFiles -TFSCollection $tfs -WorkItemIDs 3052 -GetSubWorkItems |select -Property ServerPath >".\release\modified files.txt"

#get a list of files attached and generate a zip of those files
Get-WorkItemAttachments -TFSCollection $tfs -WorkItemIDs 3052 -GetSubWorkItems -ZipPath .\release\test.zip
