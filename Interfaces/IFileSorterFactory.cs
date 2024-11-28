namespace SorterApp.Interfaces;
public interface IFileSorterFactory {
    IFileSorter Create(ILogger logger, string inputFile, string outputFile);
}