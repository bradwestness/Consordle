namespace Consordle;

public sealed class Game
{
    private const int MaxAttempts = 6;
    private string _word = string.Empty;
    private bool _isGameOver = false;
    private int _attempt = 0;

    public void Start()
    {
        _word = Words.GetRandom();
        _isGameOver = false;
        _attempt = 0;

        Console.WriteLine($"Guess the CONSORDLE in {MaxAttempts} tries.");
        Console.WriteLine("Each guess must be a valid 5-letter word. Hit the enter button to submit.");

        do
        {
            _attempt++;
            var guess = GetNextGuess();
            PrintGuessResult(guess);
            _isGameOver = CheckForWin(guess);
        } while (!_isGameOver);

        Console.WriteLine();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    public int RemainingAttempts => MaxAttempts - _attempt;

    private string GetNextGuess()
    {
        string? guess = null;

        Console.WriteLine();
        Console.Write($"Enter guess {_attempt}: ");

        while (string.IsNullOrEmpty(guess))
        {
            var input = Console.ReadLine();

            if (!Words.IsValid(input))
            {
                Console.WriteLine("Not in word list!");
                Console.WriteLine();
                Console.Write("Enter guess: ");
                continue;
            }

            guess = input;
        }

        return guess.Trim().ToUpperInvariant();
    }

    private void PrintGuessResult(string guess)
    {
        Console.WriteLine();
        Console.Write("\t");

        if (guess.Equals(Words.KonamiCode, StringComparison.OrdinalIgnoreCase))
        {
            Console.Write(_word);
        }
        else
        {
            var letters = BuildLetterDictionary(_word);

            for (var i = 0; i < guess.Length; i++)
            {
                char c = guess[i];

                if (c == _word[i])
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    letters[c]--;
                }
                else if (letters.ContainsKey(c) && letters[c] > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    letters[c]--;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }

                Console.Write(char.ToUpper(c));
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        Console.WriteLine();
    }

    private static IDictionary<char, int> BuildLetterDictionary(string word)
    {
        var dictionary = new Dictionary<char, int>();

        foreach (char c in word)
        {
            if (!dictionary.ContainsKey(c))
            {
                dictionary.Add(c, 0);
            }

            dictionary[c]++;
        }

        return dictionary;
    }

    private bool CheckForWin(string guess)
    {
        if (guess.Equals(_word, StringComparison.OrdinalIgnoreCase))
        {
            var result = RemainingAttempts switch
            {
                0 => "Phew!",
                1 => "Great!",
                2 => "Splendid!",
                3 => "Impressive!",
                4 => "Magnificent!",
                _ => "Genius!",
            };
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine();
            Console.WriteLine("\t" + result);
            Console.ForegroundColor = ConsoleColor.White;
            return true;
        }

        if (RemainingAttempts < 1)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("\t" + _word.ToUpperInvariant());
            Console.ForegroundColor = ConsoleColor.White;
            return true;
        }

        return false;
    }
}
