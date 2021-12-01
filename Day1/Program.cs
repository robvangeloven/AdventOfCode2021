Part1();

Part2();

void Part1()
{
    int increases = 0;

    File.ReadAllLines("input1.txt")
        .Select(x => int.Parse(x))
        .Aggregate((previous, current) =>
        {
            if (current > previous)
            {
                increases++;
            }
            return current;
        });

    Console.WriteLine($"Part 1: Depth increased {increases} times.");
}

void Part2()
{
    int increases = 0;

    int windowSize = 3;
    var items = File.ReadAllLines("input2.txt").Select(x => int.Parse(x)).ToArray();

    for(int index = windowSize + 1; index <= items.Length; index++)
    {
        var previousWindowSum = SumWindow(new Span<int>(items, index - windowSize -1, windowSize));
        var currentWindowSum = SumWindow(new Span<int>(items, index - windowSize, windowSize));

        if(currentWindowSum > previousWindowSum)
        {
            increases++;
        }
    }

    Console.WriteLine($"Part 2: Depth increased {increases} times.");
}

int SumWindow(Span<int> window)
{
    var result = 0;

    foreach (var item in window)
    {
        result += item;
    }

    return result;
}