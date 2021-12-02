using System.Linq;

Part1();

Part2();

void Part1()
{
    var lookupTable = File
        .ReadAllLines("input.txt")
        .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        .ToLookup(key => key[0], value => int.Parse(value[1]));

    var horizontalPosition = lookupTable["forward"].Sum();

    var depthPosition = lookupTable["down"].Sum() + (lookupTable["up"].Sum() * -1);

    Console.WriteLine("Part 1:");
    Console.WriteLine($"Horizontal position is {horizontalPosition}");
    Console.WriteLine($"depth position is {depthPosition}.");
    Console.WriteLine($"Multiplication of the two is {horizontalPosition * depthPosition}.");
    Console.WriteLine();
}

void Part2()
{
    int horizontalPosition = 0;
    int depthPosition = 0;
    int aim = 0;

    var submarineInstructions = File
        .ReadAllLines("input.txt")
        .Select(x =>
        {
            var line = x.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return new KeyValuePair<string, int>(line[0], int.Parse(line[1]));
        });

    foreach (var instruction in submarineInstructions)
    {
        switch (instruction.Key)
        {
            case "forward":
                horizontalPosition += instruction.Value;
                depthPosition += aim * instruction.Value;
                break;

            case "up":
                aim -= instruction.Value;
                break;

            case "down":
                aim += instruction.Value;
                break;
        }
    }
    Console.WriteLine("Part 2:");
    Console.WriteLine($"Horizontal position is {horizontalPosition}");
    Console.WriteLine($"depth position is {depthPosition}.");
    Console.WriteLine($"Multiplication of the two is {horizontalPosition * depthPosition}.");
    Console.WriteLine();
}