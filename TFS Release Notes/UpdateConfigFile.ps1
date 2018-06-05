Add-Type -AssemblyName System.Configuration

# Set this to the full path of your App.config
$configPath = [System.IO.Path]::Combine($PSScriptRoot,'TFS Release Notes.dll.config')

[System.AppDomain]::CurrentDomain.SetData("APP_CONFIG_FILE", $configPath)
[Configuration.ConfigurationManager].GetField("s_initState", "NonPublic, Static").SetValue($null, 0)
[Configuration.ConfigurationManager].GetField("s_configSystem", "NonPublic, Static").SetValue($null, $null)
([Configuration.ConfigurationManager].Assembly.GetTypes() | where {$_.FullName -eq "System.Configuration.ClientConfigPaths"})[0].GetField("s_current", "NonPublic, Static").SetValue($null, $null)


