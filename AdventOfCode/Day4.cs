namespace AdventOfCode;

public static class Day4
{
    public static int RunPart1(string[] input)
    {
        (var drawpile, var boards) = ParseInput(input);

        foreach (int number in drawpile)
        {
            if (boards.HasWinners(number, out var winners))
            {
                return winners[0].CalculateScore(number);
            }
        }

        return -1;
    }

    public static int RunPart2(string[] input)
    {
        (var drawpile, var boards) = ParseInput(input);

        var allWinners = new List<Board>();
        int lastWinningNumber = 0;
        foreach (int number in drawpile)
        {
            if (boards.HasWinners(number, out var winners))
            {
                allWinners.AddRange(winners);
                lastWinningNumber = number;
            }
        }

        return allWinners[^1].CalculateScore(lastWinningNumber);
    }

    public static (int answer1, int answer2) Run()
    {
        string[] input = File.ReadAllLines(@"Inputs\Day4.txt");
        return (RunPart1(input), RunPart2(input));
    }

    static (List<int>, List<Board>) ParseInput(string[] input)
    {
        var drawpile = input[0].ParseNumbers(',');
        var boards = new List<Board>();
        var currentGrid = new List<int>();
        for (int i = 2, size = 0; i < input.Length;)
        {
            currentGrid.AddRange(input[i].ParseNumbers(' '));
            size++;

            if (++i == input.Length || string.IsNullOrWhiteSpace(input[i]))
            {
                boards.Add(new(currentGrid, size));
                currentGrid = new(currentGrid.Count);
                size = 0;
                i++;
            }
        }

        return (drawpile, boards);
    }

    static List<int> ParseNumbers(this string line, char delimiter)
    {
        List<int> values = new(line.Length / 2);
        for (int i = 1, d = 0; i < line.Length;)
        {
            if (line[i] == delimiter)
            {
                int start = d > 0 ? d + 1 : 0;
                if (i > start)
                {
                    values.Add(int.Parse(line.AsSpan(start, i - start)));
                }
                d = i;
            }

            if (++i == line.Length)
            {
                values.Add(int.Parse(line.AsSpan(d + 1)));
            }
        }

        return values;
    }

    static bool HasWinners(this List<Board> boards, int number, out List<Board>? winners)
    {
        winners = null;
        foreach (var board in boards)
        {
            if (board.Won) continue;

            board.PlayNumber(number);
            int winningSum = -1 * board.Size;

            for (int row = 0; row < board.Size; row++)
            {
                int rowSum = 0, rowStart = row * board.Size;
                for (int col = 0; col < board.Size && rowSum <= 0; col++)
                {
                    rowSum += board.Grid[rowStart + col];
                }

                if (rowSum == winningSum)
                {
                    board.Won = true;
                    (winners ?? new()).Add(board);
                }
            }

            for (int col = 0; col < board.Size; col++)
            {
                int colSum = 0;
                for (int row = 0; row < board.Size && colSum <= 0; row++)
                {
                    colSum += board.Grid[col + row * board.Size];
                }

                if (colSum == winningSum)
                {
                    board.Won = true;
                    (winners ?? new()).Add(board);
                }
            }
        }

        return winners?.Count > 0;
    }

    public class Board
    {
        public Board(List<int> grid, int size)
        {
            Grid = grid;
            Size = size;
        }

        public List<int> Grid { get; set; }
        public int Size { get; set; }
        public bool Won { get; set; }

        public int CalculateScore(int winningNumber)
        {
            int unmarkedSum = 0;
            foreach (int value in Grid)
            {
                unmarkedSum += value >= 0 ? value : 0;
            }

            return unmarkedSum * winningNumber;
        }

        public void PlayNumber(int value)
        {
            for (int i = 0; i < Grid.Count; i++)
            {
                if (Grid[i] == value)
                {
                    Grid[i] = -1;
                }
            }
        }
    }
}
