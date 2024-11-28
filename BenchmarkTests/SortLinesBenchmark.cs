using BenchmarkDotNet.Attributes;

namespace SorterApp.BenchmarkTests;
[MemoryDiagnoser]
public class SortLinesBenchmark {
    private Random random = new Random();
    private List<string> smallData;
    private List<string> mediumData;
    private List<string> largeData;

    [GlobalSetup]
    public void Setup() {
        Random random = new Random();
        smallData = GenerateTestData(100);
        mediumData = GenerateTestData(10000);
        largeData = GenerateTestData(100000);
    }
    private List<string> GenerateTestData(int count) {
        return Enumerable.Range(1, count)
            .Select(i => $"{random.Next(1, 100)}. Line {random.Next(1, 10000)}")
            .ToList();
    }
    [Benchmark]
    public void SortLinesNew_Small() => SortLinesNEW(smallData);
    [Benchmark]
    public void SortLinesNew_Medium() => SortLinesNEW(mediumData);
    [Benchmark]
    public void SortLinesNew_Large() => SortLinesNEW(largeData);
    [Benchmark]
    public void SortLines_Small() => SortLines(smallData);
    [Benchmark]
    public void SortLines_Medium() => SortLines(mediumData);
    [Benchmark]
    public void SortLines_Large() => SortLines(largeData);
    private List<string> SortLines(List<string> lines) {
        var parsedLines = lines.Select(line => {
            var xSpan = line.AsSpan();
            int dotIndex = xSpan.IndexOf('.');
            var text = xSpan.Slice(dotIndex + 2).ToString();
            int number = int.Parse(xSpan.Slice(0, dotIndex));
            return (text, number, line);
        }).ToList();
        parsedLines.Sort((a, b) => {
            int textComparison = string.Compare(a.text, b.text, StringComparison.Ordinal);
            if (textComparison != 0) return textComparison;
            return a.number.CompareTo(b.number);
        });
        return parsedLines.Select(item => item.line).ToList();
    }
    private List<string> SortLinesNEW(List<string> lines) {
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
        return lines;
    }
}