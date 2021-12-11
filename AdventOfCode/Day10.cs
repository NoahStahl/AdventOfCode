namespace AdventOfCode;

public static class Day10
{
    public static int RunPart1(string[] input)
    {
        int errorScore = 0;
        foreach (var line in input)
        {
            errorScore += EvaluateLine(line);
        }

        return errorScore;
    }

    public static long RunPart2(string[] input)
    {
        var scores = new List<long>();
        foreach (var line in input)
        {
            bool isValid = EvaluateLine(line) == 0;
            if (isValid)
            {
                scores.Add(CompleteLine(line));
            }
        }
        scores.Sort();

        return scores[scores.Count / 2];
    }

    public static (int answer1, long answer2) Run()
    {
        string[] input = File.ReadAllLines(@"Inputs\Day10.txt");
        return (RunPart1(input), RunPart2(input));
    }

    static long CompleteLine(string line)
    {
        long score = 0;
        var stack = new Stack<char>();
        foreach (char c in line)
        {
            if (IsOpener(c))
            {
                stack.Push(c);
            }
            else
            {
                stack.Pop();
            }
        }

        while (stack.TryPop(out char c))
        {
            int charScore = c switch
            {
                '(' => 1,
                '[' => 2,
                '{' => 3,
                _ => 4
            };
            score = score * 5 + charScore;
        }

        return score;
    }

    static int EvaluateLine(string line)
    {
        var stack = new Stack<char>();
        foreach (char c in line)
        {
            if (IsOpener(c))
            {
                stack.Push(c);
            }
            else if (c == ')' && stack.Pop() != '(')
            {
                return 3;
            }
            else if (c == ']' && stack.Pop() != '[')
            {
                return 57;
            }
            else if (c == '}' && stack.Pop() != '{')
            {
                return 1197;
            }
            else if (c == '>' && stack.Pop() != '<')
            {
                return 25137;
            }
        }

        return 0;
    }

    static bool IsOpener(char c)
    {
        return c == '(' || c == '[' || c == '{' || c == '<';
    }
}
