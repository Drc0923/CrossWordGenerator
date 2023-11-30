namespace CrosswordGen;

using System;
using System.Collections.Generic;
using System.Linq;

public class CrosswordGenerator
{
    private List<string> words;
    private GridManager gridManager;
    private List<PlacedWordInfo> placedWordsInfo;

    public GridManager GridManager => gridManager;

    public CrosswordGenerator(List<string> words, int gridSize = 15)
    {
        this.words = words.OrderByDescending(w => w.Length).ToList();
        gridManager = new GridManager(gridSize);
        placedWordsInfo = new List<PlacedWordInfo>();
    }

    public bool GenerateCrossword()
    {
        if (PlaceWord(0))
        {
            Console.WriteLine("Crossword Puzzle Generated Successfully.");
            return true;
        }
        else
        {
            Console.WriteLine("Failed to generate a crossword puzzle with the given words.");
            return false;
        }
    }

    private bool PlaceWord(int index)
    {
        if (index >= words.Count)
            return true;

        string word = words[index];
        var positions = PotentialPositions(word);
        
        foreach (var (row, col, direction) in positions)
        {
            if (gridManager.CanPlaceWord(word, row, col, direction))
            {
                gridManager.PlaceWord(word, row, col, direction);
                placedWordsInfo.Add(new PlacedWordInfo { Word = word, Row = row, Column = col, Direction = direction });
                if (PlaceWord(index + 1))
                    return true;

                placedWordsInfo.RemoveAll(p => p.Word == word && p.Row == row && p.Column == col && p.Direction == direction);
                gridManager.RemoveWord(word, row, col, direction);
            }
        }

        return false;
    }



    private List<string> DynamicSort()
    {
        return words.OrderByDescending(word => IntersectionScore(word)).ToList();
    }


    private int IntersectionScore(string word)
    {
        int score = 0;
        foreach (char c in word)
        {
            foreach (var placed in placedWordsInfo)
            {
                if (placed.Word.Contains(c))
                    score++;
            }
        }
        return score;
    }


    private IEnumerable<(int row, int col, string direction)> PotentialPositions(string word)
    {
        var positions = new List<(int, int, string)>();

        // If there are no placed words, place the first word in the center horizontally.
        if (placedWordsInfo.Count == 0)
        {
            int mid = gridManager.Size / 2;
            positions.Add((mid, mid - word.Length / 2, "horizontal"));
        }
        else
        {
            // For subsequent words, look for intersections with already placed words.
            for (int i = 0; i < word.Length; i++)
            {
                // Look for the current letter in the grid
                char currentLetter = word[i];
                for (int row = 0; row < gridManager.Size; row++)
                {
                    for (int col = 0; col < gridManager.Size; col++)
                    {
                        if (gridManager.Grid[row, col] == currentLetter)
                        {
                            // Check if we can place the word horizontally with the current letter intersecting
                            if (col - i >= 0 && col - i + word.Length <= gridManager.Size)
                            {
                                positions.Add((row, col - i, "horizontal"));
                            }

                            // Check if we can place the word vertically with the current letter intersecting
                            if (row - i >= 0 && row - i + word.Length <= gridManager.Size)
                            {
                                positions.Add((row - i, col, "vertical"));
                            }
                        }
                    }
                }
            }
        }

        return positions;
    }
}