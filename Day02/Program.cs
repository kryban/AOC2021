// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;

int totalDepth = 0;
int totalDistance = 0;
int totalAim = 0;
int countedRows = 0;

string inputFile = @"./ProjectItems/input.txt";
File.ReadAllLines(inputFile).ToList().ForEach(x => MeasureAction(1,x, ref totalDepth, ref totalDistance, ref totalAim, ref countedRows));
Console.WriteLine($"Part1: Counted rows: {countedRows} - Distance {totalDistance} - Depth {totalDepth}");
Console.WriteLine($"Part1: Answer: {totalDistance * totalDepth}");

totalDepth = 0;
totalDistance = 0;
totalAim = 0;
countedRows = 0;
File.ReadAllLines(inputFile).ToList().ForEach(x => MeasureAction(2, x, ref totalDepth, ref totalDistance, ref totalAim, ref countedRows));
Console.WriteLine($"Part2: Counted rows: {countedRows} - Distance {totalDistance} - Depth {totalDepth}");
Console.WriteLine($"Part2: Answer: {totalDistance * totalDepth}");


static void MeasureAction(int part, string rawAction, ref int totalDepth, ref int totalDistance, ref int totalAim, ref int countedRows)
{
    var row = rawAction.ToString().Split(' ');

    string action = row[0];
    int amount = Convert.ToInt32(row[1]);

    if(action.Equals("down"))
    {
        if(part == 1)        
            totalDepth += amount;
        else
            totalAim += amount;
    }
    else if(action.Equals("up"))
    {
        if (part == 1)
            totalDepth -= amount;
        else
            totalAim -= amount;
    }
    else
    {
        if (part == 1)
            totalDistance += amount;
        else
        {
            totalDistance += amount;
            totalDepth += totalAim * amount;
        }
    } 

    countedRows++;
}

Console.Read();