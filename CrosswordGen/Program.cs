using CrosswordGen;
using System;
using System.Collections.Generic;

    while (true)
    {
        // Select 5 random words
        List<string> selectedWords = WordSelection.SelectRandomWords();
        Console.WriteLine("Selected Words for Crossword Puzzle:");
        selectedWords.ForEach(word => Console.WriteLine(word));

        // Initialize the crossword generator with the selected words
        CrosswordGenerator crosswordGenerator = new CrosswordGenerator(selectedWords, 15);

        // Generate the crossword puzzle
        bool success = crosswordGenerator.GenerateCrossword();

        // Display the final state of the grid
        if (success)
        {
            Display.PrintGrid(crosswordGenerator.GridManager.Grid);
        }

        // Prompt for generating a new puzzle
        if (!Display.PromptForNewPuzzle())
        {
            break;
        }
    }