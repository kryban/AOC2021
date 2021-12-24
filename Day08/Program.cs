// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
var inputRaw = File.ReadAllLines(inputFile).ToList();

int DecodePart1(List<string> inputRaw)
{
    return inputRaw
    .Select(x => x.Split('|')[1]
    .Split(' ').Skip(1)
    .Count(y => y.Length == 2 || y.Length == 3 || y.Length == 4 || y.Length == 7)).ToList()
    .Sum();
}

int DecodePart2(List<string> inputRaw)
{
    int retval = 0;

    var importIn = inputRaw.Select(x => x.Split('|')[0])
        .Select(inputs => inputs.Trim().Split(' ').OrderBy(inputs => inputs.Length)).ToList();
    
    var importOutp = inputRaw.Select(x => x.Split('|')[1])
        .Select(inputs => inputs.Trim().Split(' ').ToList()).ToList();

    List<int> ints = new List<int>();
    int i = 0;

    foreach (var inp in importIn)
    {
        var mapper = new DigitGeneric();
        mapper.SetSegments(inp);
        ints.Add(mapper.Translate(importOutp[i].ToList()));
        i++;
    }

    return ints.Sum();
}

Console.WriteLine($"Day 08, part 1: {DecodePart1(inputRaw)}");
Console.WriteLine($"Day 08, part 2: {DecodePart2(inputRaw)}");
Console.ReadKey();

public class DigitGeneric
{
//      MUMU	0 = LU MU RU LD RD MD
//    LU    RU  1 = RU RD
//    LU    RU	2 = MU RI MM LD MD
//      MMMM	3 = MU RU MM RD MD
//    LD    RD	4 = LU MM RU RD
//    LD    RD	5 = MU LU MM RD MD
//      MDMD    6 = MU LU MM LD RD MD 
// 				7 = MU RU RD
// 				8 = LU MU RU MM LD MD RD
// 				9 = LU MU RU MM MD RD

    //LeftUp
    public char LU { get; set; }
    //MidUp
    public char MU { get; set; }
    //RightUp
    public char RU { get; set; }
    //MidMid
    public char MM { get; set; }
    //LefDown
    public char LD { get; set; }
    //MidDown
    public char MD { get; set; }
    //RightDown
    public char RD { get; set; }

    public string s0 {get;set;}
    public string s1 {get;set;}
    public string s2 {get;set;}
    public string s3 {get;set;}
    public string s4 {get;set;}
    public string s5 {get;set;}
    public string s6 {get;set;}
    public string s7 {get;set;}
    public string s8 {get;set;}
    public string s9 { get; set; }



    public Regex n0 => new Regex(ToPattern($"{s0}"));
    public Regex n1 => new Regex(ToPattern($"{s1}")); //2
    public Regex n2 => new Regex(ToPattern($"{s2}")); //5
    public Regex n3 => new Regex(ToPattern($"{s3}")); //5
    public Regex n4 => new Regex(ToPattern($"{s4}")); //4
    public Regex n5 => new Regex(ToPattern($"{s5}")); //5
    public Regex n6 => new Regex(ToPattern($"{s6}")); //6
    public Regex n7 => new Regex(ToPattern($"{s7}")); //3
    public Regex n8 => new Regex(ToPattern($"{s8}"));
    public Regex n9 => new Regex(ToPattern($"{s9}")); //6

    public void SetSegments(IEnumerable<string> inpSet)
    {
        s0 = "";
        s1 = "";
        s2 = "";
        s3 = "";
        s4 = "";
        s5 = "";
        s6 = "";
        s7 = "";
        s8 = "";
        s9 = "";

        foreach (var inp in inpSet)
        {
            if (inp.Length == 2)
            {
                s1 = inp;
            }
            else if (inp.Length == 3)
            {
                s7 = inp;
            }
            else if (inp.Length == 4)
            {
                s4 = inp;
            }
            else if (inp.Length == 7)
            {
                s8 = inp;
            }
            else if (inp.Length == 5)
            {
                //2, 3, 4 lang zetten LU, MU, RU, MM, RD
                bool itIs5 = Determine5(inp, s1, s7, s4);
                bool itIs3 = Determine3(inp, s7);
                bool itIs2 = !itIs5 && !itIs3;

                if (itIs2)
                    s2 = inp;
                if (itIs3)
                    s3 = inp;
                if(!itIs2 && !itIs3) 
                    s5 = inp;
            }
            else
            {
                bool itIs9 = Determine9(inp, s4);
                bool itIs0 = Determine0(inp, s4, s7);

                if (itIs9)  
                    s9 = inp;
                
                if(itIs0)
                    s0 = inp;

                if(!itIs0 && !itIs9)
                    s6 = inp;
            }
        }
    }

