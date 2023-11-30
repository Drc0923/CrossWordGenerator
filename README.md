# Crossword Generator

A simple yet powerful tool for generating crossword puzzles using a predefined list of words. Written in C#, this generator creates a puzzle by placing words on a grid in a crossword fashion, allowing intersections and ensuring that the resulting layout adheres to standard crossword rules.

## Features

- Random word selection from a hardcoded list
- Customizable grid size
- Automated word placement with valid intersections
- Checks for invalid adjacent word formations
- Interactive console-based user interface
- Retry mechanism for generating new puzzles

## How to Use

1. **Start the Application**: Run the application in your IDE or command line.
2. **Word Selection**: The application will automatically select 5 random words to generate a crossword puzzle.
3. **View Puzzle**: The generated puzzle, if successful, will be displayed in the console.
4. **Retry Option**: After each puzzle generation attempt, you can choose to generate another puzzle or exit the application.

## Components

### `WordSelection`

Responsible for selecting a random set of words from a predefined list to be placed on the crossword grid.

### `GridManager`

Manages the crossword grid, checks if words can be placed at specific positions without violating crossword rules, and places or removes words from the grid.

### `CrosswordGenerator`

The core logic of the application that tries to place words on the grid recursively and backtracks if necessary to find a valid layout.

### `Display`

Handles the printing of the crossword grid and prompts the user to generate a new puzzle or exit.

## How it Works

The `CrosswordGenerator` selects words and attempts to place them on the grid starting with the longest word. It uses a recursive backtracking algorithm to find a suitable position for each word. The `GridManager` ensures that each word placed does not create invalid crossword configurations by checking for invalid adjacent words and ensuring valid intersections.

## Customization

You can customize the list of words and the size of the grid by modifying the `hardcodedWords` list and the `GridManager` constructor parameter, respectively.

## Dependencies

This project is built using C# and .NET. Ensure you have the latest .NET SDK installed to compile and run the project.

## Getting Started

To get started with the Crossword Generator:

1. Clone the repository to your local machine.
2. Open the project in your preferred IDE that supports C# (e.g., Visual Studio, Rider).
3. Build the solution.
4. Run the application.


