using SorterApp.Interfaces;
using System.Diagnostics;

namespace SorterApp.Services;
public class TimerService {
    private Stopwatch _stopwatch;
    public void Start() {
        _stopwatch = Stopwatch.StartNew();
    }
    public TimeSpan Stop() {
        _stopwatch.Stop();
        return _stopwatch.Elapsed;
    }
    public async Task<TimeSpan> MeasureAsync(string taskName, Func<Task> action) {
        ILogger.LogInfo($"Starting task: {taskName}");
        Start();
        await action();
        var elapsed = Stop();
        ILogger.LogInfo($"{taskName} completed in {elapsed.Minutes} min {elapsed.Seconds} sec {elapsed.Milliseconds} ms.");
        return elapsed;
    }
    public TimeSpan Measure(string taskName, Action action) {
        ILogger.LogInfo($"Starting task: {taskName}");
        Start();
        action();
        var elapsed = Stop();
        ILogger.LogInfo($"{taskName} completed in {elapsed.Minutes} min {elapsed.Seconds} sec {elapsed.Milliseconds} ms.");
        return elapsed;
    }
}