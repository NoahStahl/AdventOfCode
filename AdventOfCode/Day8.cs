namespace AdventOfCode;

public static class Day8
{
    public static int RunPart1(List<Entry> entries)
    {
        int mappedDigitCount = 0;
        foreach (var entry in entries)
        {
            foreach (var value in entry.Output)
            {
                mappedDigitCount += value.Length switch
                {
                    2 => 1,
                    4 => 4,
                    3 => 7,
                    7 => 8,
                    _ => 0,
                };
            }
        }

        return mappedDigitCount;
    }

    public static int RunPart2(List<Entry> entries)
    {
        int decodedSum = 0;
        foreach (var entry in entries)
        {
            var keys = DecodePatterns(entry.Patterns);
            int outputSum = 0;
            for (int i = 0, e = 3; i < 4; i++, e--)
            {
                int value = MapValue(keys, entry.Output[i]);
                outputSum += value * (int)Math.Pow(10, e);
            }
            decodedSum += outputSum;
        }

        return decodedSum;
    }

    public static (int answer1, int answer2) Run()
    {
        string[] input = File.ReadAllLines(@"Inputs\Day8.txt");
        var entries = ParseInput(input);
        return (RunPart1(entries), RunPart2(entries));
    }

    public static Key[] DecodePatterns(string[] patterns)
    {
        patterns = patterns.OrderBy(i => i.Length).ToArray();
        string pattern1 = patterns[0];
        string pattern7 = patterns[1];
        string pattern4 = patterns[2];
        string pattern8 = patterns[9];
        char top = pattern7.First(c => !pattern1.Contains(c));
        var middles = pattern4.Where(c => !pattern1.Contains(c));

        var pattern3 = patterns.First(i => i.Length == 5
            && pattern1.All(c => i.Contains(c))
            && middles.Any(c => i.Contains(c)));
        
        var topLeft = middles.First(c => !pattern3.Contains(c));
        var middle = middles.First(c => c != topLeft);
        var bottom = pattern3.First(c => c != top
            && !pattern1.Contains(c) && !pattern4.Contains(c));

        var pattern2 = patterns.First(i => i.Length == 5
            && i.Contains(bottom)
            && middles.Count(c => i.Contains(c)) == 1
            && pattern1.Count(c => i.Contains(c)) == 1);

        var bottomLeft = pattern2.First(c => c != top 
            && c != bottom && !pattern4.Contains(c));
        
        var pattern5 = patterns.First(i => i.Length == 5
            && i.Contains(bottom)
            && middles.All(c => i.Contains(c))
            && pattern1.Count(c => i.Contains(c)) == 1);

        var pattern6 = patterns.First(i => i.Length == 6
            && i.Contains(bottomLeft)
            && middles.All(c => i.Contains(c))
            && pattern1.Count(c => i.Contains(c)) == 1);
        
        var topRight = pattern1.First(c => !pattern6.Contains(c));
        var bottomRight = pattern1.First(c => c != topRight);

        var pattern9 = patterns.First(i => i.Length == 6
            && !i.Contains(bottomLeft)
            && middles.All(c => i.Contains(c))
            && pattern1.All(c => i.Contains(c)));

        var pattern0 = patterns.First(i => i.Length == 6
            && i.Contains(bottomLeft)
            && !i.Contains(middle)
            && pattern1.All(c => i.Contains(c)));

        Key[] keys = new Key[] 
        { 
            new(pattern0, 0),
            new(pattern1, 1),
            new(pattern2, 2),
            new(pattern3, 3),
            new(pattern4, 4),
            new(pattern5, 5),
            new(pattern6, 6),
            new(pattern7, 7),
            new(pattern8, 8),
            new(pattern9, 9)
        };

        return keys;
    }

    public static int MapValue(Key[] keys, string value)
    {
        return keys.Single(i => i.Pattern.Length == value.Length
            && i.Pattern.Intersect(value).Count() == value.Length).Value;
    }

    static List<Entry> ParseInput(string[] input)
    {
        var entries = new List<Entry>();
        foreach (var line in input)
        {
            var tokens = line.Split(" ");
            entries.Add(new(tokens[0..10], tokens[11..]));
        }

        return entries;
    }

    public record Entry(string[] Patterns, string[] Output);
    public record Key(string Pattern, int Value);
}
