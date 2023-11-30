namespace CrosswordGen;

using System;
using System.Collections.Generic;
using System.Linq;

public static class WordSelection
{
    private static readonly List<string> hardcodedWords = new List<string>
    {
        "apple", "banana", "cherry", "date", "elderberry",
        "fig", "grape", "honeydew", "kiwi", "lemon",
        "mango", "nectarine", "orange", "papaya",
        "raspberry", "strawberry", "tangerine", "vanilla",
        "watermelon", "yam", "zucchini", "pumpkin", "pawpaw",
        "blueberry",
    };

    public static List<string> SelectRandomWords(int numWords = 5)
    {
        var validWords = hardcodedWords.Where(word => word.Length >= 2 && word.Length <= 10).ToList();
        Random rand = new Random();
        return validWords.OrderBy(x => rand.Next()).Take(numWords).ToList();
    }
}
