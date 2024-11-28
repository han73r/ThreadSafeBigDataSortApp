using SorterApp.Interfaces;
using SorterApp.Utilities;
using System.Collections.Concurrent;

namespace SorterApp.Services;
internal class Logger : ILogger {
    private readonly ConcurrentQueue<string> _logQueue = new ConcurrentQueue<string>();
    private readonly ManualResetEvent _logEvent = new ManualResetEvent(false);
    private readonly static SemaphoreSlim _writeSemaphore = new SemaphoreSlim(1, 1);
    private readonly string _logFilePath;
    private readonly bool _logToFile;
    private readonly bool _logToConsole;
    private bool _isRunning;
    private Thread _logThread;
    private Level _currentLogLevel;
    public Logger(Level logLevel = Level.INFO, bool logToFile = true, bool logToConsole = true) {
        _currentLogLevel = logLevel;
        string fileName = $"{DateTime.Now:yyyy-MM-dd}.log";
        _logFilePath = Path.Combine(GlobalSettings.RESULTS_FOLDER, fileName);
        _logToFile = logToFile;
        _logToConsole = logToConsole;
        _isRunning = true;
        _logThread = new Thread(ProcessLogQueue) { IsBackground = true };
        _logThread.Start();
    }
    public void Log(Level level, string message, int? threadId = null) {
        if (level < _currentLogLevel) return;
        string timestamp = GlobalSettings.GetCurrentTimestamp();
        string threadInfo = threadId.HasValue ? $"[Thread: #{threadId}]" : "[]";
        string logMessage = $"{timestamp} {level} {threadInfo}: {message}";

        _logQueue.Enqueue(logMessage);
        _logEvent.Set();
    }
    public void SetLogLevel(Level newLogLevel) {
        _currentLogLevel = newLogLevel;
    }
    private void ProcessLogQueue() {
        while (_isRunning || !_logQueue.IsEmpty) {
            if (_logQueue.TryDequeue(out string logMessage)) {
                WriteLogMessage(logMessage);
            } else {
                _logEvent.WaitOne();
                _logEvent.Reset();
            }
        }
        FinishingLogs();
    }
    private void FinishingLogs() {
        while (!_logQueue.IsEmpty) {
            if (_logQueue.TryDequeue(out string logMessage))
                WriteLogMessage(logMessage);
        }
    }
    private void WriteLogMessage(string logMessage) {
        try {
            _writeSemaphore.Wait();
            if (_logToFile)
                File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
            if (_logToConsole)
                Console.WriteLine(logMessage);
        } catch (Exception ex) {
            Console.WriteLine($"Failed to write log: {ex.Message}");
        } finally {
            _writeSemaphore.Release();
        }
    }
    public void Stop() {
        _isRunning = false;
        _logEvent.Set();
        _logThread.Join();
    }
}
public enum Level {
    DEBUG,
    INFO,
    ERROR
}