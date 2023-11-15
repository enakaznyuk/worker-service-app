using WorkerServiceApp;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.Sources.Clear();
IHostEnvironment env = builder.Environment;

builder.Configuration
    .AddJsonFile("fileWatcher.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"fileWatcher.{env.EnvironmentName}.json", true, true);

FileWatcherConfig options = new();
builder.Configuration.GetSection(nameof(FileWatcherConfig))
    .Bind(options);

Console.WriteLine($"FileWatcherConfig.FilePath={options.FilePath}");
Console.WriteLine($"FileWatcherConfig.DirectoryLocation={options.DirectoryLocation}");

builder.Services.AddHostedService<FileWatcherService>();
using IHost host = builder.Build();
await host.RunAsync();



/*IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        
        services.AddHostedService<FileWatcherService>();
    })
    .Build();

host.Run();*/
