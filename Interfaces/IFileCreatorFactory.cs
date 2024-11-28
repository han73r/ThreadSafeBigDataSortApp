namespace SorterApp.Interfaces;
public interface IFileCreatorFactory {
    Task<IFileCreator> CreateFileAsync(string filePath, int fileSizeMB);
}