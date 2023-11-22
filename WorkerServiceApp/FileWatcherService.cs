using Microsoft.Extensions.Options;

namespace WorkerServiceApp;

public class FileWatcherService : BackgroundService
{
    private readonly FileWatcherConfig _options;
    
    public FileWatcherService(IOptions<FileWatcherConfig> options) =>
        _options = options.Value;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var filePath = "folderEventsNew.txt";
        
        Console.WriteLine("_options.DirectoryLocation = " + _options.DirectoryLocation);
        Console.WriteLine("_options.FilePath = " + _options.FilePath);
        
        try
        {
            var watcher = new FileSystemWatcher(_options.DirectoryLocation);
            // записываем данные о создании файлов и папок
            watcher.Created += OnCreated;
            // записываем данные об удалении файлов и папок
            watcher.Deleted += async (o, e) =>
                await File.AppendAllTextAsync(_options.FilePath, $"{DateTime.Now} Deleted: {e.FullPath}\n");
            // записываем данные о переименовании
            watcher.Renamed += async (o, e) =>
                await File.AppendAllTextAsync(_options.FilePath, $"{DateTime.Now} Renamed: {e.OldFullPath} to {e.FullPath}\n");
            // записываем данные об ошибках
            watcher.Error += async (o, e) =>
                await File.AppendAllTextAsync(_options.FilePath, $"{DateTime.Now} Error: {e.GetException().Message}\n");

            watcher.IncludeSubdirectories = true; // отслеживаем изменения в подкаталогах
            watcher.EnableRaisingEvents = true; // включаем события
           
            
            // если операция не отменена, то выполняем задержку в 200 миллисекунд
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(200, stoppingToken);
            }
            //watcher.Dispose();
            await Task.CompletedTask;
            watcher.Dispose();
        } 
        catch (DirectoryNotFoundException)
        {
            var errorMessage = "The directory D:\\Temp cannot be found.";
            Console.WriteLine(errorMessage);
        }
        
        
    }
    
    private static void OnCreated(object sender, FileSystemEventArgs e)
    {
        var filePath = "folderEventsNew.txt";
        Console.WriteLine($"{DateTime.Now} Created: {e.FullPath}\n");
        File.AppendAllTextAsync(filePath, $"{DateTime.Now} Created: {e.FullPath}\n");
    }
}