using System.Text;

namespace AdventOfCode;

public static class Day15
{
    static int RunPart1(Map map)
    {
        return BestPath(map);
    }

    static int RunPart2(Map map)
    {
        return BestPath(Expand(map, 5));
    }

    public static (int answer1, int answer2) Run()
    {
        string[] input = File.ReadAllLines(@"Inputs\Day15.txt");
        var parsedInput = ParseInput(input);
        return (RunPart1(parsedInput), RunPart2(parsedInput));
    }

    static int BestPath(Map map)
    {
        var queue = new PriorityQueue<(int, int), int>();
        var distances = new int[map.Values.Length];
        Array.Fill(distances, int.MaxValue);
        var visited = new bool[map.Values.Length];
        distances[0] = 0;
        Visit(0, 0, 0);

        do
        {
            (int x, int y) = queue.Dequeue();
            var currentDistance = distances[x + y * map.Size];
            Visit(x - 1, y, currentDistance);
            Visit(x + 1, y, currentDistance);
            Visit(x, y - 1, currentDistance);
            Visit(x, y + 1, currentDistance);
        }
        while (queue.Count > 0);

        void Visit(int x, int y, int sourceDistance)
        {
            var index = x + y * map.Size;
            if (!InMap(x, y) || visited[index]) return;
            var weight = map.Values[x, y];
            var proposedDistance = sourceDistance + weight;
            distances[index] = Math.Min(proposedDistance, distances[index]);
            queue.Enqueue((x, y), distances[index]);
            visited[index] = true;
        }

        bool InMap(int x, int y)
        {
            return x >= 0 && y >= 0 && x < map.Size && y < map.Size;
        }

        return distances[map.Values.Length - 1];
    }

    static Map Expand(Map input, int tiles)
    {
        int size = input.Size;
        int newSize = input.Size * tiles;
        int[,] expanded = new int[newSize, newSize];
        for (int tr = 0; tr < tiles; tr++)
        {
            for (int tc = 0; tc < tiles; tc++)
            {
                for (int r = 0; r < size; r++)
                {
                    for (int c = 0; c < size; c++)
                    {
                        var value = input.Values[c, r] + tr + tc;
                        value = value <= 9 ? value : (value % 9);
                        expanded[c + tc * size, r + tr * size] = value;
                    }
                }
            }
        }

        return new(expanded, newSize);
    }

    static Map ParseInput(string[] input)
    {
        int size = input.Length;
        int[,] map = new int[size, size];
        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                map[c, r] = input[r][c] - '0';
            }
        }

        return new(map, size);
    }
}

public record Map(int[,] Values, int Size);