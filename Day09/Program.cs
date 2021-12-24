// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
var inputRaw = File.ReadAllLines(inputFile).ToList();
List<int> lowestPoints = new List<int>();

for (int i = 0; i < inputRaw.Count(); i++)
{
    var currentRowPosition = i;

    var previousRow = currentRowPosition > 0 ? inputRaw[currentRowPosition - 1] : String.Empty;
    var currentRow = inputRaw[currentRowPosition];
    var nextRow = currentRowPosition <= inputRaw.Count - 2 ? inputRaw[currentRowPosition + 1] : String.Empty;

    for (int ii = 0; ii < currentRow.Length; ii++)
    {
        int currentPosition = ii;

        var above = !String.IsNullOrEmpty(previousRow) ? previousRow[currentPosition].ToString() : String.Empty;
        var left = currentPosition > 0 ? currentRow[currentPosition-1].ToString() : String.Empty;
        var current = currentRow[currentPosition].ToString();
        var right = currentPosition + 1 <= currentRow.Length -1 ? currentRow[currentPosition+1].ToString() : String.Empty;
        var below = !String.IsNullOrEmpty(nextRow) ? nextRow[currentPosition].ToString() : String.Empty;

        bool lowest = current.IsSmallerThen(above) && current.IsSmallerThen(left) && current.IsSmallerThen(right) && current.IsSmallerThen(below);

        if (lowest) 
            lowestPoints.Add(Convert.ToInt32(current));
    }
}

lowestPoints.ForEach(p => Console.Write(p));
Console.WriteLine();
Console.WriteLine($"Day 9, part 1: {lowestPoints.Select(x => x+1).ToList().Sum()}");
Console.ReadKey();


public static class Ext
{
    public static bool IsSmallerThen(this string current, string neighbour)
    {
        bool retval;
        retval = String.IsNullOrEmpty(neighbour) || (Convert.ToInt32(current) < Convert.ToInt32(neighbour));
        return retval;
    }
}