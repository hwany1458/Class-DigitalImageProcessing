param(
    [Parameter(Mandatory=$false)] [string]   $installPath,
    [Parameter(Mandatory=$false)] [string]   $toolsPath,
    [Parameter(Mandatory=$false)]            $package,
    [Parameter(Mandatory=$false)]           $project
)

#first of all, check if it's running on Win platform
if ($IsWindows -ne $true -or $PSBoundParameters.ContainsKey('project') -eq $false -or $dte -eq $null) {
    return;
}

#find a DocuViewareLicensing.RegisterKEY occurence to check if the package is a fresh install or not
$find = $dte.Find;
if ($package.Id -eq 'GdPicture') {
    $find.findWhat = ".RegisterKEY"
} elseif ($package.Id -eq 'DocuVieware') {
    $find.findWhat = "DocuViewareLicensing.RegisterKEY"
}

$find.action = 2 # vsFindAction.vsFindActionFindAll
$find.target = 6 # vsFindTarget.vsFindTargetSolution
$find.ResultsLocation = 0 # vsFindResultsLocation.vsFindResultsNone
$find.WaitForFindToComplete = $true

if ($find.Execute() -eq 0) {
# open embedded licenseManager to get a key
    $msgBoxInput =  [System.Windows.MessageBox]::Show(
        "This appears to be the first installation of $($package.Id). Do you need an evaluation key?",
        $package.Id,
        'OKCancel',
        'Information')

    switch  ($msgBoxInput) {
        'Ok' {
            if ($package.Id -eq 'GdPicture') {
                Start-Process "https://www.gdpicture.com/guides/gdpicture/Evaluation.html"
                Start-Process "$toolsPath\licenseManager.exe" -ArgumentList @("/silent")
            } elseif ($package.Id -eq 'DocuVieware') {
                Start-Process "https://docuvieware.com/guides/aspnet/Registering%20DocuVieware.html"
                Start-Process "$toolsPath\licenseManager.exe"  -ArgumentList @("/silent")
            }
        }
    }
}