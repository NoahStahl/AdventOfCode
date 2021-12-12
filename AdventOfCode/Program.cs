namespace AdventOfCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, Advent of Code!");

            (var answer1, var answer2) = Day11.Run();

            Console.WriteLine($"Answer 1: {answer1}, Answer 2: {answer2}");
        }
    }
}
