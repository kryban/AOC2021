// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";

var input = File.ReadAllLines(inputFile).ToList();
int countIncreased = 0;

int previous = Convert.ToInt32(input[0]);

input.ForEach(x => IncreaseCounter(Convert.ToInt32(x), ref previous, ref countIncreased));

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



