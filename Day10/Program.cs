// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
List<string> inputRaw = File.ReadAllLines(inputFile).ToList();
List<string> incompleteLines = new List<string>();
List<string> incompleteLinesCleaned = new List<string>();

Console.WriteLine($"Day 10, Part 1: Corrupt score is (370407) {CalculateCorruptScore(inputRaw)}");
Console.WriteLine($"Day 10, Part 2: Corrupt score is (3249889609) {CalculateAutoCompleteScore(inputRaw)}");

long CalculateAutoCompleteScore(List<string> inputRaw)
{
    incompleteLines = new List<string>();
    incompleteLinesCleaned = new List<string>();
    List<long> autoCompleteScores = new List<long>();
    int retval = 0;
    CalculateCorruptScore(inputRaw);

    foreach(var line in incompleteLinesCleaned)
    {
        var openingChars = line.Replace(".", string.Empty);
        long subtotal = 0;

        foreach (var currentOpeningChar in openingChars.Reverse())
        {
            subtotal = AutoCompleteScore(subtotal, CorrespondingCloseChar(currentOpeningChar));
        }
        autoCompleteScores.Add(subtotal);
    }

    return autoCompleteScores.OrderBy(x => x).ToList()[autoCompleteScores.Count/2];
}

long AutoCompleteScore(long subtotal, char c)
{
    // ): 1 points.
    // ]: 2 points.
    // }: 3 points.
    // >: 4 points.
    var retval = subtotal * 5;

    return c.Equals(')') ? retval + 1
    : c.Equals(']') ? retval + 2
    : c.Equals('}') ? retval + 3 : retval + 4;
}

int CalculateCorruptScore(List<string> inputRaw)
{
    int retval = 0;

    foreach (var line in inputRaw)
    {
        bool rowIsCorrupt = false;
        int index = 0;
        var tmp = line;

        foreach (char current in line)
        {
            if (current.Equals(')'))
            {
                ValidateClosingChar('(',current,ref tmp, index, ref rowIsCorrupt);
            }
            if (current.Equals('}'))
            {
                ValidateClosingChar('{', current, ref tmp, index, ref rowIsCorrupt);
            }
            if (current.Equals(']'))
            {
                ValidateClosingChar('[', current, ref tmp, index, ref rowIsCorrupt);
            }
            if (current.Equals('>'))
            {
                ValidateClosingChar('<', current, ref tmp, index, ref rowIsCorrupt);
            }

            if (rowIsCorrupt)
            {
                retval += CharScore(current);
                break;
            }

            index++;
        }

        if (!rowIsCorrupt)
        {
            incompleteLines.Add(line);
            incompleteLinesCleaned.Add(tmp);
        }
    }

    return retval;
}

void ValidateClosingChar(char expectedOpener, char current, ref string tmp, int index, ref bool rowIsCorrupt)
{
    tmp = tmp.Remove(index, 1).Insert(index, ".");
    var substringBack = new string(tmp.Substring(0, index).Reverse().ToArray());
    var t = substringBack;
    int i = 0;
    while (i < substringBack.Length)
    {
        if (substringBack[i] != '.' && substringBack[i] != expectedOpener)
        {
            rowIsCorrupt = true;
            Console.WriteLine($"Corrupt at {substringBack[i]}. Expected '{CorrespondingCloseChar(substringBack[i])}', but found {current}.");
            return;
        }
        else if (substringBack[i] != '.' && substringBack[i] == expectedOpener)
        {
            t = t.Remove(i, 1).Insert(i, ".");
            break;
        }
        i++;
    }

    substringBack = t;

    var openerIndex = substringBack.Length - 1 - i;
    tmp = tmp.Remove(openerIndex, 1).Insert(openerIndex, ".");
}

int CharScore(char c)
{
    // ): 3 points.
    // ]: 57 points.
    // }: 1197 points.
    // >: 25137 points.
    return c.Equals(')') ? 3
    : c.Equals(']') ? 57
    : c.Equals('}') ? 1197 : 25137;
}

char CorrespondingCloseChar(char openingChar)
{
    return openingChar.Equals('{') ? '}'
        : openingChar.Equals('(') ? ')'
        : openingChar.Equals('[') ? ']' : '>';
}
