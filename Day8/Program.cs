
Part1();

Part2();

void Part1()
{
    var segmentData = File
        .ReadAllLines("input.txt")
        .Select(x =>
        {
            var inputs = x.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var input = inputs[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var output = inputs[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            return new Tuple<IEnumerable<string>, IEnumerable<string>>(input, output);
        });

    var uniqueSegments = segmentData.Select(data => data.Item2.Count(output =>
    {
        return
        output.Length == 2 || // 1
        output.Length == 4 || // 4
        output.Length == 3 || // 7
        output.Length == 7;   // 8
    })).Sum();

    Console.WriteLine("Part 1");
    Console.WriteLine($"Unique segments: {uniqueSegments}");
}

void Part2()
{
    var segmentData = File
        .ReadAllLines("input.txt")
        .Select(x =>
        {
            var inputs = x.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var input = inputs[0]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => string.Concat(x.OrderBy(x => x)))
            .ToList();

            var output = inputs[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList()
            .Select(x => string.Concat(x.OrderBy(x => x)))
            .ToList();

            return (input, output);
        });

    int totalSegmentSum = 0;

    foreach(var segment in segmentData)
    {
        var decoder = CreateDecoder(segment.input);
        int segmentSize = segment.output.Count();
        int decodedSegment = 0;

        for (int i = 0; i < segmentSize; i++)
        {
            decodedSegment += (int)(Decode(decoder, segment.output[i]) * (Math.Pow(10, segmentSize - i - 1)));
        }

        totalSegmentSum += decodedSegment;
    }

    Console.WriteLine("Part 2");
    Console.WriteLine($"Total segments sum: {totalSegmentSum}.");

    int Decode(IDictionary<string, int> decoder, string cipherText)
    {
        return decoder[cipherText];
    }

    IDictionary<string, int> CreateDecoder(IEnumerable<string> cipher)
    {
        var decoder = new Dictionary<string, int>();

        string one = cipher.Single(x => x.Length == 2);
        decoder.Add(one, 1);

        string four = cipher.Single(x => x.Length == 4);
        decoder.Add(four, 4);

        string seven = cipher.Single(x => x.Length == 3);
        decoder.Add(seven, 7);

        string eight = cipher.Single(x => x.Length == 7);
        decoder.Add(eight, 8);

        string combined = string.Concat(seven.Union(four));
        string nine = cipher.Single(x => x.Length == 6 && x.Except(combined).Count() == 1);
        decoder.Add(nine, 9);

        string three = cipher.Single(x => x.Length == 5 && x.Except(seven).Count() == 2);
        decoder.Add(three, 3);

        string zero = cipher.Single(x => x.Length == 6 && x != nine && x.Except(seven).Count() == 3);
        decoder.Add(zero, 0);

        string six = cipher.Single(x => x.Length == 6 && x != nine && x.Except(zero).Count() == 1);
        decoder.Add(six, 6);

        string five = cipher.Single(x => x.Length == 5 && six.Except(x).Count() == 1);
        decoder.Add(five, 5);

        string two = cipher.Single(x => x.Length == 5 && x.Except(four).Count() == 3);
        decoder.Add(two, 2);

        return decoder;
    }
}