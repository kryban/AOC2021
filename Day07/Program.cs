// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFilePart1 = @"./ProjectItems/input.txt";


long CalculateFuel(string inputFile)
{
    long retval = 0;
    var inputRaw = File.ReadAllLines(inputFile).ToList();
    var crabsUnpositioned = inputRaw.First().Split(',').Select(x => long.Parse(x)).ToList();

    List<long> allRouteCosts = new List<long>();

    foreach(var crab in crabsUnpositioned)
    {
        long subtotal = 0;
        for (int i = 0; i < crabsUnpositioned.Count(); i++)
        {
            subtotal += Math.Abs(crabsUnpositioned[i] - crab);
        }

        allRouteCosts.Add(subtotal);
    }

    return allRouteCosts.Min();
}

Console.WriteLine($"Day 7, part 1: {CalculateFuel(inputFilePart1)}");
Console.ReadKey();