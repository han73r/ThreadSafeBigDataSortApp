using System.Collections.Concurrent;
using System.Text;

public class FileWriter {
    private readonly string _filePath;
    private readonly BlockingCollection<string[]> _chunkQueue;
    private readonly long _maxFileSizeBytes;
    private long _currentFileSizeBytes;
    public FileWriter(string filePath, BlockingCollection<string[]> chunkQueue, long maxFileSizeBytes) {
        _filePath = filePath;
        _chunkQueue = chunkQueue;
        _maxFileSizeBytes = maxFileSizeBytes;
        _currentFileSizeBytes = 0;
    }
    public async Task StartAsync() {
        if (File.Exists(_filePath)) {
            _currentFileSizeBytes = new FileInfo(_filePath).Length;
        }
        using (var writer = new StreamWriter(_filePath, append: true)) {
            await ProcessChunksAsync(writer);
        }
    }
    private async Task ProcessChunksAsync(StreamWriter writer) {
        while (!_chunkQueue.IsAddingCompleted) {
            if (_chunkQueue.TryTake(out string[] chunk)) {
                long chunkSize = GetChunkSize(chunk);
                if (_currentFileSizeBytes + chunkSize > _maxFileSizeBytes) {
                    await HandlePartialChunkAsync(writer, chunk, chunkSize);
                    break;
                }
                WriteChunk(writer, chunk);
                _currentFileSizeBytes += chunkSize;
            }
        }
    }
    private long GetChunkSize(string[] chunk) {
        return chunk.Sum(line => Encoding.UTF8.GetByteCount(line + Environment.NewLine));
    }
    private async Task HandlePartialChunkAsync(StreamWriter writer, string[] chunk, long chunkSize) {
        long remainingSize = _maxFileSizeBytes - _currentFileSizeBytes;
        if (remainingSize > 0) {
            WritePartialChunk(writer, chunk, remainingSize);
        }
    }
    private void WriteChunk(StreamWriter writer, string[] chunk) {
        foreach (var line in chunk) {
            writer.WriteLine(line);
        }
    }
    private void WritePartialChunk(StreamWriter writer, string[] chunk, long remainingSize) {
        long currentSize = 0;
        foreach (var line in chunk) {
            long lineSize = Encoding.UTF8.GetByteCount(line + Environment.NewLine);
            if (currentSize + lineSize > remainingSize) {
                var partialLine = line.Substring(0, (int)(remainingSize - currentSize));
                writer.WriteLine(partialLine);
                break;
            }
            writer.WriteLine(line);
            currentSize += lineSize;
        }
    }
    public long GetCurrentFileSizeBytes() {
        return _currentFileSizeBytes;
    }
}