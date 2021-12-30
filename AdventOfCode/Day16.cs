using System.Globalization;

namespace AdventOfCode;

public static class Day16
{
    static int RunPart1(Bit[] input)
    {
        var packet = Decode(input);
        return 0;
    }

    static int RunPart2(Bit[] input)
    {
        return 0;
    }

    public static (int answer1, int answer2) Run()
    {
        string input = File.ReadAllText(@"Inputs\Day16.txt");
        var parsedInput = ParseInput(input);
        return (RunPart1(parsedInput), RunPart2(parsedInput));
    }

    static Packet Decode(Bit[] content)
    {
        int version = ReadInt(content, 0, 3);
        int type = ReadInt(content, 3, 3);
        if (type == 4)
        {
            // Parse as literal
        }
        else
        {
            // Parse as operator
        }
        return new(version, type);
    }

    static Bit[] ParseInput(string input)
    {
        var bits = new Bit[input.Length * 4];
        for (int i = 0, offset = 0; i < input.Length; i++, offset += 4)
        {
            var hex = byte.Parse(input.AsSpan(i, 1), NumberStyles.HexNumber);
            for (int h = 3, b = 0; h >= 0; h--, b++)
            {
                bits[offset + b] = new((byte)((hex >> h) & 1));
            }
        }

        return bits;
    }

    static int ReadInt(Bit[] bits, int start, int length)
    {
        int value = 0;
        for (int i = start, p = length - 1; i < start + length; i++, p--)
        {
            value += bits[i].Value << p;
        }

        return value;
    }
}

public record struct Bit(byte Value);
public record Packet(int Version, int Type, int? LiteralValue = null);