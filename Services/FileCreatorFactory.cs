using SorterApp.Interfaces;

namespace SorterApp.Services;
public class FileCreatorFactory : IFileCreatorFactory {
    public IFileCreator Create(string fileName, int fileSizeMB) {
        return new FileCreator(fileName, fileSizeMB);
    }
    public async Task<IFileCreator> CreateFileAsync(string fileName, int fileSizeMB) {
        var fileCreator = new FileCreator(fileName, fileSizeMB);
        await Task.Yield();
        return fileCreator;
    }
}