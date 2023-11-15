using WorkerServiceApp;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.Sources.Clear();
IHostEnvironment env = builder.Environment;

builder.Configuration
    .AddJsonFile("fileWatcher.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"fileWatcher.{env.EnvironmentName}.json", true, true);

var options =
    builder.Configuration.GetSection(nameof(FileWatcherConfig))
        .Get<FileWatcherConfig>();

Console.WriteLine($"FileWatcherConfig.FilePath={options.FilePath}");
Console.WriteLine($"FileWatcherConfig.DirectoryLocation={options.DirectoryLocation}");

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<FileWatcherService>();
    })
    .Build();

host.Run();
