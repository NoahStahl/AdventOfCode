namespace AdventOfCode;

public static class Day1
{
    public static int Run()
    {
        const int windowSize = 3;

        string[] lines = File.ReadAllLines(@"Inputs\Day1.txt");

        int increaseCount = 0;
        int? previous = null;
        for (int i = 0; i <= lines.Length - windowSize; i++)
        {
            int windowSum = ParseInt(lines[i]) + ParseInt(lines[i + 1]) + ParseInt(lines[i + 2]);
            if (previous.HasValue && windowSum > previous)
            {
                increaseCount++;
            }

            previous = windowSum;
        }

        return increaseCount;
    }

    public static int ParseInt(string value) => int.Parse(value.Trim());
}
