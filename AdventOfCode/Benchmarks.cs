using BenchmarkDotNet.Attributes;

namespace AdventOfCode;

[MemoryDiagnoser]
public class Benchmarks
{
    readonly string[] lines;

    public Benchmarks()
    {
        lines = File.ReadAllLines(@"Inputs\Day3.txt");
    }

    [Benchmark]
    public int Day3Part1() => Day3.RunPart1(lines);

    [Benchmark]
    public int Day3Part2() => Day3.RunPart2(lines);

    [Benchmark]
    public int Day3Part2Linq() => Day3.RunPart2Linq(lines);
}
