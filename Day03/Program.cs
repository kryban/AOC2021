// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
var inputRaw = File.ReadAllLines(inputFile).ToList();
var elementLength = inputRaw.First().Length;
List<int> gamma = new List<int>();
List<int> epsilon = new List<int>();


SetGamma(inputRaw, elementLength, gamma);
SetEpsilon(gamma, epsilon);
CalculateAnswerPart1(gamma, epsilon);

static void CalculateAnswerPart1(List<int> gamma, List<int> epsilon)
{
    var gammaInt = Convert.ToInt32(string.Concat(gamma.Select(x => x.ToString())), 2);
    var epsilonInt = Convert.ToInt32(string.Concat(epsilon.Select(x => x.ToString())), 2);

    Console.WriteLine($"Part 1: Answer {gammaInt*epsilonInt}");
    Console.ReadKey();
}

static void SetEpsilon(List<int> gamma, List<int> epsilon)
{
    foreach (var item in gamma)
    {
        if (item == 1)
            epsilon.Add(0);
        else
            epsilon.Add(1);
    }
}

static void SetGamma(List<string> inp, int length, List<int> gamma)
{
    for (int i = 0; i < length; i++)
    {
        gamma.Add(CountDominant(inp, i));
    }
}

static int CountDominant(List<string> inp, int pos)
{
    List<int> relevantNumbers = inp.Select(row => int.Parse(row[pos].ToString())).ToList();

    int foo; 
    foo = relevantNumbers.Where(x => x == 0).Count() > (relevantNumbers.Count() / 2) ? 0 : 1;
    return foo;
}

//static void FilterElementsWithDominant(List<char> inp, char odm )
