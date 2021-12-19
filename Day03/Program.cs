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

CalculateAnswerPart2(inputRaw, elementLength);

static void CalculateAnswerPart1(List<int> gamma, List<int> epsilon)
{
    var gammaInt = Convert.ToInt32(string.Concat(gamma.Select(x => x.ToString())), 2);
    var epsilonInt = Convert.ToInt32(string.Concat(epsilon.Select(x => x.ToString())), 2);

    Console.WriteLine($"Part 1: Answer {gammaInt*epsilonInt}");
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

    if (inp.Count > 1)
        return relevantNumbers.Where(x => x == 1).Count() >= (relevantNumbers.Count() / 2) ? 1 : 0; // if 1''s and 0s are equal, then 1 is dominant
    else
        return int.Parse(inp.First()[pos].ToString());
}

static int CountRecessive(List<string> inp, int pos)
{
    List<int> relevantNumbers = inp.Select(row => int.Parse(row[pos].ToString())).ToList();

    if (inp.Count > 1)
        return relevantNumbers.Where(x => x == 0).Count() <= (relevantNumbers.Count() / 2) ? 0 : 1; // if 1''s and 0s are equal, then 0 is recessive
    else
        return int.Parse(inp.First()[pos].ToString());
}

static List<string> FilterElementsWithCounted(List<string> inp, int dominant, int relevantPosition)
{
    return inp.Where(x => x[relevantPosition] == Convert.ToChar(dominant.ToString())).ToList();
}

static void CalculateAnswerPart2(List<string> inp, int length)
{
    List<string> listOxygen = inp;
    List<string> listCo2 = inp;

    for (int i = 0; i < length; i++)
    {
        listOxygen = FilterElementsWithCounted(listOxygen, CountDominant(listOxygen, i), i);
        listCo2 = FilterElementsWithCounted(listCo2, CountRecessive(listCo2, i), i);
    }

    var oxygen = Convert.ToInt32(listOxygen.First(), 2);
    var co2 = Convert.ToInt32(listCo2.First(), 2);

    Console.WriteLine($"Part 2: Answer {oxygen*co2}");
    Console.ReadKey();

}
