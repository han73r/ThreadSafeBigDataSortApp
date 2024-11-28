using SorterApp.Interfaces;
using SorterApp.Services;

public class FileSorterFactory : IFileSorterFactory {
    public IFileSorter Create(ILogger logger, string inputFile, string outputFile) {
        return new FileSorter(logger, inputFile, outputFile);
    }
}