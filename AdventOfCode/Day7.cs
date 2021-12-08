namespace AdventOfCode;

public static class Day7
{
    public static int RunPart1(int[] sortedPositions)
    {
        int median = FindMedian(sortedPositions);
        int fuelCost = 0;
        for (int i = 0; i < sortedPositions.Length; i++)
        {
            fuelCost += Distance(sortedPositions[i], median);
        }

        return fuelCost;
    }

    public static int RunPart2(int[] sortedPositions)
    {
        int median = FindMedian(sortedPositions);
        int fuelCost = CalculateMovementCost(sortedPositions, median);

        for (int l = median - 1; l > 0; l--)
        {
            int leftwardCost = CalculateMovementCost(sortedPositions, l);
            if (leftwardCost > fuelCost) break;
            fuelCost = leftwardCost;
        }

        for (int r = median + 1; r < sortedPositions[^1]; r++)
        {
            int rightwardCost = CalculateMovementCost(sortedPositions, r);
            if (rightwardCost > fuelCost) break;
            fuelCost = rightwardCost;
        }

        return fuelCost;
    }

    public static (int answer1, int answer2) Run()
    {
        string input = File.ReadAllText(@"Inputs\Day7.txt");
        int[] positions = input.ParseNumbers(',');
        Array.Sort(positions);
        return (RunPart1(positions), RunPart2(positions));
    }

    static int CalculateMovementCost(int[] positions, int target)
    {
        int fuelCost = 0;
        for (int i = 0; i < positions.Length; i++)
        {
            int distance = Distance(positions[i], target);
            for (int d = 0; d < distance; d++)
            {
                fuelCost += d + 1;
            }
        }

        return fuelCost;
    }

    static int Distance(int start, int end) => Math.Abs(start - end);

    static int FindMedian(int[] sortedValues)
    {
        int n = sortedValues.Length;
        return n % 2 == 0
            ? (sortedValues[(n / 2) - 1] + sortedValues[n / 2]) / 2
            : sortedValues[n / 2];
    }

    static int[] ParseNumbers(this string line, char delimiter)
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

        return values.ToArray();
    }
}
