// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;

int totalDepth = 0;
int totalDistance = 0;
int countedRows = 0;

string inputFile = @"./ProjectItems/input.txt";
File.ReadAllLines(inputFile).ToList().ForEach(x => MeasureAction(x, ref totalDepth, ref totalDistance, ref countedRows));

static void MeasureAction(string rawAction, ref int totalDepth, ref int totalDistance, ref int countedRows)
{
    var row = rawAction.ToString().Split(' ');

    string action = row[0];
    int amount = Convert.ToInt32(row[1]);

    if(action.Equals("down"))
    {
        totalDepth += amount;
    }
    else if(action.Equals("up"))
    {
        totalDepth -= amount;
    }
    else
    {
        totalDistance += amount;
    } 

    countedRows++;
}

Console.WriteLine($"Counted rows: {countedRows} - Distance {totalDistance} - Depth {totalDepth}");
Console.WriteLine($"Answer part 1: {totalDistance*totalDepth}");
Console.Read();