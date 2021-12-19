namespace AdventOfCode;
using VisitFilter = Func<Cave, Path, bool>;

public static class Day12
{
    static int RunPart1(List<Cave> caves)
    {
        static bool filter(Cave c, Path path) => c.Big || !path.Contains(c);

        var paths = FindPaths(caves.First(c => c.Start), new(), filter);
        return paths.Count;
    }

    static int RunPart2(List<Cave> caves)
    {
        static bool filter(Cave cave, Path path)
        {
            if (cave.Start) return false;
            int smallRepeats = 0;
            foreach (var pathCave in path)
            {
                if (!pathCave.Big && !pathCave.Start)
                {
                    var count = path.Count(c => c == pathCave);
                    if (count > 1 && smallRepeats++ > 1) return false;
                }
            }

            return true;
        }

        var paths = FindPaths(caves.First(c => c.Start), new(), filter);
        return paths.Count;
    }

    public static (int answer1, int answer2) Run()
    {
        var caves = new Dictionary<string, Cave>();
        string[] input = File.ReadAllLines(@"Inputs\Day12.txt");
        foreach (var segment in input)
        {
            string[] ends = segment.Split('-');
            Cave start = caves.TryGetValue(ends[0], out var startCave)
                ? startCave
                : new(ends[0]);
            Cave end = caves.TryGetValue(ends[1], out var endCave)
                ? endCave
                : new(ends[1]);

            start.Neighbors.Add(end);
            end.Neighbors.Add(start);
            caves[start.Id] = start;
            caves[end.Id] = end;
        }

        var caveList = caves.Values.ToList();
        return (RunPart1(caveList), RunPart2(caveList));
    }

    static List<Path> FindPaths(Cave cave, Path path, VisitFilter filter)
    {
        path.Add(cave);
        if (cave.End) return new() { path };

        var paths = new List<Path>();
        foreach (var neighbor in cave.Neighbors)
        {
            if (filter(neighbor, path))
            {
                var forks = FindPaths(neighbor, path.Fork(), filter);
                paths.AddRange(forks);
            }
        }

        return paths;
    }
}

public record Cave
{
    public Cave(string id)
    {
        Id = id;
        Big = id[0] <= 'Z';
        Start = id == "start";
        End = id == "end";
    }

    public string Id { get; }
    public bool Big { get; }
    public bool Start { get; }
    public bool End { get; }
    public List<Cave> Neighbors { get; } = new();
}

public class Path : List<Cave>
{
    public Path() { }
    public Path(Path path) : base(path) { }
    public Path Fork() => new(this);
}
