// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
var inputRaw = File.ReadAllLines(inputFile).ToList();
var initialFishes = inputRaw.First().Split(',').Select(x => new Fish(int.Parse(x))).ToList();
var duration = 80;
var population = new List<Fish>(initialFishes);

for (int i = 0; i < duration; i++)
{
    foreach (var fish in new List<Fish>(population))
    {
        if(fish.Timer == 0)
        {
            fish.Timer = 6;
            population.Add(new Fish());
            continue;
        }

        fish.Timer--;
    }
}

Console.WriteLine($"Total days easured: {duration}");
Console.WriteLine($"Answer day 5, part 1: {population.Count}");
Console.ReadKey();

class Fish
{
    public Fish()
    {
        Timer = 8;
    }
    public Fish(int timer)
    {
        Timer = timer;
    }
    public int Timer { get; set; }
}

