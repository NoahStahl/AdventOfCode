namespace AdventOfCode;

public static class Day3
{
    public static int RunPart1(string[] lines)
    {
        Span<int> ones = stackalloc int[lines[0].Length];
        int valueCount = CountOnes(lines, ones);

        int gamma = 0, epsilon = 0;
        int digits = ones.Length;
        for (int i = 0, n = digits - 1; i < digits; i++, n--)
        {
            int placeValue = 1 << n; // 2^n
            (gamma, epsilon) = ones[i] > valueCount / 2
                ? (gamma + placeValue, epsilon)
                : (gamma, epsilon + placeValue);
        }

        return gamma * epsilon;
    }

    public static int RunPart2(string[] lines)
    {
        int oxygen = CalculateRating(useMajority: true);
        int co2 = CalculateRating(useMajority: false);

        return oxygen * co2;

        int CalculateRating(bool useMajority)
        {
            int rating = 0, digits = lines[0].Length;
            Span<int> ones = stackalloc int[lines[0].Length];
            Span<bool> filter = stackalloc bool[lines.Length];
            for (int n = 0; n < digits; n++)
            {
                int valueCount = CountOnes(lines, ones, filter);
                bool onesMajority = ones[n] * 2 >= valueCount;
                char filterBy = useMajority
                    ? onesMajority ? '1' : '0'
                    : onesMajority ? '0' : '1';
                for (int i = 0; i < lines.Length; i++)
                {
                    if (!filter[i] && lines[i][n] != filterBy)
                    {
                        filter[i] = true;
                    }
                }

                if (TryGetOnly(lines, filter, out var result))
                {
                    rating = result;
                    break;
                }
            }

            return rating;
        }
    }

    public static (int answer1, int answer2) Run()
    {
        string[] lines = File.ReadAllLines(@"Inputs\Day3.txt");
        return (RunPart1(lines), RunPart2(lines));
    }

    static int CountOnes(string[] lines, Span<int> ones, Span<bool> filter)
    {
        ones.Clear();
        int width = lines[0].Length;
        int valueCount = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            if (!filter.IsEmpty && filter[i]) continue;
            for (int n = 0; n < width; n++)
            {
                ones[n] += lines[i][n] == '1' ? 1 : 0;
            }
            valueCount++;
        }

        return valueCount;
    }

    static int CountOnes(string[] lines, Span<int> ones)
    {
        return CountOnes(lines, ones, Array.Empty<bool>());
    }

    static bool TryGetOnly(string[] lines, Span<bool> filter, out int result)
    {
        int last = filter.LastIndexOf(false);
        result = filter.IndexOf(false) == last
            ? Convert.ToInt32(lines[last], 2)
            : -1;
        return result != -1;
    }

    // For perf comparison
    public static int RunPart2Linq(string[] lines)
    {
        int oxygen = CalculateRating(useMajority: true);
        int co2 = CalculateRating(useMajority: false);

        return oxygen * co2;

        int CalculateRating(bool useMajority)
        {
            int rating = 0, digits = lines[0].Length;
            Span<int> ones = stackalloc int[lines[0].Length];
            var items = lines;
            for (int n = 0; n < digits; n++)
            {
                int valueCount = CountOnes(items, ones);
                bool onesMajority = ones[n] * 2 >= valueCount;
                char filterBy = useMajority
                    ? onesMajority ? '1' : '0'
                    : onesMajority ? '0' : '1';
                items = items.Where(i => i[n] == filterBy).ToArray();

                if (items.Length == 1)
                {
                    rating = Convert.ToInt32(items[0], 2);
                    break;
                }
            }

            return rating;
        }
    }
}
