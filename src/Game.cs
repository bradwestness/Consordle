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

        Console.Write($"Enter guess {_attempt}: ");

        while (string.IsNullOrEmpty(guess))
        {
            var input = Console.ReadLine();

            if (!Words.IsValid(input))
            {
                Console.WriteLine("Not in word list!");
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

        if (guess.Equals(Words.KonamiCode, StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine(_word);
        }
        else
        {
            for (var i = 0; i < guess.Length; i++)
            {
                Console.ForegroundColor = guess[i] switch
                {
                    char c when c == _word[i] => ConsoleColor.DarkGreen,
                    char c when _word.Contains(c) => ConsoleColor.DarkYellow,
                    _ => ConsoleColor.DarkGray
                };

                Console.Write(Char.ToUpper(guess[i]));
            }
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
    }

    private bool CheckForWin(string guess)
    {
        Console.WriteLine();

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
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(result);
            Console.ForegroundColor = ConsoleColor.White;
            return true;
        }

        if (RemainingAttempts < 1)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(_word.ToUpperInvariant());
            Console.ForegroundColor = ConsoleColor.White;
            return true;
        }

        return false;
    }
}
