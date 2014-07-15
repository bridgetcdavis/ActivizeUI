<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="Activize" generation="1" functional="0" release="0" Id="d60a4bdd-c28b-4f89-865b-809cd2f3c77c" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="ActivizeGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="Bootstrap:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/Activize/ActivizeGroup/LB:Bootstrap:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Bootstrap:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Activize/ActivizeGroup/MapBootstrap:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="BootstrapInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Activize/ActivizeGroup/MapBootstrapInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:Bootstrap:Endpoint1">
          <toPorts>
            <inPortMoniker name="/Activize/ActivizeGroup/Bootstrap/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapBootstrap:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Activize/ActivizeGroup/Bootstrap/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapBootstrapInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Activize/ActivizeGroup/BootstrapInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="Bootstrap" generation="1" functional="0" release="0" software="C:\Users\t-bdavis\Documents\Visual Studio 2013\Projects\Bootstrap\Activize\csx\Debug\roles\Bootstrap" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;Bootstrap&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Bootstrap&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Activize/ActivizeGroup/BootstrapInstances" />
            <sCSPolicyUpdateDomainMoniker name="/Activize/ActivizeGroup/BootstrapUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/Activize/ActivizeGroup/BootstrapFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="BootstrapUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="BootstrapFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="BootstrapInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="8a0acac0-301c-49f8-bb86-7a0c2b5bfbf3" ref="Microsoft.RedDog.Contract\ServiceContract\ActivizeContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="879f8b4e-17f8-42cc-871c-0d84f44d26e6" ref="Microsoft.RedDog.Contract\Interface\Bootstrap:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/Activize/ActivizeGroup/Bootstrap:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>