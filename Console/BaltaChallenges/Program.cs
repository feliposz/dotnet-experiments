using System.Collections.Generic;

void Pause()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine();
    Console.WriteLine("============================");
    Console.WriteLine("Press any key to continue...");
    Console.WriteLine("============================");
    Console.ResetColor();
    Console.ReadKey(true);
}

void Primes(int maxPrime)
{
    Console.WriteLine($"Primes up until {maxPrime}:");

    List<int> primes = new List<int>();

    primes.Add(2);

    // fast "enough" up until 100000
    for (int n = 3; n < maxPrime; n += 2)
    {
        bool isPrime = true;
        foreach (int d in primes)
        {
            if (n % d == 0)
            {
                isPrime = false;
                break;
            }
        }
        if (isPrime)
        {
            primes.Add(n);
        }
    }

    int count = 0;
    foreach (int prime in primes)
    {
        Console.Write(prime + "\t");
        if (++count % 10 == 0)
        {
            Console.WriteLine();
        };
    }
}

void Fibonacci(int count)
{
    Console.WriteLine($"The first {count} numbers in the Fibonacci sequence:");
    int i = 1, j = 1;
    for (int num = 1; num <= count; num++)
    {
        Console.Write(i + "\t");
        (i, j) = (j, i + j);
        if (num % 10 == 0)
        {
            Console.WriteLine();
        };
    }
}

void Pyramid(int size)
{
    Console.WriteLine($"Pyramid with size: {size}");
    for (int row = 0; row < size; row++)
    {
        int spaces = size - row - 1;
        Console.WriteLine(new string(' ', spaces) + new string('#', row * 2 + 1));
    }
}

void MarioWorld()
{
    string[] map =
    {
        "....................................o...",
        "..o.o.o........................?...ooo..",
        ".ooooooo................................",
        "..o.o.o.................................",
        ".......o.......?.............#?#?#......",
        "@.....ooo...............................",
        "@@.....o........v.v.v...@@..........v...",
        "@@@.......>....vvvvvvv.@@@@...^....vvv..",
        "########################################",
        "########################################"
    };

    var tiles = new Dictionary<char, (ConsoleColor, ConsoleColor, char)>();

    tiles.Add('o', (ConsoleColor.Cyan, ConsoleColor.White, '@'));
    tiles.Add('.', (ConsoleColor.Cyan, ConsoleColor.Cyan, ' '));
    tiles.Add('@', (ConsoleColor.DarkGreen, ConsoleColor.Green, '^'));
    tiles.Add('v', (ConsoleColor.Green, ConsoleColor.DarkGreen, '^'));
    tiles.Add('>', (ConsoleColor.Red, ConsoleColor.DarkYellow, '☻'));
    tiles.Add('^', (ConsoleColor.DarkRed, ConsoleColor.Yellow, '☺'));
    tiles.Add('?', (ConsoleColor.DarkYellow, ConsoleColor.DarkRed, '?'));
    tiles.Add('#', (ConsoleColor.DarkRed, ConsoleColor.DarkYellow, '#'));

    Console.WriteLine("SuperMario");
    for (int i = 0; i < map.Length; i++)
    {
        for (int j = 0; j < map[i].Length; j++)
        {
            (ConsoleColor bg, ConsoleColor fg, char c) = tiles[map[i][j]];
            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
            Console.Write(c);
        }
        Console.ResetColor();
        Console.WriteLine();
    }
}

void Change(decimal cost, decimal cash)
{
    decimal[] noteValues = { 100.00M, 50.00M, 20.00M, 10.00M, 5.00M, 2.00M, 1.00M, 0.50M, 0.25M, 0.10M, 0.05M, 0.01M };
    var change = cash - cost;
    Console.WriteLine($"Cash: ${cash}\nCost: ${cost}\nChange: ${change}\nDivided in:");
    if (change <= 0) {
        Console.WriteLine("No change.");
    }
    var changeNotes = new Stack<decimal>();
    foreach (var noteValue in noteValues)
    {
        while (change >= noteValue)
        {
            changeNotes.Push(noteValue);
            change -= noteValue;
        }
    }
    decimal totalChange = 0;
    while (changeNotes.Count > 0)
    {
        int count = 0;
        decimal noteValue = 0;
        while (true)
        {
            noteValue = changeNotes.Pop();
            count++;
            totalChange += noteValue;
            if (changeNotes.Count == 0 || changeNotes.Peek() != noteValue)
            {
                break;
            }
        }
        string type = noteValue > 1M ? "bill" : "coin";
        string plural = count == 1 ? "" : "s";
        Console.WriteLine(String.Format("{0} {1}{2} of {3:C2}", count, type, plural, noteValue));
    }
}

Primes(100);
Pause();
Fibonacci(35);
Pause();
Pyramid(9);
Pause();
MarioWorld();
Pause();
Change(232.78M, 400M);
