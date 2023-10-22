﻿namespace WorkerServiceApp;

public class FileWatcherService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var filePath = "folderEvents.txt";
        using var watcher = new FileSystemWatcher("D:\\Temp");

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

        // если операция не отменена, то выполняем задержку в 200 миллисекунд
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(200, stoppingToken);
        }

        await Task.CompletedTask;
    }
}