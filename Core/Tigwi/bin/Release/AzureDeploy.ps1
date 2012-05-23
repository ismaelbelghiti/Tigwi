$error.clear()
$sub = $args[0]
$certThumbprint = $args[1].ToUpper()
$certPath = "cert:\LocalMachine\MY\" + $certThumbprint
$cert = get-item $certPath
$buildPath = $args[2]
$packagename = $args[3]
$serviceconfig = $args[4]
$servicename = $args[5]
$storageAccount = $args[6]
$package = join-path $buildPath $packageName
$config = join-path $buildPath $serviceconfig
$a = Get-Date
$buildLabel = $a.ToShortDateString() + "-" + $a.ToShortTimeString()
 
if ((Get-PSSnapin | ?{$_.Name -eq "WAPPSCmdlets"}) -eq $null)  # Change WAPPSCmdlets to AzureManagementToolsSnapIn for cmdlets v1
{
  Add-PSSnapin WAPPSCmdlets  # Change WAPPSCmdlets to AzureManagementToolsSnapIn for cmdlets v1
}
 
$hostedService = Get-HostedService $servicename -Certificate $cert -SubscriptionId $sub | Get-Deployment -Slot Staging
 
if ($hostedService.Status -ne $null)
{
    $hostedService |
      Set-DeploymentStatus 'Suspended' |
      Get-OperationStatus -WaitToComplete
    $hostedService |
      Remove-Deployment |
      Get-OperationStatus -WaitToComplete
}
 
Get-HostedService $servicename -Certificate $cert -SubscriptionId $sub |
    New-Deployment -package $package -configuration $config -label $buildLabel -serviceName $servicename -StorageServiceName $storageAccount |
    Get-OperationStatus -WaitToComplete
 
Get-HostedService $servicename -Certificate $cert -SubscriptionId $sub |
    Get-Deployment -Slot Staging |
    Set-DeploymentStatus 'Running' |
    Get-OperationStatus -WaitToComplete
 
$Deployment = Get-HostedService $servicename -Certificate $cert -SubscriptionId $sub | Get-Deployment -Slot Staging
Write-host Deployed to staging slot: $Deployment.Url
 
if ($error) { exit 888 }