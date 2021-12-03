
Part1();

Part2();

void Part1()
{
    var lines = File.ReadAllLines("input.txt");
    var dataSize = lines.First().Length;

    var input = lines.SelectMany(x =>
            x.ToCharArray()
            .Select(y => Convert.ToInt32(y.ToString(), 2)))
        .ToList();

    var inputs = new List<List<int>>();

    int epsilonBitMask = 1;

    for (int i = 0; i < dataSize; i++)
    {
        inputs.Add(new List<int>());
        epsilonBitMask |= 1 << i;
    }

    for (int i = 0; i < input.Count(); i += dataSize)
    {
        for (int j = 0; j < dataSize; j++)
        {
            inputs[j].Add(input[i + j]);
        }
    }

    int gammaRate = 0;

    for (int i = 0; i < inputs.Count; i++)
    {
        gammaRate |= ((inputs[i].Sum() - (inputs[i].Count / 2.0)) > 0 ? 1 : 0) << (dataSize - i - 1);
    }

    var epsilonRate = gammaRate ^ epsilonBitMask;

    Console.WriteLine("Part 1");
    Console.WriteLine($"Gamma rate: {gammaRate}");
    Console.WriteLine($"Epsilon rate: {epsilonRate}");
    Console.WriteLine($"Power consumption: {gammaRate * epsilonRate}");
}

void Part2()
{
    var lines = File.ReadAllLines("input.txt");
    var dataSize = lines.First().Length;

    var input = lines.Select(x => Convert.ToInt32(x, 2)).ToList();


    var oxygen = FilterValue(input, dataSize, 0, x => x >= 0 ? 1 : 0);
    var co2 = FilterValue(input, dataSize, 0, x => x >= 0 ? 0 : 1);

    Console.WriteLine("Part 2");
    Console.WriteLine($"Oxygen generator rating: {oxygen}");
    Console.WriteLine($"CO2 scrubber rating: {co2}");
    Console.WriteLine($"Life support rating: {oxygen * co2}");
}

int FilterValue(List<int> inputs, int dataSize, int filterPosition, Func<double, int> filter)
{
    int inputsCount = inputs.Count;

    var sum = 0;
    var shiftValue = (dataSize - filterPosition - 1);

    Func<int, int> bitSelector = input => (input >> shiftValue) & 1;

    foreach (var input in inputs)
    {
        sum += bitSelector(input);
    }

    int filterValue = filter(sum - (inputsCount / 2.0));

    var newInputs = inputs.Where(x => bitSelector(x) == filterValue).ToList();
    inputsCount = newInputs.Count;

    if (inputsCount > 1)
    {
        return FilterValue(newInputs, dataSize, filterPosition + 1, filter);
    }

    return newInputs.First();
}
