<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="Activize" generation="1" functional="0" release="0" Id="f650647a-fe39-4d5b-9bb8-f369a09c261a" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="ActivizeGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="Activize:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/Activize/ActivizeGroup/LB:Activize:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Activize:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Activize/ActivizeGroup/MapActivize:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ActivizeInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Activize/ActivizeGroup/MapActivizeInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:Activize:Endpoint1">
          <toPorts>
            <inPortMoniker name="/Activize/ActivizeGroup/Activize/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapActivize:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Activize/ActivizeGroup/Activize/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapActivizeInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Activize/ActivizeGroup/ActivizeInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="Activize" generation="1" functional="0" release="0" software="C:\Users\t-bdavis\Documents\Visual Studio 2013\Projects\Bootstrap\Activize\csx\Debug\roles\Activize" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;Activize&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Activize&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Activize/ActivizeGroup/ActivizeInstances" />
            <sCSPolicyUpdateDomainMoniker name="/Activize/ActivizeGroup/ActivizeUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/Activize/ActivizeGroup/ActivizeFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="ActivizeUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="ActivizeFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="ActivizeInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="a178534c-3b11-4297-b250-278e854ca5ca" ref="Microsoft.RedDog.Contract\ServiceContract\ActivizeContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="fa22e640-59ef-4d0c-87bc-de9309f4551e" ref="Microsoft.RedDog.Contract\Interface\Activize:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/Activize/ActivizeGroup/Activize:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>