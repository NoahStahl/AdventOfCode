namespace AdventOfCode;

public static class Day5
{
    public static int RunPart1(string[] input)
    {
        Span<Segment> segments = stackalloc Segment[input.Length];
        int size = ParseInput(input, segments);

        return CountOverlaps(segments, size, false);
    }

    public static int RunPart2(string[] input)
    {
        Span<Segment> segments = stackalloc Segment[input.Length];
        int size = ParseInput(input, segments);

        return CountOverlaps(segments, size, true);
    }

    static int CountOverlaps(Span<Segment> segments, int size, bool diagonals)
    {
        int overlapCount = 0;
        var heatMap = new Dictionary<int, int>();
        foreach (var seg in segments)
        {
            if (seg.Start.X == seg.End.X)
            {
                (int startY, int endY) = MinMax(seg.Start.Y, seg.End.Y);
                for (int y = startY; y <= endY; y++)
                {
                    MapHeat(seg.Start.X + y * size);
                }
            }
            else if (seg.Start.Y == seg.End.Y)
            {
                (int startX, int endX) = MinMax(seg.Start.X, seg.End.X);
                int offset = seg.Start.Y * size;
                for (int x = startX; x <= endX; x++)
                {
                    MapHeat(x + offset);
                }
            }
            else if (diagonals)
            {
                int xStep = seg.Start.X < seg.End.X ? 1 : -1;
                int yStep = seg.Start.Y < seg.End.Y ? 1 : -1;
                int x = seg.Start.X, y = seg.Start.Y;
                int stepCount = Math.Abs(seg.Start.X - seg.End.X) + 1;
                for (int i = 0; i < stepCount; i++)
                {
                    MapHeat(x + y * size);
                    x += xStep;
                    y += yStep;
                }
            }
        }

        void MapHeat(int index)
        {
            heatMap.TryGetValue(index, out int value);
            heatMap[index] = ++value;
            if (value == 2) overlapCount++; // Count only first overlap
        }

        return overlapCount;
    }

    public static (int answer1, int answer2) Run()
    {
        string[] input = File.ReadAllLines(@"Inputs\Day5.txt");
        return (RunPart1(input), RunPart2(input));
    }

    static int ParseInput(string[] lines, Span<Segment> segments)
    {
        int xMax = 0, yMax = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            int comma = line.IndexOf(',');
            int stop = line.IndexOf(' ') - 1;
            int startX = int.Parse(line.AsSpan(0, comma));
            int startY = int.Parse(line.AsSpan(comma + 1, stop - comma));
            comma = line.LastIndexOf(',');
            int begin = line.LastIndexOf(' ') + 1;
            int endX = int.Parse(line.AsSpan(begin, comma - begin));
            int endY = int.Parse(line.AsSpan(comma + 1));
            segments[i] = new(new(startX, startY), new(endX, endY));
            xMax = Max(xMax, startX, endX);
            yMax = Max(yMax, startY, endY);
        }

        return xMax + 1; // Size (width / height)
    }

    static int Max(int x, int y, int z) => Math.Max(x, Math.Max(y, z));

    static (int, int) MinMax(int x, int y) => (Math.Min(x, y), Math.Max(x, y));

    public record struct Coordinate(int X, int Y);
    public record struct Segment(Coordinate Start, Coordinate End);
}
