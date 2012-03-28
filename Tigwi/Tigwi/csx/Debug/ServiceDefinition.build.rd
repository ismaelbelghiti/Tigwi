<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="Tigwi" generation="1" functional="0" release="0" Id="a55e29f3-f52a-40d4-9230-f4fb507c5dd8" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="TigwiGroup" generation="1" functional="0" release="0">
      <settings>
        <aCS name="Tests:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Tigwi/TigwiGroup/MapTests:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="Tests:StorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Tigwi/TigwiGroup/MapTests:StorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="TestsInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Tigwi/TigwiGroup/MapTestsInstances" />
          </maps>
        </aCS>
      </settings>
      <maps>
        <map name="MapTests:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Tigwi/TigwiGroup/Tests/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapTests:StorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Tigwi/TigwiGroup/Tests/StorageConnectionString" />
          </setting>
        </map>
        <map name="MapTestsInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Tigwi/TigwiGroup/TestsInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="Tests" generation="1" functional="0" release="0" software="C:\Users\Guigui\Desktop\Tigwi\Tigwi\Tigwi\csx\Debug\roles\Tests" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="StorageConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;Tests&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Tests&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Tigwi/TigwiGroup/TestsInstances" />
            <sCSPolicyFaultDomainMoniker name="/Tigwi/TigwiGroup/TestsFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="TestsFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="TestsInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
</serviceModel>