Imports System.Management.Automation

Public Class TFSCmdlet
    Inherits PSCmdlet

    <Parameter(Mandatory:=True,
          ValueFromPipelineByPropertyName:=True,
          ValueFromPipeline:=True,
          Position:=0,
          ParameterSetName:="URL",
          HelpMessage:="The URL of the TFS collection to access.")>
    Property ServerURL As Uri

    <Parameter(Mandatory:=True,
               ValueFromPipelineByPropertyName:=True,
               ValueFromPipeline:=True,
               Position:=0,
               ParameterSetName:="Collection",
               HelpMessage:="The TFS collection to use.")>
    Property TFSCollection As TFSCollection

    Protected Overrides Sub BeginProcessing()
        MyBase.BeginProcessing()
        Select Case True

            Case TFSCollection IsNot Nothing
                'we are good
            Case TFSCollection Is Nothing AndAlso ServerURL IsNot Nothing

                TFSCollection = GetTFSCollection(ServerURL)

        End Select

    End Sub
End Class
