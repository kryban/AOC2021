// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
List<string> inputRaw = File.ReadAllLines(inputFile).ToList();

Console.WriteLine($"Day 10, Part 1: Corrupt score is (370407) {CalculateCorruptScore(inputRaw)}");

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
                retval += ValidateClosingChar('(',current,ref tmp, index, ref rowIsCorrupt);
            }
            if (current.Equals('}'))
            {
                retval += ValidateClosingChar('{', current, ref tmp, index, ref rowIsCorrupt);
            }
            if (current.Equals(']'))
            {
                retval += ValidateClosingChar('[', current, ref tmp, index, ref rowIsCorrupt);
            }
            if (current.Equals('>'))
            {
                retval += ValidateClosingChar('<', current, ref tmp, index, ref rowIsCorrupt);
            }

            if (rowIsCorrupt)
            {
                break;
            }

            index++;
        }
    }

    return retval;
}

int ValidateClosingChar(char expectedOpener, char current, ref string tmp, int index, ref bool rowIsCorrupt)
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
            return CorruptCharScore(current);
            break;
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

    return 0;
}

int CorruptCharScore(char c)
{
    // ): 3 points.
    // ]: 57 points.
    // }: 1197 points.
    // >: 25137 points.
    return c.Equals(')') ? 3
    : c.Equals(']') ? 57
    : c.Equals('}') ? 1197 : 25137;
}

char CorrespondingCloseChar(char v)
{
    return v.Equals('{') ? '}'
        : v.Equals('(') ? ')'
        : v.Equals('[') ? ']' : '>';
}
