using WorkerServiceApp;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.Sources.Clear();

builder.Configuration
    .AddJsonFile("fileWatcher.json", optional: true, reloadOnChange: true);

/*FileWatcherConfig options = new();
    builder.Configuration.GetSection(nameof(FileWatcherConfig))
        .Bind(options);*/

/*var options =
    builder.Configuration.GetSection(nameof(FileWatcherConfig))
        .Get<FileWatcherConfig>();*/

builder.Services.Configure<FileWatcherConfig>(
    builder.Configuration.GetSection(
        key: nameof(FileWatcherConfig)));

builder.Services.AddHostedService<FileWatcherService>();
using IHost host = builder.Build();

await host.RunAsync();
