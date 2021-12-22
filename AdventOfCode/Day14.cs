namespace AdventOfCode;

public static class Day14
{
    static long RunPart1(Input input) => Polymerize(input, 10);

    static long RunPart2(Input input) => Polymerize(input, 40);

    public static (long answer1, long answer2) Run()
    {
        string[] input = File.ReadAllLines(@"Inputs\Day14.txt");
        var parsedInput = ParseInput(input);
        return (RunPart1(parsedInput), RunPart2(parsedInput));
    }

    static long Polymerize(Input input, int steps)
    {
        var elementCounts = new Dictionary<char, long>();
        foreach (char c in input.Template)
        {
            elementCounts.TryGetValue(c, out long count);
            elementCounts[c] = ++count;
        }

        var polymer = input.Polymer;
        for (int s = 0; s < steps; s++)
        {
            var result = new Polymer();
            foreach (KeyValuePair<Pair, long> item in polymer)
            {
                (char insert, Pair left, Pair right) = input.Rules[item.Key];
                result.TryGetValue(left, out long leftCount);
                result[left] = leftCount + item.Value;
                result.TryGetValue(right, out long rightCount);
                result[right] = rightCount + item.Value;

                elementCounts.TryGetValue(insert, out long count);
                elementCounts[insert] = count + item.Value;
            }

            polymer = result;
        }

        var sorted = elementCounts.OrderBy(i => i.Value).ToArray();

        return sorted[^1].Value - sorted[0].Value;
    }

    static Input ParseInput(string[] input)
    {
        string template = input[0];
        var polymer = new Polymer();
        for (int i = 0; i < template.Length - 1; i++)
        {
            var pair = new Pair(template[i], template[i + 1]);
            polymer.TryGetValue(pair, out long count);
            polymer[pair] = ++count;
        }

        var rules = new RuleMap();
        foreach (var line in input.Skip(2))
        {
            var source = new Pair(line[0], line[1]);
            char insert = line[^1];
            var leftResult = new Pair(source.Left, insert);
            var rightResult = new Pair(insert, source.Right);
            rules.Add(source, (insert, leftResult, rightResult));
        }

        return new(template, polymer, rules);
    }
}

public record struct Pair(char Left, char Right);
public class RuleMap : Dictionary<Pair, (char, Pair, Pair)> { };
public class Polymer : Dictionary<Pair, long> { };
public record Input(string Template, Polymer Polymer, RuleMap Rules);
