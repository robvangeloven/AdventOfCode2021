
Part1();

Part2();

void Part1()
{
    var map = Map.LoadMap("input.txt", line => line.Start.X == line.End.X || line.Start.Y == line.End.Y);

    Console.WriteLine("Part 1");
    // map.Draw();
    Console.WriteLine($"Number of dangerous areas: {map.GetDangerousAreas()}.");
}

void Part2()
{
    var map = Map.LoadMap("input.txt", line => true);

    Console.WriteLine("Part 2");
    // map.Draw();
    Console.WriteLine($"Number of dangerous areas: {map.GetDangerousAreas()}.");
}

record Point
{
    public int X { get; init; }

    public int Y { get; init; }
}

record Line
{
    public Point Start { get; init; }

    public Point End { get; init; }
}

class Map
{
    public static Map LoadMap(string path, Func<Line, bool> lineFilter)
    {
        var lines = File.ReadAllLines(path)
            .Select(line =>
            {
                var points = line.Split("->", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                var pointA = points[0].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                Point A = new Point
                {
                    X = int.Parse(pointA[0]),
                    Y = int.Parse(pointA[1])
                };

                var pointB = points[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                Point B = new Point
                {
                    X = int.Parse(pointB[0]),
                    Y = int.Parse(pointB[1])
                };

                return new Line
                {
                    Start = A,
                    End = B,
                };
            })
            .Where(line => lineFilter(line));

        var maxX = 0;
        var maxY = 0;

        foreach (var line in lines)
        {
            maxX = Math.Max(maxX, Math.Max(line.Start.X, line.End.X));
            maxY = Math.Max(maxY, Math.Max(line.Start.Y, line.End.Y));
        }

        var map = new Map(maxX + 1, maxY + 1);

        foreach (var line in lines)
        {
            map.DrawLine(line);
        }

        return map;
    }

    private Map(int sizeX, int sizeY)
    {
        Grid = new int[sizeX, sizeY];

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                Grid[x, y] = 0;
            }
        }
    }
    public int[,] Grid { get; }

    private void DrawLine(Line line)
    {
        int deltaX = line.End.X - line.Start.X;
        int deltaY = line.End.Y - line.Start.Y;

        float steps;

        if (Math.Abs(deltaX) > Math.Abs(deltaY))
        {
            steps = Math.Abs(deltaX);
        }
        else
        {
            steps = Math.Abs(deltaY);
        }

        var stepX = deltaX / steps;
        var stepY = deltaY / steps;

        float xPoint = line.Start.X;
        float yPoint = line.Start.Y;

        for (int step = 0; step <= steps; step++)
        {
            Grid[(int)xPoint, (int)yPoint]++;

            xPoint += stepX;
            yPoint += stepY;
        }
    }

    public void Draw()
    {
        for (int y = 0; y < Grid.GetLength(1); y++)
        {
            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                var point = Grid[x, y] switch
                {
                    0 => ".".PadRight(2),
                    _ => $"{Grid[x, y],-2}",
                };

                Console.Write(point);
            }

            Console.WriteLine();
        }
    }

    public int GetDangerousAreas()
    {
        int dangerousAreas = 0;

        for (int y = 0; y < Grid.GetLength(1); y++)
        {
            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                if (Grid[x, y] > 1)
                {
                    dangerousAreas++;
                }
            }

        }
        return dangerousAreas;
    }
}