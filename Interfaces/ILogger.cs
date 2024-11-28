using SorterApp.Services;
using SorterApp.Utilities;

namespace SorterApp.Interfaces;
public interface ILogger {
    void Log(Level level, string message, int? threadId = null);
    void Stop();
    static void LogSystemInfo(int availableThreads, float availableMemory, long chunkSize) {
        var message = $"{GlobalSettings.GetCurrentTimestamp()} INFO []: " +
                                    $"Detected {availableThreads} logical processors. " +
                                    $"Available memory: {availableMemory} MB. " +
                                    $"Determined chunk size: {chunkSize / GlobalSettings.MB_TO_BYTE} MB.";
        Console.WriteLine(message);
    }
    static void LogError(string message) {
        var _message = $"{GlobalSettings.GetCurrentTimestamp()} ERROR []: {message}";
        Console.WriteLine(_message);
    }
    static void LogInfo(string message) {
        var _message = $"{GlobalSettings.GetCurrentTimestamp()} INFO []: {message}";
        Console.WriteLine(_message);
    }
}