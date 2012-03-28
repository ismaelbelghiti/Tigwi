<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="Tigwi" generation="1" functional="0" release="0" Id="93cc602e-416c-4150-8af7-31d1bf1d023d" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="TigwiGroup" generation="1" functional="0" release="0">
      <settings>
        <aCS name="StorageLibrary_test:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Tigwi/TigwiGroup/MapStorageLibrary_test:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="StorageLibrary_test:StorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Tigwi/TigwiGroup/MapStorageLibrary_test:StorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="StorageLibrary_testInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Tigwi/TigwiGroup/MapStorageLibrary_testInstances" />
          </maps>
        </aCS>
      </settings>
      <maps>
        <map name="MapStorageLibrary_test:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Tigwi/TigwiGroup/StorageLibrary_test/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapStorageLibrary_test:StorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Tigwi/TigwiGroup/StorageLibrary_test/StorageConnectionString" />
          </setting>
        </map>
        <map name="MapStorageLibrary_testInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Tigwi/TigwiGroup/StorageLibrary_testInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="StorageLibrary_test" generation="1" functional="0" release="0" software="C:\Users\Guigui\Desktop\Tigwi\Tigwi\Tigwi\csx\Debug\roles\StorageLibrary_test" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="StorageConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;StorageLibrary_test&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;StorageLibrary_test&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Tigwi/TigwiGroup/StorageLibrary_testInstances" />
            <sCSPolicyFaultDomainMoniker name="/Tigwi/TigwiGroup/StorageLibrary_testFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="StorageLibrary_testFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="StorageLibrary_testInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
</serviceModel>