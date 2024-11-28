using SorterApp.Interfaces;
using SorterApp.Utilities;
using System.Collections.Concurrent;

namespace SorterApp.Services;
internal class FileCreator: IFileCreator {
    private readonly LineGenerator _lineGenerator;
    private readonly FileWriter _fileWriter;
    private readonly BlockingCollection<string[]> _chunkQueue;
    private readonly int _maxThreads;
    private readonly float _availableMemoryMB;
    private readonly int _chunkSizeMB = GlobalSettings.MIN_CHUNK_SIZE_MB;
    private readonly long _chunkSizeBytes;
    private readonly long _maxFileSizeBytes;
    private readonly string _filePath;
    public FileCreator(string fileName, int fileSizeMB) {
        _filePath = Path.Combine(GlobalSettings.RESULTS_FOLDER, fileName);
        _availableMemoryMB = SystemInfo.GetAvailableMemory();
        _maxThreads = SystemInfo.GetAvailableThreads();
        _maxFileSizeBytes = fileSizeMB * GlobalSettings.MB_TO_BYTE;
        _chunkSizeBytes = _chunkSizeMB * GlobalSettings.MB_TO_BYTE;
    
        _chunkQueue = new BlockingCollection<string[]>(_maxThreads);
        _lineGenerator = new LineGenerator(_chunkSizeBytes, _chunkQueue);
        _fileWriter = new FileWriter(_filePath, _chunkQueue, _maxFileSizeBytes);
    }
    public async Task CreateFileAsync() {
        ILogger.LogSystemInfo(_maxThreads, _availableMemoryMB, _chunkSizeBytes);
        await FileHelper.EnsureFileExistsAsync(_filePath);
        if (FileHelper.CheckAndHandleMaxFileSizeReached(_filePath, _maxFileSizeBytes)) return;

        _lineGenerator.Start();
        await _fileWriter.StartAsync();
        _lineGenerator.Stop();
    }
    public long GetCurrentFileSizeBytes() {
        return _fileWriter.GetCurrentFileSizeBytes();
    }
}