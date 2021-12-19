namespace AdventOfCode;

public static class Day13
{
    static int RunPart1(HashSet<Point> points, List<Fold> folds)
    {
        return Fold(points, folds[0]).Count;
    }

    static string RunPart2(HashSet<Point> points, List<Fold> folds)
    {
        foreach (var fold in folds)
        {
            points = Fold(points, fold);
        }

        int xMax = 0, yMax = 0;
        foreach (var point in points)
        {
            xMax = Math.Max(xMax, point.X);
            yMax = Math.Max(yMax, point.Y);
        }

        var grid = new bool[xMax + 1, yMax + 1];
        foreach (var point in points)
        {
            grid[point.X, point.Y] = true;
        }

        var code = new System.Text.StringBuilder(xMax);
        for (int y = 0; y <= yMax; y++)
        {
            for (int x = 0; x <= xMax; x++)
            {
                code.Append(grid[x, y] ? '#' : '.');
            }
            code.Append(Environment.NewLine);
        }

        return code.ToString();
    }

    public static (int answer1, string answer2) Run()
    {
        string[] input = File.ReadAllLines(@"Inputs\Day13.txt");
        (var points, var folds) = ParseInput(input);
        return (RunPart1(points, folds), RunPart2(points, folds));
    }

    static HashSet<Point> Fold(HashSet<Point> points, Fold fold)
    {
        var folded = new HashSet<Point>();
        foreach (var point in points)
        {
            if (fold.Axis == Axis.X && point.X > fold.Index)
            {
                int distance = point.X - fold.Index;
                folded.Add(new(fold.Index - distance, point.Y));
            }
            else if (fold.Axis == Axis.Y && point.Y > fold.Index)
            {
                int distance = point.Y - fold.Index;
                folded.Add(new(point.X, fold.Index - distance));
            }
            else
            {
                folded.Add(point);
            }
        }

        return folded;
    }

    static (HashSet<Point> points, List<Fold> folds) ParseInput(string[] input)
    {
        var points = new HashSet<Point>();
        var folds = new List<Fold>();

        foreach (var line in input)
        {
            if (line.StartsWith("fold"))
            {
                int eq = line.LastIndexOf('=');
                var axis = Enum.Parse<Axis>(line.AsSpan(eq - 1, 1), true);
                var index = int.Parse(line.AsSpan(eq + 1));
                folds.Add(new(axis, index));
            }
            else if (line.Length > 0)
            {
                int comma = line.IndexOf(',');
                var x = int.Parse(line.AsSpan(0, comma));
                var y = int.Parse(line.AsSpan(comma + 1));
                points.Add(new(x, y));
            }
        }

        return (points, folds);
    }
}

public enum Axis { X, Y }
public record Fold(Axis Axis, int Index);
public record Point(int X, int Y);