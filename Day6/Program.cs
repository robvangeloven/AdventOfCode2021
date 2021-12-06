
using System.Diagnostics;
using System.Numerics;

Part1();

Part2();

void Part1()
{
    var fishes = File.ReadAllText("input.txt")
        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(int.Parse)
        .ToList();

    var newSpawn = new List<int>();

    for (int day = 0; day < 80; day++)
    {
        for (int i = 0; i < fishes.Count; i++)
        {
            if (fishes[i] == 0)
            {
                fishes[i] = 6;
                newSpawn.Add(8);
            }
            else
            {
                fishes[i]--;
            }
        }

        fishes.AddRange(newSpawn);
        newSpawn.Clear();
    }

    Console.WriteLine("Part 1");
    Console.WriteLine($"Number of fishes: {fishes.Count}.");
}

void Part2()
{
    var bucketsOfFish = new long[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    int generations = 256;

    File.ReadAllText("input.txt")
        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(int.Parse)
        .ToList()
        .ForEach(fish => bucketsOfFish[fish]++);

    for (int generation = 0; generation < generations; generation++)
    {
        var spawningFish = bucketsOfFish[0];

        for (int i = 0; i < 8; i++)
        {
            bucketsOfFish[i] = bucketsOfFish[i + 1];
        }

        bucketsOfFish[6] += spawningFish;

        bucketsOfFish[8] = spawningFish;
    }
    
    Console.WriteLine("Part 2");
    Console.WriteLine($"Number of fishes: {bucketsOfFish.Sum()}.");
}

