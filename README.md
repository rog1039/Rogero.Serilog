# Rogero.Serilog

This project aims to make it easy to use Serilog configure Serilog throughout both single and multi-project systems.

We can configure the Serilog factory class, ```ConfigurableSerilogFactory```,  through either code or JSON attributed with type information. Helper methods for reading/writing the JSON is provided in the Rogero.Serilog.Serialization project/nuget package.

## Install
```
Install-Package Rogero.Serilog
Install-Package Rogero.Serilog.ElasticSearchSink
Install-Package Rogero.Serilog.Serialization
```
https://www.nuget.org/packages/Rogero.Serilog/  
https://www.nuget.org/packages/Rogero.Serilog.ElasticSearchSink/  
https://www.nuget.org/packages/Rogero.Serilog.Serialization/

## Use
With an instance of ConfigurableSerilogFactory in hand, you can create an ILogger easily:
```csharp
var logger = factory.Create();
```

## Configuration

### Through code

```csharp
var factory = new ConfigurableSerilogFactory(
                new AppInstanceGuidConfigurator(),
                new BasicApplicationDetailsConfigurator("MyApp", "EnvironmentName"),
                new MachineAndUserDetailsConfigurator(),
                new PropertyEnricherConfigurator("SomeProp", "SomeValue", false),
                new PropertyListEnricherConfigurator(new Dictionary<string, object>{ {"prop1", "val1"}, {"prop2",10 }},true),
                new ConsoleSinkConfigurator(),
                new RollingFileSinkConfigurator("logPrefix"),                
                new ElasticSearchSinkConfigurator(new []{ "http://elastic1:5601", "http://elastic1:5601", }, ElasticSearchIndexName.FromAppName("myApp"), TimeSpan.FromSeconds(1))
                );
```

### Through JSON
###### Requires Rogero.Serilog.Serialization package

Create a factory from a helper method to load JSON from a file and create a factory.
```csharp
var factory = "pathToSomeConfig.json".LoadFromJsonFile();
```

#### Sample .json configuration file.
```javascript
{
  "$type": "Rogero.Serilog.ConfigurableSerilogFactory, Rogero.Serilog",
  "Configurators": {
    "$type": "System.Collections.Generic.List`1[[Rogero.Serilog.ISerilogConfigurator, Rogero.Serilog]], mscorlib",
    "$values": [
      {
        "$type": "Rogero.Serilog.Enrichments.AppInstanceGuidConfigurator, Rogero.Serilog",
        "AppInstanceGuid": "659c1a5e-752a-4501-9f40-a66ef90e427a"
      },
      {
        "$type": "Rogero.Serilog.Enrichments.BasicApplicationDetailsConfigurator, Rogero.Serilog"
      },
      {
        "$type": "Rogero.Serilog.Enrichments.MachineAndUserDetailsConfigurator, Rogero.Serilog"
      },
      {
        "$type": "Rogero.Serilog.Enrichments.PropertyEnricherConfigurator, Rogero.Serilog",
        "PropertyName": "SomeProp",
        "Value": "SomeValue",
        "DestructureObjects": false
      },
      {
        "$type": "Rogero.Serilog.Enrichments.PropertyListEnricherConfigurator, Rogero.Serilog",
        "Properties": {
          "$type": "System.Collections.Generic.Dictionary`2[[System.String, mscorlib],[System.Object, mscorlib]], mscorlib",
          "prop1": "val1",
          "prop2": 10
        },
        "DestructureObjects": true
      },
      {
        "$type": "Rogero.Serilog.Sinks.ConsoleSinkConfigurator, Rogero.Serilog",
        "RestrictedToMinimumLevel": 0,
        "OutputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}",
        "FormatProvider": null
      },
      {
        "$type": "Rogero.Serilog.Sinks.RollingFileSinkConfigurator, Rogero.Serilog",
        "PathFormat": "logPrefix",
        "RestrictedToMinimumLevel": 0,
        "OutputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}",
        "FormatProvider": null,
        "FileSizeLimitBytes": 1073741824,
        "RetainedFileCountLimit": 31
      },
      {
        "$type": "Rogero.Serilog.ElasticSearchSink.Sinks.ElasticSearchSinkConfigurator, Rogero.Serilog.ElasticSearchSink",
        "ElasticSearchUrls": {
          "$type": "System.String[], mscorlib",
          "$values": [
            "http://elastic1:5601",
            "http://elastic1:5601"
          ]
        },
        "IndexName": {
          "$type": "Rogero.Serilog.ElasticSearchSink.Sinks.ElasticSearchIndexName, Rogero.Serilog.ElasticSearchSink",
          "IndexName": "myApp-{0:yyyy.MM.dd}"
        },
        "ShippingPeriod": "00:00:01",
        "ElasticSearchTemplateName": "serilog-events-template",
        "AutoRegisterTemplate": true,
        "BufferInLogsSubdirectory": true
      }
    ]
  }
}
```


