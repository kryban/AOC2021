// See https://aka.ms/new-console-template for more information
using System.Runtime.Serialization;
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
        .Select(inputs => inputs.Trim().Split(' ').OrderBy(inputs => inputs.Length)).ToList();//.OrderBy(yy => yy.Length));
    
    var importOutp = inputRaw.Select(x => x.Split('|')[1])
        .Select(inputs => inputs.Trim().Split(' ').OrderBy(inputs => inputs.Length)).ToList();

    List<int> ints = new List<int>();
    int i = 0;

    foreach (var inp in importIn)
    {
        var mapper = new DigitGeneric();
        mapper.SetSegments(inp);
        ints.Add(mapper.Translate(importOutp[i].ToList()));
        i++;
    }

    //var importOutput = inputRaw.Select(x => x.Split('|')[1]).ToList().Select(y => y.Trim().Split(' ')).ToList();

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

    public Regex n0 => new Regex(ToPattern($"{LU}{MU}{RU}{LD}{RD}{MD}"));//6
    public Regex n1 => new Regex(ToPattern($"{RU}{RD}")); //2
    public Regex n2 => new Regex(ToPattern($"{MU}{RU}{MM}{LD}{MD}"));//5
    public Regex n3 => new Regex(ToPattern($"{MU}{RU}{MM}{RD}{MD}")); //5
    public Regex n4 => new Regex(ToPattern($"{LU}{MM}{RU}{RD}")); //4
    public Regex n5 => new Regex(ToPattern($"{MU}{LU}{MM}{RD}{MD}")); //5
    public Regex n6 => new Regex(ToPattern($"{MU}{LU}{MM}{LD}{RD}{MD}")); //6
    public Regex n7 => new Regex(ToPattern($"{MU}{RU}{RD}")); //3
    public Regex n8 => new Regex(ToPattern($"{LU}{MU}{RU}{MM}{LD}{MD}{RD}"));
    public Regex n9 => new Regex(ToPattern($"{LU}{MU}{RU}{MM}{MD}{RD}")); //6

    public void SetSegments(IEnumerable<string> inpSet)
    {
        string s0 = "";
        string s1 = "";
        string s2 = "";
        string s3 = "";
        string s4 = "";
        string s5 = "";
        string s6 = "";
        string s7 = "";
        string s8 = "";
        string s9 = "";

        foreach (var inp in inpSet)
        {
            if (inp.Length == 2)
            {
                //2 lang is een 1 en zet RU, en RD
                //1 = RU RD
                RU = inp[0];
                RD = inp[1];
            }
            if (inp.Length == 3)
            {
                //3 lang zet MU, RU en RD
                //7 = MU RU RD
                var tmp = inp;
                tmp = tmp.Replace(RU.ToString(),String.Empty);
                tmp = tmp.Replace(RD.ToString(),String.Empty);
                MU = tmp[0];
            }
            if (inp.Length == 4)
            {
                //4 lang zet LU, MM, RU en RD
                //4 = LU MM RU RD
                var tmp = inp;
                tmp = tmp.Replace(RU.ToString(), String.Empty);
                tmp = tmp.Replace(RD.ToString(), String.Empty);
                LU = tmp[0];
                MM = tmp[1];
            }
            if (inp.Length == 5)
            {
                //2, 3, 4 lang zetten LU, MU, RU, MM, RD
                var tmp = inp;

//                tmp = tmp.Replace(LU.ToString(), String.Empty);
//                tmp = tmp.Replace(MU.ToString(), String.Empty);
//                tmp = tmp.Replace(RU.ToString(), String.Empty);
//                tmp = tmp.Replace(MM.ToString(), String.Empty);
//                tmp = tmp.Replace(RD.ToString(), String.Empty);

                //5 lang kan zijn 2, 3, of 5
                /////onzin//2 kan het alleen zijn als MU, RU, MM in voorkomen maar RD NIET
                ///
                //2 kan het alleen zijn als 2 letter in 7 voorkomen, 1 letter in 4 en de overgebleven niet in 1 of 7
                if (inp.Contains(MU) && inp.Contains(RU) && inp.Contains(MM) && !inp.Contains(RD))
                {
                    LD = tmp[0].Equals(MD) ? tmp[1] : tmp[0];
                    MD = tmp[1].Equals(LD) ? tmp[0] : tmp[1];
                }

                //3 kan het alleen zijn als MU, RU, MM in voorkomen maar LD NIET
                if (inp.Contains(MU) && inp.Contains(RU) && inp.Contains(MM) && !inp.Contains(LD))
                    MD = tmp[0];

                //5 kan het alleen zijn als MU, LU, MM, RD in voorkomen 
                if (inp.Contains(MU) && inp.Contains(LU) && inp.Contains(MM) && inp.Contains(RD))
                    MD = tmp[0];
            }
            if (inp.Length == 6)
            {
                //2, 3, 4 lang zetten LU, MU, RU, MM, RD
                var tmp = inp;

                tmp = tmp.Replace(LU.ToString(), String.Empty);
                tmp = tmp.Replace(MU.ToString(), String.Empty);
                tmp = tmp.Replace(RU.ToString(), String.Empty);
                tmp = tmp.Replace(MM.ToString(), String.Empty);
                tmp = tmp.Replace(RD.ToString(), String.Empty);

                //6 lang kan zijn 0, 6 of 9

                //0 kan het alleen zijn als het 6 lang is en GEEN MM heeft
                if (!inp.Contains(MM))
                    Console.WriteLine("0onbekendeActie");

                //6 kan het alleen zijn als het 6 lang is en GEEN RU heeft
                if (!inp.Contains(RU))
                    Console.WriteLine("6onbekendeActie");

                //9 kan het alleen zijn als MU, LU, MM, RU en RD in voorkomen
                if (inp.Contains(MU) && inp.Contains(LU) && inp.Contains(MM) && inp.Contains(RU) && inp.Contains(RD))
                    Console.WriteLine("9onbekendeActie");
            }
            if (inp.Length == 7)
            {
                var tmp = inp;
                //8 = LU MU RU MM LD MD RD
            }
        }
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
        return $"[{inp}]"+"{"+$"{inp.Length}"+"}";
    }
}
