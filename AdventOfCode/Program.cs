using AdventOfCode;
using BenchmarkDotNet.Running;

Console.WriteLine("Hello, Advent of Code!");

(int answer1, int answer2) = Day5.Run();

Console.WriteLine($"Answer 1: {answer1}, Answer 2: {answer2}");

//BenchmarkRunner.Run<Benchmarks>();