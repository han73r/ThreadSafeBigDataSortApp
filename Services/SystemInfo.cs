using System.Diagnostics;

namespace SorterApp.Services;
public static class SystemInfo {
    private static readonly int _availableThreads = Environment.ProcessorCount;
    public static int GetAvailableThreads() {
        return _availableThreads;
    }
    public static float GetAvailableMemory() {
        using (var availableMemoryCounter = new PerformanceCounter("Memory", "Available MBytes")) {
            return availableMemoryCounter.NextValue();
        }
    }
}