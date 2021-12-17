// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";

var input = File.ReadAllLines(inputFile).Select(x => int.Parse(x)).ToList();
int countIncreased = 0;

int previous = input[0];

input.Skip(1).ToList().ForEach(current => IncreaseCounter(current, ref previous, ref countIncreased));

Console.WriteLine($"Answer: {countIncreased}");
Console.ReadKey();


static void IncreaseCounter(int current, ref int previous, ref int counter)
{
    {
        if (current > previous)
        {
            counter++;
        }

        previous = current;
    }
}



