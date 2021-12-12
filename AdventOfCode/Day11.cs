namespace AdventOfCode;

public static class Day11
{
    public static int RunPart1(string[] input)
    {
        int[,] grid = ParseInput(input);
        int flashCount = 0;
        for (int step = 0; step < 100; step++)
        {
            flashCount += DoStep(grid);
        }

        return flashCount;
    }

    public static int RunPart2(string[] input)
    {
        int[,] grid = ParseInput(input);
        int step = 1;
        while (DoStep(grid) < grid.Length)
        {
            step++;
        }

        return step;
    }

    public static (int answer1, int answer2) Run()
    {
        string[] input = File.ReadAllLines(@"Inputs\Day11.txt");
        return (RunPart1(input), RunPart2(input));
    }

    static int DoStep(int[,] grid)
    {
        int size = grid.GetLength(0), maxIndex = size - 1;
        var flashed = new HashSet<(int, int)>();

        ForEach((row, col) => AddEnergy(row, col));
        ForEach((row, col) => TryFlash(row, col));
        ForEach((row, col) => Reset(row, col));

        return flashed.Count;


        void AddEnergy(int row, int col) => grid[row, col]++;

        void TryFlash(int row, int col, bool propagated = false)
        {
            if (row < 0 || row > maxIndex || col < 0 || col > maxIndex)
            {
                return;
            }

            if (propagated)
            {
                grid[row, col]++;
            }

            if (!flashed.Contains((row, col)) && grid[row, col] >= 10)
            {
                flashed.Add((row, col));
                TryFlash(row, col + 1, true);
                TryFlash(row, col - 1, true);
                TryFlash(row - 1, col, true);
                TryFlash(row - 1, col + 1, true);
                TryFlash(row - 1, col - 1, true);
                TryFlash(row + 1, col, true);
                TryFlash(row + 1, col + 1, true);
                TryFlash(row + 1, col - 1, true);
            }
        }

        void Reset(int row, int col)
        {
            if (grid[row, col] > 9)
            {
                grid[row, col] = 0;
            }
        }

        void ForEach(Action<int, int> action)
        {
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    action(row, col);
                }
            }
        }
    }

    static int[,] ParseInput(string[] input)
    {
        int size = input.Length;
        int[,] grid = new int[size, size];
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                grid[row, col] = int.Parse(input[row].AsSpan(col, 1));
            }
        }

        return grid;
    }
}
