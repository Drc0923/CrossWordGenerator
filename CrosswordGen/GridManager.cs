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

        for (int i = 0; i < word.Length; i++)
        {
            int currentRow = direction == "horizontal" ? row : row + i;
            int currentCol = direction == "horizontal" ? col + i : col;
            char letter = word[i];

            // Check if the current position is already occupied by a different letter
            if (IsAdjacentInvalid(currentRow, currentCol, direction, letter))
                return false;

            // Check if placing the word here would result in invalid adjacent configurations
            if (!IsValidIntersection(currentRow, currentCol, letter))
                return false;
        }

        return true; // All checks passed, word can be placed
    }

    private bool IsValidIntersection(int row, int col, char letter)
    {
        // Check if the cell is empty, which is a valid intersection point.
        if (Grid[row, col] == '.')
            return true;

        // If the cell has the same letter, it could be a valid intersection.
        if (Grid[row, col] == letter)
        {
            // Check if the letter is part of an existing word in the placedWordsInfo list.
            return placedWordsInfo.Any(pwi => pwi.Word.Contains(letter) &&
                                              ((pwi.Direction == "horizontal" && pwi.Row == row) ||
                                               (pwi.Direction == "vertical" && pwi.Column == col)));
        }

        // If the cell has a different letter, it's not a valid intersection.
        return false;
    }

    private bool FormsNewWord(int row, int col, string currentWordDirection, char letter)
    {
        // Determine the direction to check for new word formation.
        int[] rowOffsets = currentWordDirection == "horizontal" ? new int[] {-1, 1} : new int[] {0, 0};
        int[] colOffsets = currentWordDirection == "vertical" ? new int[] {-1, 1} : new int[] {0, 0};

        // Check the cells before and after the current cell in the direction perpendicular to the word being placed.
        for (int i = 0; i < 2; i++)
        {
            int newRow = row + rowOffsets[i];
            int newCol = col + colOffsets[i];

            // Skip the check if it's out of bounds.
            if (newRow < 0 || newRow >= Size || newCol < 0 || newCol >= Size)
                continue;

            // If the adjacent cell has a letter, it might form a new word.
            if (Grid[newRow, newCol] != '.')
            {
                return true;
            }
        }

        // No new words are formed.
        return false;
    }


private bool IsAdjacentInvalid(int row, int col, string direction, char letter)
{
    // Check in the perpendicular direction of the word placement for any adjacent letters.
    int dRow = direction == "horizontal" ? 1 : 0;
    int dCol = direction == "vertical" ? 1 : 0;

    // Check before and after the current position.
    for (int d = -1; d <= 1; d += 2)
    {
        int newRow = row + d * dRow;
        int newCol = col + d * dCol;

        // Continue if it's out of bounds.
        if (newRow < 0 || newRow >= Size || newCol < 0 || newCol >= Size)
            continue;

        char adjacentCell = Grid[newRow, newCol];
        
        // If the adjacent cell is not empty and not the same letter, and also not part of a valid intersection,
        // it's an invalid placement as it would form a new word or extend an existing word incorrectly.
        if (adjacentCell != '.' && adjacentCell != letter && !IsValidIntersection(newRow, newCol, adjacentCell))
            return true;
    }

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

