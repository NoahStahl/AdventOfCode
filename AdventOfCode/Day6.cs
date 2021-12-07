namespace AdventOfCode;

public static class Day6
{
    public static long RunPart1(List<int> initialTimers)
    {
        return Simulate(initialTimers, 80);
    }

    public static long RunPart2(List<int> initialTimers)
    {
        return Simulate(initialTimers, 256);
    }

    public static (long answer1, long answer2) Run()
    {
        string input = File.ReadAllText(@"Inputs\Day6.txt");
        List<int> initialTimers = input.ParseNumbers(',');
        return (RunPart1(initialTimers), RunPart2(initialTimers));
    }

    static long Simulate(List<int> initialTimers, int days)
    {
        Span<long> cohorts = stackalloc long[9];
        foreach (int timer in initialTimers)
        {
            cohorts[timer]++;
        }

        do
        {
            long spawnCount = cohorts[0];
            for (int i = 1; i <= 8; i++)
            {
                cohorts[i - 1] = cohorts[i];
            }

            cohorts[6] += spawnCount; 
            cohorts[8] = spawnCount;
        } 
        while (--days > 0);

        long total = 0;
        for (int i = 0; i <= 8; i++)
        {
            total += cohorts[i];
        }

        return total;
    }

    static List<int> ParseNumbers(this string line, char delimiter)
    {
        List<int> values = new(line.Length / 2);
        for (int i = 1, d = 0; i < line.Length;)
        {
            if (line[i] == delimiter)
            {
                int start = d > 0 ? d + 1 : 0;
                if (i > start)
                {
                    values.Add(int.Parse(line.AsSpan(start, i - start)));
                }
                d = i;
            }

            if (++i == line.Length)
            {
                values.Add(int.Parse(line.AsSpan(d + 1)));
            }
        }

        return values;
    }
}
