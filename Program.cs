using SorterApp.Services;
using Microsoft.Extensions.DependencyInjection;
using SorterApp.Interfaces;

var serviceProvider = new ServiceCollection()
    .AddSingleton<IFileCreatorFactory, FileCreatorFactory>()
    .AddSingleton<IFileSorterFactory, FileSorterFactory>()
    .AddSingleton<ILogger, Logger>()
    .AddSingleton<TimerService>()
    .BuildServiceProvider();

var timerService = serviceProvider.GetService<TimerService>();
var logger = serviceProvider.GetService<ILogger>();
var fileCreatorFactory = serviceProvider.GetService<IFileCreatorFactory>();
var fileCreator = await fileCreatorFactory.CreateFileAsync("output.txt", 999);
var fileSorterFactory = serviceProvider.GetService<IFileSorterFactory>();
var fileSorter = fileSorterFactory.Create(logger, "output.txt", "sortedFile.txt");

if (fileCreator == null || fileSorter == null) {
    ILogger.LogError("Error: Could not create required objects.");
    return;
}

await timerService.MeasureAsync("File Creation", async () => {
    await fileCreator.CreateFileAsync();
});

timerService.Measure("File Sorting", () => {
    fileSorter.Sort();
});
