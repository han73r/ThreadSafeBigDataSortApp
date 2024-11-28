using SorterApp.Interfaces;

namespace SorterApp.Utilities;
public static class FileHelper {
    public static async Task EnsureFileExistsAsync(string filePath) {
        try {
            if (!File.Exists(filePath)) {
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true)) {
                    await fs.FlushAsync();
                }
            }
        } catch (Exception ex) {
            throw new InvalidOperationException($"Failed to create the file: {filePath}", ex);
        }
    }
    public static bool CheckAndHandleMaxFileSizeReached(string filePath, long maxFileSizeBytes) {
        long currentSize = new FileInfo(filePath).Length;
        if (currentSize >= maxFileSizeBytes) {
            ILogger.LogError($"File \"{filePath}\" already has max Size: {maxFileSizeBytes / GlobalSettings.MB_TO_BYTE} MB.");
            return true;
        }
        return false;
    }
}