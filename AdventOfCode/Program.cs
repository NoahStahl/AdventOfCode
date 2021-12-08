using AdventOfCode;
using BenchmarkDotNet.Running;

Console.WriteLine("Hello, Advent of Code!");

(var answer1, var answer2) = Day7.Run();

Console.WriteLine($"Answer 1: {answer1}, Answer 2: {answer2}");

//BenchmarkRunner.Run<Benchmarks>();