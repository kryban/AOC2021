// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
var inputRaw = File.ReadAllLines(inputFile).ToList();
var initialPopulation = inputRaw.First().Split(',').Select(x => int.Parse(x)).ToList();

long MeasurePopulattion(int duration, List<int> initialPopulation)
{
    long[] timers = new long[9];
    long retval = 0;
    foreach (var timer in initialPopulation)
    {
        timers[timer]++;
    }

    for (int i = 0; i < duration; i++)
    {
        var newBirthsNewCycles = timers[0];
        timers[0] = timers[1];
        timers[1] = timers[2];
        timers[2] = timers[3];
        timers[3] = timers[4];
        timers[4] = timers[5];
        timers[5] = timers[6];
        timers[6] = timers[7] + newBirthsNewCycles;
        timers[7] = timers[8];
        timers[8] = newBirthsNewCycles;
    }

    timers.ToList().ForEach(subPopulation => retval += subPopulation);

    return retval;
}

Console.WriteLine($"Answer day 5, part 1: {MeasurePopulattion(80, initialPopulation)}");
Console.WriteLine($"Answer day 5, part 2: {MeasurePopulattion(256, initialPopulation)}");
Console.ReadKey();


