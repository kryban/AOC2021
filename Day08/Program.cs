// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
var inputRaw = File.ReadAllLines(inputFile).ToList();

var numberOfUniquePatterns = inputRaw
    .Select(x => x.Split('|')[1]
    .Split(' ').Skip(1)
    .Count(y => y.Length == 2 || y.Length == 3 || y.Length == 4 || y.Length == 7)).ToList()
    .Sum();

Console.WriteLine($"Day 08, part 1: {numberOfUniquePatterns}");
Console.ReadKey();

