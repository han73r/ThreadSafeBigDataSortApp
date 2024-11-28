using System.Collections.Concurrent;

public class ChunkQueueService {
    private readonly BlockingCollection<string[]> _queue;
    public ChunkQueueService(int capacity) {
        _queue = new BlockingCollection<string[]>(capacity);
    }
    public void AddChunk(string[] chunk) {
        _queue.Add(chunk);
    }
    public string[] TakeChunk() {
        return _queue.Take();
    }
    public void CompleteAdding() {
        _queue.CompleteAdding();
    }
    public bool IsAddingCompleted => _queue.IsAddingCompleted;
}