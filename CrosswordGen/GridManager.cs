namespace CrosswordGen;

using System;

public class GridManager
{
    public int Size { get; private set; }
    public char[,] Grid { get; private set; }
    
    private List<PlacedWordInfo> placedWordsInfo;

    public GridManager(int size = 15)
    {
        Size = size;
        ResetGrid();
        placedWordsInfo = new List<PlacedWordInfo>();
    }

    public void ResetGrid()
    {
        Grid = new char[Size, Size];
        for (int i = 0; i < Size; i++)
        for (int j = 0; j < Size; j++)
            Grid[i, j] = '.';
    }

    public bool CanPlaceWord(string word, int row, int col, string direction)
    {
        int wordLength = word.Length;

        // Check if the word goes out of bounds
        if (direction == "horizontal" && col + wordLength > Size) return false;
        if (direction == "vertical" && row + wordLength > Size) return false;

        for (int i = 0; i < wordLength; i++)
        {
            int currentRow = direction == "horizontal" ? row : row + i;
            int currentCol = direction == "horizontal" ? col + i : col;

            // Check if the current position is already occupied by a different letter
            if (Grid[currentRow, currentCol] != '.' && Grid[currentRow, currentCol] != word[i])
                return false;

            // Check for adjacent words which would form invalid placements
            if (IsAdjacentInvalid(currentRow, currentCol, direction))
                return false;
        }

        return true;
    }
    
    private bool IsAdjacentInvalid(int row, int col, string direction)
    {
        // Directions to check based on current word direction
        int[] dx = direction == "horizontal" ? new int[] { -1, 1 } : new int[] { 0, 0 };
        int[] dy = direction == "horizontal" ? new int[] { 0, 0 } : new int[] { -1, 1 };

        for (int d = 0; d < 2; d++)
        {
            int adjacentRow = row + dx[d], adjacentCol = col + dy[d];

            // Check bounds
            if (adjacentRow < 0 || adjacentRow >= Size || adjacentCol < 0 || adjacentCol >= Size)
                continue;

            // If adjacent cell is not empty and not part of an intersecting word, it's invalid
            if (Grid[adjacentRow, adjacentCol] != '.' && !IsValidIntersection(adjacentRow, adjacentCol, Grid[adjacentRow, adjacentCol], placedWordsInfo))
                return true;
        }

        return false;
    }

    
    private bool IsValidIntersection(int row, int col, char letter, List<PlacedWordInfo> placedWords)
    {
        // Check if the intersecting letter is part of a word in the opposite direction
        foreach (var placedWord in placedWords)
        {
            if (placedWord.Word.Contains(letter))
            {
                // Calculate the index of the letter in the placed word
                int letterIndex = placedWord.Word.IndexOf(letter);

                // Check if the intersection is at the correct position for a horizontal word
                if (placedWord.Direction == "horizontal" && placedWord.Row == row &&
                    col >= placedWord.Column && col < placedWord.Column + placedWord.Word.Length &&
                    col == placedWord.Column + letterIndex)
                {
                    return true;
                }
                // Check if the intersection is at the correct position for a vertical word
                else if (placedWord.Direction == "vertical" && placedWord.Column == col &&
                         row >= placedWord.Row && row < placedWord.Row + placedWord.Word.Length &&
                         row == placedWord.Row + letterIndex)
                {
                    return true;
                }
            }
        }

        // If no valid intersection is found with the opposite direction words, it's not valid
        return false;
    }

    private bool IsAdjacentToOtherWord(int row, int col, char letter)
    {
        // Check if the placement would create a new word by being adjacent to a letter that is not part of an intersection
        int[] dr = { -1, 1, 0, 0 };
        int[] dc = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            int adjRow = row + dr[i], adjCol = col + dc[i];
            if (adjRow >= 0 && adjRow < Size && adjCol >= 0 && adjCol < Size)
            {
                if (Grid[adjRow, adjCol] != '.' && Grid[adjRow, adjCol] != letter)
                    return true;
            }
        }

        return false;
    }

    public void PlaceWord(string word, int row, int col, string direction)
    {
        if (!CanPlaceWord(word, row, col, direction))
            throw new ArgumentException("Cannot place word on grid at specified position and direction");

        PlacedWordInfo placedWordInfo = new PlacedWordInfo
        {
            Word = word,
            Row = row,
            Column = col,
            Direction = direction
        };

        for (int i = 0; i < word.Length; i++)
        {
            if (direction == "horizontal")
                Grid[row, col + i] = word[i];
            else // vertical
                Grid[row + i, col] = word[i];
        }

        placedWordsInfo.Add(placedWordInfo);
    }

    public void RemoveWord(string word, int row, int col, string direction)
    {
        for (int i = 0; i < word.Length; i++)
        {
            if (direction == "horizontal")
                Grid[row, col + i] = '.';
            else // vertical
                Grid[row + i, col] = '.';
        }

        placedWordsInfo.RemoveAll(p => p.Word == word && p.Row == row && p.Column == col && p.Direction == direction);
    }


    public void PrintGrid()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
                Console.Write(Grid[i, j] + " ");
            Console.WriteLine();
        }
    }
}

