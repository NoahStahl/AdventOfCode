namespace AdventOfCode;

public static class Day16
{
    static int RunPart1(Bit[] input)
    {
        _ = Decode(input);
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

    static IReadOnlyList<Packet> Decode(ReadOnlySpan<Bit> content)
    {
        var header = DecodeHeader(content);
        if (header.Type == 4) // Literal
        {
            (var value, _) = DecodeLiteral(content[6..]);
            return new[] { new Packet(header, value) };
        }
        else // Operator
        {
            var packets = new List<Packet>();
            if (content[6].Value == 0)
            {
                int totalLength = content.ReadInt(7, 15);
                int parsedLength = 0, i = 22;
                while(parsedLength < totalLength)
                {
                    var subheader = DecodeHeader(content[i..]);
                    i += 6;
                    if (subheader.Type == 4)
                    {
                        var subliteral = DecodeLiteral(content[i..]);
                        int sublength = subliteral.length + 6;
                        i += sublength;
                        parsedLength += sublength;
                    }
                    else
                    {
                        // TODO
                    }
                }
            }
            else
            {
                var subPacketsCount = content.ReadInt(7, 11);
            }

            return packets;
        }

    }

    static Header DecodeHeader(ReadOnlySpan<Bit> bits)
    {
        return new(bits.ReadInt(0, 3), bits.ReadInt(3, 3));
    }

    static (int value, int length) DecodeLiteral(ReadOnlySpan<Bit> bits)
    {
        var chunks = new List<int>();
        for (int i = 0; i < bits.Length; i += 5)
        {
            chunks.Add(i);
            if (bits[i].Value == 0) break;
        }

        Span<Bit> valueBits = stackalloc Bit[chunks.Count * 4];
        for (int i = 0, v = 0; i < chunks.Count; i++)
        {
            for (int b = chunks[i] + 1; b <= chunks[i] + 4; b++)
            {
                valueBits[v++] = bits[b];
            }
        }
        ReadOnlySpan<Bit> result = valueBits;

        return (result.ReadInt(), result.Length + chunks.Count);
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

    static int ReadInt(this ReadOnlySpan<Bit> bits, int start, int length)
    {
        int value = 0;
        for (int i = start, p = length - 1; i < start + length; i++, p--)
        {
            value += bits[i].Value << p;
        }

        return value;
    }
    static int ReadInt(this ReadOnlySpan<Bit> bits) => bits.ReadInt(0, bits.Length);
}

[DebuggerDisplay("{Value}")]
public record struct Bit(byte Value);
public record struct Header(int Version, int Type);
public record Packet(Header Header, int? LiteralValue = null);
