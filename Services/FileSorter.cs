using SorterApp.Interfaces;
using System.Diagnostics;
using SorterApp.Utilities;
using System.Collections.Concurrent;

namespace SorterApp.Services;
class FileSorter : IFileSorter {
    private static float memoryUsagePercent = 0.8f;
    private readonly string _inputFilePath;
    private readonly string _outputFilePath;
    private readonly float _availableMemory;
    private readonly int _availableThreads;
    private readonly long _chunkSize;
    private readonly ILogger _logger;
    public FileSorter(ILogger logger, string inputFileName, string outputFileName) {
        _inputFilePath = Path.Combine(GlobalSettings.RESULTS_FOLDER, inputFileName);
        _outputFilePath = Path.Combine(GlobalSettings.RESULTS_FOLDER, outputFileName);
        _availableThreads = SystemInfo.GetAvailableThreads();
        _availableMemory = SystemInfo.GetAvailableMemory();
        _chunkSize = (long)(_availableMemory * GlobalSettings.MB_TO_BYTE * memoryUsagePercent) / _availableThreads;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public void Sort() {
        ILogger.LogSystemInfo(_availableThreads, _availableMemory, _chunkSize);
        var stopwatch = Stopwatch.StartNew();
        _logger.Log(Level.INFO, "Start SplitAndSortChunks");

        List<string> tempFiles = SplitAndSortChunks(_inputFilePath, _chunkSize, _availableThreads);

        stopwatch.Stop();
        TimeSpan elapsed = stopwatch.Elapsed;
        _logger.Log(Level.INFO, $"Stop SplitAndSortChunks. Elapsed time: {elapsed.Minutes} min {elapsed.Seconds} sec {elapsed.Milliseconds} ms.");
        stopwatch = Stopwatch.StartNew();
        _logger.Log(Level.INFO, "Start MergeChunks");

        MergeChunks(tempFiles, _outputFilePath);

        stopwatch.Stop();
        elapsed = stopwatch.Elapsed;
        _logger.Log(Level.INFO, $"Stop MergeChunks. Elapsed time: {elapsed.Minutes} min {elapsed.Seconds} sec {elapsed.Milliseconds} ms.");
    }
    private List<string> SplitAndSortChunks(string inputFilePath, long chunkSize, int availableThreads) {
        var tempFiles = new ConcurrentBag<string>();
        var tasks = new List<Task>();
        var semaphore = new SemaphoreSlim(availableThreads);
        chunkSize = _chunkSize;
        using (var reader = new StreamReader(inputFilePath)) {
            while (!reader.EndOfStream) {
                var lines = new List<string>();
                int currentSize = 0;
                while (currentSize < chunkSize && !reader.EndOfStream) {
                    var line = reader.ReadLine();
                    lines.Add(line);
                    currentSize += line.Length + Environment.NewLine.Length;
                }
                semaphore.Wait();
                var task = Task.Run(() => {
                    try {
                        var threadId = Task.CurrentId;
                        _logger.Log(Level.DEBUG, $"Started processing chunk", threadId);
                        var sortedLines = SortLines(lines, threadId);
                        string tempFile = WriteChunkToFile(sortedLines);
                        tempFiles.Add(tempFile);
                        _logger.Log(Level.DEBUG, $"Finished processing chunk", threadId);
                    } catch (Exception ex) {
                        var threadId = Task.CurrentId;
                        _logger.Log(Level.ERROR, $"Error processing chunk: {ex.Message}", threadId);
                    } finally {
                        semaphore.Release();
                    }
                });
                tasks.Add(task);
            }
            Task.WhenAll(tasks).Wait();
        }
        _logger.Log(Level.DEBUG, $"Finished processing all chunks. Created {tempFiles.Count} temporary files.");
        return tempFiles.ToList();
    }
    /// <summary>
    /// in-place sorting
    /// </summary>
    private List<string> SortLines(List<string> lines, int? threadId) {
        var stopwatch = Stopwatch.StartNew();
        _logger.Log(Level.DEBUG, "Start SortLines", threadId);
        lines.Sort((x, y) => {
            var xSpan = x.AsSpan();
            var ySpan = y.AsSpan();
            int xDotIndex = xSpan.IndexOf('.');
            int yDotIndex = ySpan.IndexOf('.');
            var xText = xSpan.Slice(xDotIndex + 2);
            var yText = ySpan.Slice(yDotIndex + 2);
            int textComparison = xText.CompareTo(yText, StringComparison.Ordinal);
            if (textComparison != 0) {
                return textComparison;
            }
            int xNumber = int.Parse(xSpan.Slice(0, xDotIndex));
            int yNumber = int.Parse(ySpan.Slice(0, yDotIndex));
            return xNumber.CompareTo(yNumber);
        });

        stopwatch.Stop();
        TimeSpan elapsed = stopwatch.Elapsed;
        _logger.Log(Level.DEBUG, "Stop SortLines", threadId);
        _logger.Log(Level.DEBUG, $"Elapsed time: {elapsed.Minutes} min {elapsed.Seconds} sec {elapsed.Milliseconds} ms.");
        return lines;
    }
    private string WriteChunkToFile(List<string> lines) {
        string tempFile = Path.GetTempFileName();
        File.WriteAllLines(tempFile, lines);
        return tempFile;
    }
    private void MergeChunks(List<string> tempFiles, string outputFilePath) {
        var readers = tempFiles.Select(file => new StreamReader(file)).ToList();
        var priorityQueue = new PriorityQueue<(string line, int fileIndex), string>();
        using (var writer = new StreamWriter(outputFilePath)) {
            for (int i = 0; i < readers.Count; i++) {
                if (!readers[i].EndOfStream) {
                    string line = readers[i].ReadLine();
                    priorityQueue.Enqueue((line, i), line);
                }
            }
            while (priorityQueue.Count > 0) {
                var minEntry = priorityQueue.Dequeue();
                writer.WriteLine(minEntry.line);
                int index = minEntry.fileIndex;
                if (!readers[index].EndOfStream) {
                    string nextLine = readers[index].ReadLine();
                    priorityQueue.Enqueue((nextLine, index), nextLine);
                }
            }
        }
        foreach (var reader in readers) reader.Dispose();
        foreach (var file in tempFiles) File.Delete(file);
    }
}