namespace CrosswordGen;

using System;

public static class Display
{
    public static void PrintGrid(char[,] grid)
    {
        int size = grid.GetLength(0);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Console.Write(grid[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    public static bool PromptForNewPuzzle()
    {
        Console.Write("Generate another puzzle? Enter 'yes' or 'no': ");
        string response = Console.ReadLine();
        return response.Trim().ToLower() == "yes";
    }
}
