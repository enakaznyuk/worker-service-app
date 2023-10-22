using WorkerServiceApp;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<FileWatcherService>();
        services.AddHostedService<FileWatcherService>();
    })
    .Build();

host.Run();