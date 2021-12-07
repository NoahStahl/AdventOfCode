using AdventOfCode;
using BenchmarkDotNet.Running;

Console.WriteLine("Hello, Advent of Code!");

(long answer1, long answer2) = Day6.Run();

Console.WriteLine($"Answer 1: {answer1}, Answer 2: {answer2}");

//BenchmarkRunner.Run<Benchmarks>();