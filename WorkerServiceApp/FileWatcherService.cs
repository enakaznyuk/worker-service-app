namespace WorkerServiceApp;

public class FileWatcherService : BackgroundService
{
    
    private readonly IConfiguration Configuration;
    
    public FileWatcherService(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var filePath = "folderEvents.txt";
        
        var path = Configuration["FileWatcher:Path"];
        
        try
        {
            using var watcher = new FileSystemWatcher(path);
            // записываем изменения
            watcher.Changed += async (o, e) =>
                await File.AppendAllTextAsync(filePath, $"{DateTime.Now} Changed: {e.FullPath}\n");
            // записываем данные о создании файлов и папок
            watcher.Created += async (o, e) =>
                await File.AppendAllTextAsync(filePath, $"{DateTime.Now} Created: {e.FullPath}\n");
            // записываем данные об удалении файлов и папок
            watcher.Deleted += async (o, e) =>
                await File.AppendAllTextAsync(filePath, $"{DateTime.Now} Deleted: {e.FullPath}\n");
            // записываем данные о переименовании
            watcher.Renamed += async (o, e) =>
                await File.AppendAllTextAsync(filePath, $"{DateTime.Now} Renamed: {e.OldFullPath} to {e.FullPath}\n");
            // записываем данные об ошибках
            watcher.Error += async (o, e) =>
                await File.AppendAllTextAsync(filePath, $"{DateTime.Now} Error: {e.GetException().Message}\n");

            watcher.IncludeSubdirectories = true; // отслеживаем изменения в подкаталогах
            watcher.EnableRaisingEvents = true; // включаем события
        }
        catch (DirectoryNotFoundException e)
        {
            var errorMessage = "The directory D:\\Temp cannot be found.";
            Console.WriteLine(errorMessage);
            await File.AppendAllTextAsync(filePath, $"{DateTime.Now} Error: {e} {errorMessage}\n");
        }

        // если операция не отменена, то выполняем задержку в 200 миллисекунд
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(200, stoppingToken);
        }

        await Task.CompletedTask;
    }
}