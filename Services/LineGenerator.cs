using System.Collections.Concurrent;
using System.Text;

namespace SorterApp.Services;
public class LineGenerator {
    private readonly long _chunkSizeBytes;
    private readonly BlockingCollection<string[]> _chunkQueue;
    private readonly int _maxThreads;
    private bool _isStopped;
    public LineGenerator(long chunkSizeBytes, BlockingCollection<string[]> chunkQueue) {
        _chunkSizeBytes = chunkSizeBytes;
        _chunkQueue = chunkQueue;
        _maxThreads = Environment.ProcessorCount;
        _isStopped = false;
    }
    public void Stop() {
        _isStopped = true;
    }
    public void Start() {
        Task.Run(() => GenerateChunks());
    }
    private void GenerateChunks() {
        Parallel.For(0, _maxThreads, _ => {
            while (!_isStopped) {
                var chunk = GenerateChunk();
                if (chunk != null) {
                    _chunkQueue.Add(chunk);
                }
            }
            _chunkQueue.CompleteAdding();
        });
    }
    private string[] GenerateChunk() {
        var random = new Random();
        var lines = new List<string>();
        long totalSize = 0;
        while (totalSize < _chunkSizeBytes && !_isStopped) {
            string line = GenerateRandomLine(random);
            long lineSize = Encoding.UTF8.GetByteCount(line + Environment.NewLine);

            if (totalSize + lineSize > _chunkSizeBytes) {
                break;
            }
            lines.Add(line);
            totalSize += lineSize;
        }
        return lines.ToArray();
    }
    private string GenerateRandomLine(Random random) {
        int randomInt = random.Next(1, 100000);
        var sb = new StringBuilder();
        bool lastWasSpace = false;
        int maxLength = 100;

        for (int i = 0; i < maxLength; i++) {
            bool isSpace = random.Next(0, 4) == 0;
            if (isSpace && !lastWasSpace && i != 0 && i != maxLength - 1) {
                sb.Append(' ');
                lastWasSpace = true;
            } else if (!isSpace) {
                char randomChar = (random.Next(0, 2) == 0)
                    ? (char)random.Next(65, 91)
                    : (char)random.Next(97, 123); 
                sb.Append(randomChar);
                lastWasSpace = false;
            }
        }
        return $"{randomInt}. {sb}";
    }
}