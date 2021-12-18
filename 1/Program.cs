// See https://aka.ms/new-console-template for more information

string inputFile = @"./ProjectItems/input.txt";

var input = File.ReadAllLines(inputFile).Select(x => int.Parse(x)).ToList();
int countIncreased = 0;

int previous = 0;
var index = 1;

input.ToList().ForEach(x => Counter(index++, 1, ref countIncreased, input, ref previous));
Console.WriteLine($"Answer Part 1: {countIncreased}");

previous = 0;
countIncreased = 0;
index = 1;

input.ToList().ForEach(x => Counter(index++, 3, ref countIncreased, input, ref previous));
Console.WriteLine($"Answer Part 2: {countIncreased}");
Console.ReadKey();

static void Counter(int currentIndex, int numberOflookBackItems, ref int counter, List<int> inp, ref int previous)
{
    var canLookBackFarEnough = currentIndex >= numberOflookBackItems;
    var goBack= numberOflookBackItems;

    int totalCurrent = 0;

    if(canLookBackFarEnough)
    {
        while(goBack >0)
        {
            totalCurrent += inp[currentIndex - goBack];
            goBack--;
        }

        if (previous != 0 && totalCurrent > previous)
        {
            counter++;
        }

        previous = totalCurrent;
    }
}



