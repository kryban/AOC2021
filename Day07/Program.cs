// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFilePart = @"./ProjectItems/input.txt";

(long position, long costs) CalculateFuelWithConstantBurnRate(string inputFile)
{
    long retval = 0;
    var inputRaw = File.ReadAllLines(inputFile).ToList();
    var crabsUnpositioned = inputRaw.First().Split(',').Select(x => long.Parse(x)).ToList();

    List<(long position, long fuelCosts)> allRouteCosts = new List<(long position, long fuelCosts)>();

    foreach(var crab in crabsUnpositioned)
    {
        long subtotal = 0;
        for (int i = 0; i < crabsUnpositioned.Count(); i++)
        {
            subtotal += Math.Abs(crabsUnpositioned[i] - crab);
        }

        allRouteCosts.Add((crab,subtotal));
    }

    return allRouteCosts.OrderBy(f =>f.fuelCosts).First();
}

(long position, long costs) CalculateFuelWithIncreasingBurnRate(string inputFile)
{
    var inputRaw = File.ReadAllLines(inputFile).ToList();
    var crabsUnpositioned = inputRaw.First().Split(',').Select(x => int.Parse(x)).ToList();

    List<(long position, long fuelCosts)> allRouteCosts = new List<(long position, long fuelCosts)>();

    var allPositions = crabsUnpositioned.Max() - crabsUnpositioned.Min();

    for (int currentPosition = crabsUnpositioned.Min(); currentPosition < allPositions; currentPosition++)
    {
        long subtotal = 0;

        foreach (var crab in crabsUnpositioned)
        {
            var distance = Math.Abs(currentPosition - crab);
            var subcosts = 0;

            if (distance > 0)
            {
                for (int l = 1; l <= distance; l++)
                {
                    subcosts += l;
                }

                subtotal += subcosts;
            }
        }

        allRouteCosts.Add((currentPosition, subtotal));
    }

    return allRouteCosts.OrderBy(f => f.fuelCosts).First();
}

var part1 = CalculateFuelWithConstantBurnRate(inputFilePart);
var part2 = CalculateFuelWithIncreasingBurnRate(inputFilePart);
Console.WriteLine($"Day 7, part 1: Cheapest move is to position {part1.position} costs {part1.costs} fuel.");
Console.WriteLine($"Day 7, part 2: Cheapest move is to position {part2.position} costs {part2.costs} fuel.");
Console.ReadKey();