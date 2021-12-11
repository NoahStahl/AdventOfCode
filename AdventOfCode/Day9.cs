namespace AdventOfCode;

public static class Day9
{
    public static int RunPart1(Map map)
    {
        int risk = 0;
        var lowPoints = FindLowPoints(map);
        foreach (var point in lowPoints)
        {
            risk += map.Values[point.Row, point.Column] + 1;
        }

        return risk;
    }

    public static int RunPart2(Map map)
    {
        var basinSizes = new List<int>();
        var queue = new Queue<Point>();
        var lowPoints = FindLowPoints(map);
        foreach (var point in lowPoints)
        {
            basinSizes.Add(ExploreBasin(point));
        }
        basinSizes.Sort();

        return basinSizes[^1] * basinSizes[^2] * basinSizes[^3];

        int ExploreBasin(Point start)
        {
            int basinSize = 0;
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                (int row, int col) = queue.Dequeue();
                int value = map.Values[row, col];
                if (row > 0 && value < map.Values[row - 1, col])
                {
                    Survey(row - 1, col);
                }
                if (col > 0 && value < map.Values[row, col - 1])
                {
                    Survey(row, col - 1);
                }
                if (row < map.LastRow && value < map.Values[row + 1, col])
                {
                    Survey(row + 1, col);
                }
                if (col < map.LastCol && value < map.Values[row, col + 1])
                {
                    Survey(row, col + 1);
                }
            }

            void Survey(int row, int col)
            {
                if (map.Values[row, col] < 9)
                {
                    queue.Enqueue(new(row, col));
                    map.Values[row, col] = -1;
                    basinSize++;
                }
            }

            return basinSize;
        }
    }

    public static (int answer1, int answer2) Run()
    {
        string[] input = File.ReadAllLines(@"Inputs\Day9.txt");
        var map = ParseInput(input);
        return (RunPart1(map), RunPart2(map));
    }

    static List<Point> FindLowPoints(Map map)
    {
        var lowPoints = new List<Point>();
        for (int r = 0; r <= map.LastRow; r++)
        {
            for (int c = 0; c <= map.LastCol; c++)
            {
                int value = map.Values[r, c];
                if (IsLowPoint(value, r, c))
                {
                    lowPoints.Add(new(r, c));
                }
            }
        }

        bool IsLowPoint(int value, int row, int col) =>
            (row == 0 || value < map.Values[row - 1, col]) &&
            (col == 0 || value < map.Values[row, col - 1]) &&
            (row == map.LastRow || value < map.Values[row + 1, col]) &&
            (col == map.LastCol || value < map.Values[row, col + 1]);

        return lowPoints;
    }

    static Map ParseInput(string[] input)
    {
        int rows = input.Length;
        int cols = input[0].Length;
        int[,] map = new int[rows, cols];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                map[r, c] = int.Parse(input[r].AsSpan(c, 1));
            }
        }

        return new(map, rows - 1, cols - 1);
    }

    public record Map(int[,] Values, int LastRow, int LastCol);
    public record struct Point(int Row, int Column);
}
