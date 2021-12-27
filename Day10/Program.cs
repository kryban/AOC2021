// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/inputExample.txt";
List<string> inputRaw = File.ReadAllLines(inputFile).ToList();

List<char> RoundBracketOpen = new List<char>();
List<char> RoundBracketClose = new List<char>();
List<char> BraceOpen = new List<char>();
List<char> BraceClose = new List<char>();
List<char> SquareBracketOpen = new List<char>();
List<char> SquareBracketClose = new List<char>();
List<char> SmallerThen = new List<char>();
List<char> GreaterThen = new List<char>();

List<string> corruptLines = CalculateCorruptLines(inputRaw);

List<string> CalculateCorruptLines(List<string> inputRaw)
{
    List<string> retval = new List<string>();

    foreach (var line in inputRaw)
    {
        bool rowIsCorrupt = false;
        int index = 0;
        var tmp = line;

        foreach (char c in line)
        {
            if (c.Equals(')'))
            {
                tmp = tmp.Remove(index, 1).Insert(index,".");
                var substringBack = new string(tmp.Substring(0, index).Reverse().ToArray());
                var t = substringBack;
                int i = 0;
                while(i < substringBack.Length)
                {
                    if (substringBack[i] != '.' && substringBack[i] != '(')
                    {
                        rowIsCorrupt = true;
                        Console.WriteLine($"Corrupt at {substringBack[i]}");
                    }
                    else if (substringBack[i] != '.' && substringBack[i] == '(')
                    {
                        t = t.Remove(i, 1).Insert(i, ".");
                        break;
                    }
                    i++;
                }

                substringBack = t;

                ////substringBack = substringBack.Replace(".",String.Empty);

                //if (!substringBack.Last().Equals('('))
                //{
                //    rowIsCorrupt = true;
                //    Console.WriteLine("Corrupt");
                //}

                var openerIndex = substringBack.Length - 1 - i;
                tmp = tmp.Remove(openerIndex, 1).Insert(openerIndex, ".");
            }
            if (c.Equals('}'))
            {
                tmp = tmp.Remove(index, 1).Insert(index, ".");
                var substringBack = new string(tmp.Substring(0, index).Reverse().ToArray());
                var t = substringBack;
                int i = 0;
                while (i < substringBack.Length)
                {
                    if (substringBack[i] != '.' && substringBack[i] != '{')
                    {
                        rowIsCorrupt = true;
                        Console.WriteLine($"Corrupt at {substringBack[i]}");
                    }
                    else if (substringBack[i] != '.' && substringBack[i] == '{')
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
            if (c.Equals(']'))
            {
                tmp = tmp.Remove(index, 1).Insert(index, ".");
                var substringBack = new string(tmp.Substring(0, index).Reverse().ToArray());
                var t = substringBack;
                int i = 0;
                while (i < substringBack.Length)
                {
                    if (substringBack[i] != '.' && substringBack[i] != '[')
                    {
                        rowIsCorrupt = true;
                        Console.WriteLine($"Corrupt at {substringBack[i]}");
                    }
                    else if (substringBack[i] != '.' && substringBack[i] == '[')
                    {
                        t = t.Remove(i, 1).Insert(i, ".");
                        break;
                    }
                    i++;
                }

                substringBack = t;

                ////substringBack = substringBack.Replace(".",String.Empty);

                //if (!substringBack.Last().Equals('('))
                //{
                //    rowIsCorrupt = true;
                //    Console.WriteLine("Corrupt");
                //}

                var openerIndex = substringBack.Length - 1 - i;
                tmp = tmp.Remove(openerIndex, 1).Insert(openerIndex, ".");
            }
            if (c.Equals('>'))
            {
                tmp = tmp.Remove(index, 1).Insert(index, ".");
                var substringBack = new string(tmp.Substring(0, index).Reverse().ToArray());
                var t = substringBack;
                int i = 0;
                while (i < substringBack.Length)
                {
                    if (substringBack[i] != '.' && substringBack[i] != '<')
                    {
                        rowIsCorrupt = true;
                        Console.WriteLine($"Corrupt at {substringBack[i]}");
                    }
                    else if (substringBack[i] != '.' && substringBack[i] == '<')
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


            //if (c.Equals('('))
            //    RoundBracketOpen.Add(c);
            //if (c.Equals('{'))
            //    BraceOpen.Add(c);
            //if (c.Equals('['))
            //    SquareBracketOpen.Add(c);
            //if (c.Equals('<'))
            //    SmallerThen.Add(c);
            //if (c.Equals(')'))
            //{
            //    rowIsCorrupt = RoundBracketClose.AddCloser(c).AndValidateAgainst(RoundBracketOpen);
            //}
            //if (c.Equals('}'))
            //{
            //    rowIsCorrupt = BraceClose.AddCloser(c).AndValidateAgainst(BraceOpen);
            //}
            //if (c.Equals(']'))
            //{
            //    rowIsCorrupt = SquareBracketClose.AddCloser(c).AndValidateAgainst(SquareBracketOpen);
            //}
            //if (c.Equals('>'))
            //{
            //    rowIsCorrupt = SmallerThen.AddCloser(c).AndValidateAgainst(GreaterThen);
            //}

            if (rowIsCorrupt)
            {
                retval.Add(line);
                break;
            }

            index++;
        }
    }

    return retval;
}

public static class Ext
{
    public static List<char> AddCloser(this List<char> closingList, char c)
    {
        closingList.Add(c);
        return closingList;
    }

    public static bool AndValidateAgainst(this List<char> closeList, List<char> openList)
    {
        return closeList.Count > openList.Count;
    }
}