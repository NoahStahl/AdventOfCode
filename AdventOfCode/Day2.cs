namespace AdventOfCode;

public static class Day2
{
    public static int RunPart1(string[] commands)
    {
        (int position, int depth) = (0, 0);
        foreach (var command in commands)
        {
            (var direction, var distance) = ParseCommand(command);
            (position, depth) = direction switch
            {
                Direction.Up => (position, depth - distance),
                Direction.Down => (position, depth + distance),
                Direction.Forward => (position + distance, depth),
                _ => (position, depth)
            };
        }

        return position * depth;
    }

    public static int RunPart2(string[] commands)
    {
        (int position, int depth, int aim) = (0, 0, 0);
        foreach (var command in commands)
        {
            (var direction, var distance) = ParseCommand(command);
            (position, depth, aim) = direction switch
            {
                Direction.Up => (position, depth, aim - distance),
                Direction.Down => (position, depth, aim + distance),
                Direction.Forward => (position + distance, depth + aim * distance, aim),
                _ => (position, depth, aim)
            };
        }

        return position * depth;
    }

    public static (int answer1, int answer2) Run()
    {
        string[] commands = File.ReadAllLines(@"Inputs\Day2.txt");
        return (RunPart1(commands), RunPart2(commands));
    }

    public static (Direction direction, int distance) ParseCommand(string value)
    {
        int spaceIndex = value.LastIndexOf(" ");
        var direction = Enum.Parse<Direction>(value.AsSpan(0, spaceIndex), true);
        int distance = int.Parse(value.AsSpan(spaceIndex));

        return (direction, distance);
    }

    public enum Direction { Forward, Up, Down }
}