    private bool Determine9(string inp, string s4)
    {
        string tmp4 = s4;

        foreach (var c in inp)
        {
            // eerst alles van mogelijk 4 weghalen. 
            if (tmp4.Contains(c))
            {
                tmp4 = tmp4.Replace(c.ToString(), String.Empty);
            }
        }

        // Als alles van 4 weggehaald kunnen worden, terwijl het 6 lang is, dan is het een 9
        if (tmp4.Length == 0)
            return true;

        return false;
    }

    private bool Determine0(string inp, string s4, string s7)
    {
        string tmp4 = s4;
        string tmp7 = s7;

        foreach (var c in inp)
        {
            // eerst alles van mogelijk 4 weghalen. 
            if (tmp4.Contains(c))
            {
                tmp4 = tmp4.Replace(c.ToString(), String.Empty);
            }
        }

        foreach (var c in inp)
        {
            // eerst alles van mogelijk 4 weghalen. 
            if (tmp7.Contains(c))
            {
                tmp7 = tmp7.Replace(c.ToString(), String.Empty);
            }
        }

        // Als alles van 4 weggehaald kunnen worden, terwijl het 6 lang is, dan is het geen 0 maar een 9 of een 6
        if (tmp4.Length == 0)
            return false;

        // als moet alles van 4 maar wel alles van 7 overeenkomt, dan is het een 0
        if (tmp4.Length == 1 && tmp7.Length == 0)
            return true;

        // anders dus een 6
        return false;
    }

    private bool Determine5(string inp, string s1, string s7, string s4)
    {
        string tmp = inp;
        string tmp7 = s7;

        foreach (var c in inp)
        {
            // kijken of MU, RU en RD kunnen vervangen
            if (s7.Contains(c))
            {
                tmp7 = tmp7.Replace(c.ToString(), String.Empty);
                tmp = tmp.Replace(c.ToString(), String.Empty);
            }
        }

        // als de 5 letterig zowel een MU als een RU en RD heeft, dan kan het alleen een 3 zijn 
        if (tmp7.Length == 0)
        {
            return false;
        }

        // nu kan het ook nog een 5 zijn als uit 7 RU is overgebleven, dat weten we hier niet
        // als uit 7 de RD is overgebleven, dan wordt het niwet gedekt door inp en is het een 2
        if (tmp7.Length == 1)
        {
            var rest = tmp;
            // daarom eerst weghalen wat ook in 4 voorkomt, dat kan zijn LU en/of MM
            foreach(var c in tmp)
            {
                if (s4.Contains(c))
                    rest = rest.Replace(c.ToString(), String.Empty);
            }

            // wat er overblijft komt dus niet meer voor in 7 en 4
            tmp = rest;
        }

        // als er 1 resteert, dan kan dan alleen nog MD zijn en is het een 5,
        // anders is het 2 en dat zijn LD en MD en is het een 2
        return tmp.Length == 1;
    }

    private bool Determine3(string inp, string s7)
    {
        string tmp7 = s7;

        foreach (var c in inp)
        {
            // kijken of MU, RU en RD kunnen vervangen
            if (tmp7.Contains(c))
            {
                tmp7 = tmp7.Replace(c.ToString(), String.Empty);
            }
        }

        // als de 5 letterig zowel een MU als een RU en RD heeft, dan kan het alleen een 3 zijn 
        if (tmp7.Length == 0)
        {
            return true;
        }

        return false;
    }

    public int Translate(List<string> digits)
    {
        string retval = "";

        foreach (string digit in digits)
        {
            string t;
            string mess = "Not all signals defined";

            if (digit.Length == 2)
                t = n1.Match(digit).Success ? "1" : throw new ArgumentException(mess);
            else if (digit.Length == 3)
                t = n7.Match(digit).Success ? "7" : throw new ArgumentException(mess);
            else if (digit.Length == 4)
                t = n4.Match(digit).Success ? "4" : throw new ArgumentException(mess);
            else if (digit.Length == 5)
                t = n2.Match(digit).Success ? "2"
                        : n3.Match(digit).Success ? "3"
                        : n5.Match(digit).Success ? "5" : throw new ArgumentException(mess);
            else if (digit.Length == 6)
                t = n0.Match(digit).Success ? "0"
                        : n6.Match(digit).Success ? "6"
                        : n9.Match(digit).Success ? "9" : throw new ArgumentException(mess);
            else 
                t = n8.Match(digit).Success ? "8" : throw new ArgumentException(mess);

            retval += t;
        }

        return Convert.ToInt32(retval);
    }

    private string ToPattern(string inp)
    {
        return inp.Length == 0 
            ? $""
            : $"[{inp}]"+"{"+$"{inp.Length}"+"}";
    }
}
