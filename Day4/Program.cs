
Part1();

Part2();

void Part1()
{
    var (randomNumbers, bingoCards) = LoadBingoGameFromFile("input.txt");

    BingoCard? winningBingoCard = null;

    foreach (var number in randomNumbers)
    {
        winningBingoCard = PlayTurn(number, bingoCards);

        if (winningBingoCard is not null)
        {
            break;
        }
    }

    Console.WriteLine($"Part 1");

    if (winningBingoCard is null)
    {
        Console.WriteLine("No winning bingo cards found with the given game input.");
        return;
    }

    winningBingoCard.Draw();

    Console.WriteLine($"Winning bingo card has a final score of {winningBingoCard.FinalScore}.");
}

void Part2()
{
    var (randomNumbers, bingoCards) = LoadBingoGameFromFile("input.txt");

    BingoCard? winningBingoCard = null;

    Stack<BingoCard> winningBingoCards = new Stack<BingoCard>();

    foreach (var number in randomNumbers)
    {
        winningBingoCard = PlayTurn(number, bingoCards);

        if (winningBingoCard is not null)
        {
            winningBingoCards.Push(winningBingoCard);
        }
    }

    var lastBingoCardToWin = winningBingoCards.Pop();

    Console.WriteLine($"Part 2");
    lastBingoCardToWin.Draw();

    Console.WriteLine($"The last bingo card to win has a final score of {lastBingoCardToWin.FinalScore}.");
}


BingoCard? PlayTurn(int number, IList<BingoCard> bingoCards)
{
    BingoCard? result = null;

    foreach (var bingoCard in bingoCards)
    {
        if (!bingoCard.HasBingo)
        {
            bingoCard.Mark(number);

            if (bingoCard.HasBingo)
            {
                result = bingoCard;
            }
        }
    }

    return result;
}

(IEnumerable<int> RandomNumbers, IList<BingoCard> BingoCards) LoadBingoGameFromFile(string path)
{
    using var fileReader = File.OpenRead(path) ?? throw new NullReferenceException();
    using var streamReader = new StreamReader(fileReader);

    var randomNumbers = streamReader.ReadLine()!
        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(y => Convert.ToInt32(y));

    // Empty line
    streamReader.ReadLine();

    var bingoCards = new List<BingoCard>();

    while (!streamReader.EndOfStream)
    {
        bingoCards.Add(ReadBingoCardFromFile(streamReader));

        // Empty line
        streamReader.ReadLine();
    }

    return (randomNumbers, bingoCards);
}

BingoCard ReadBingoCardFromFile(StreamReader streamReader)
{
    var bingoCard = new BingoCard();

    for (int y = 0; y < 5; y++)
    {
        var tiles = streamReader
            .ReadLine()!
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => new Tile(int.Parse(x)))
            .ToArray();

        bingoCard.AddRow(y, tiles);
    }

    return bingoCard;
}

record Tile
{
    public Tile(int value)
    {
        Value = value;
    }

    public bool Marked { get; set; } = false;

    public int Value { get; }
}

class BingoCard
{
    public BingoCard()
    {
        for (int x = 0; x < 5; x++)
        {
            Tiles[x] = new Tile[5];
        }
    }

    public Tile[][] Tiles { get; } = new Tile[5][];

    public int CardNumber { get; }

    public int FinalScore { get; private set; } = 0;

    public bool HasBingo { get; private set; } = false;

    public void AddRow(int horizontalRowPosition, Tile[] tiles)
    {
        for (int i = 0; i < 5; i++)
        {
            Tiles[i][horizontalRowPosition] = tiles[i];
        }
    }

    public void Mark(int number)
    {
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                if (Tiles[x][y].Value == number)
                {
                    Tiles[x][y].Marked = true;
                }
            }
        }

        HasBingo = CheckIfCardHasBingo();

        if (HasBingo)
        {
            FinalScore = CalculateFinalScore(number);
        }
    }

    public void Draw()
    {
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                var tile = Tiles[x][y];
                if (tile.Marked)
                {
                    Console.Write($"|({tile.Value,-2})");
                }
                else
                {
                    Console.Write($"|{tile.Value,-4}");
                }

            }

            Console.Write("|");
            Console.WriteLine();
        }
    }

    private bool CheckIfCardHasBingo()
    {
        for (int x = 0; x < 5; x++)
        {
            if (Tiles[x].All(tile => tile.Marked))
            {
                return true;
            };
        }

        for (int y = 0; y < 5; y++)
        {
            var verticalTiles = new List<Tile>();

            for (int x = 0; x < 5; x++)
            {
                verticalTiles.Add(Tiles[x][y]);
            }

            if (verticalTiles.All(tile => tile.Marked))
            {
                return true;
            }
        }

        return false;
    }

    private int CalculateFinalScore(int lastNumber)
    {
        int sum = 0;

        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                if (!Tiles[x][y].Marked)
                {
                    sum += Tiles[x][y].Value;
                }
            }
        }

        return sum * lastNumber;
    }
}