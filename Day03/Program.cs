// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
var inputRaw = File.ReadAllLines(inputFile).ToList();
var elementLength = inputRaw.First().Length;
List<char> gamma = new List<char>();
List<char> epsilon = new List<char>();


SetGamma(inputRaw, elementLength, gamma);
SetEpsilon(gamma, epsilon);
CalculateAnswer(gamma, epsilon);

static void CalculateAnswer(List<char> gamma, List<char> epsilon)
{
    var gammaInt = Convert.ToInt32(new string(gamma.ToArray()),2);
    var epsilonInt = Convert.ToInt32(new string(epsilon.ToArray()),2);

    Console.WriteLine($"Part 1: Answer {gammaInt*epsilonInt}");
    Console.ReadKey();
}

static void SetEpsilon(List<char> gamma, List<char> epsilon)
{
    foreach (var item in gamma)
    {
        if (item == '1')
            epsilon.Add('0');
        else
            epsilon.Add('1');
    }
}

static void SetGamma(List<string> inp, int length, List<char> gamma)
{
    for (int i = 0; i < length; i++)
    {
        gamma.Add(CountDominant(inp, i));
    }
}

static char CountDominant(List<string> inp, int pos)
{
    List<char> relevantNumbers = inp.Select(row => row[pos]).ToList();

    return relevantNumbers.Where(x => x == '0').Count() > relevantNumbers.Count() / 2 ? '0' : '1';
}
