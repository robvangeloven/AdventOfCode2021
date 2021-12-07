
Part1();

Part2();

void Part1()
{
    var crabSubs = File
        .ReadAllText("input.txt")
        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(int.Parse)
        .GroupBy(x => x);

    (var bestPosition, var fuelNeeded) = CalculateBestHorizontalPosition(crabSubs, (crab, horizontalPosition) =>
    {
        return Math.Abs(crab - horizontalPosition);
    });

    Console.WriteLine("Part 1");
    Console.WriteLine($"Least fuel intensitive position is {bestPosition}. Total fuel used: {fuelNeeded}.");
}

void Part2()
{
    var crabSubs = File
        .ReadAllText("input.txt")
        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(int.Parse)
        .GroupBy(x => x);

    (var bestPosition, var fuelNeeded) = CalculateBestHorizontalPosition(crabSubs, (crab, horizontalPosition) =>
    {
        var distance = Math.Abs(crab - horizontalPosition);
        return (distance * (distance + 1)) / 2;
    });

    Console.WriteLine("Part 2");
    Console.WriteLine($"Least fuel intensitive position is {bestPosition}. Total fuel used: {fuelNeeded}.");
}

(int BestPosition, int FuelNeeded) CalculateBestHorizontalPosition(IEnumerable<IGrouping<int,int>> crabSubs, Func<int, int, int> distanceFunc)
{
    Dictionary<int, int> crabDistances = new Dictionary<int, int>();

    int highestHorizontalCrabPosition = crabSubs.Max(x => x.Key);

    for (int horizontalPosition = 0; horizontalPosition <= highestHorizontalCrabPosition; horizontalPosition++)
    {
        int totalDistanceToAllCrabs = 0;

        foreach (var targetCrab in crabSubs)
        {
            var distance = distanceFunc(targetCrab.Key, horizontalPosition);

            totalDistanceToAllCrabs += distance * targetCrab.Count();
        }

        crabDistances.Add(horizontalPosition, totalDistanceToAllCrabs);
    }

    var bestPosition = crabDistances.MinBy(x => x.Value);

    return (bestPosition.Key, bestPosition.Value);
}
