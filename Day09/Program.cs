// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
List<string> inputRaw = File.ReadAllLines(inputFile).ToList();
List<PartPoint> lowestPoints = new List<PartPoint>();


int Part2()
{
    int retval = 0;
    List<int> relevantNeighbours = new List<int>();

    //1 calculate lowest 
    List<PartPoint> lowestPoints = CalculateLowest(inputRaw);
    List<int> bassinSizes = new List<int>();

    //1 get first lowest 
    foreach (var lowestPoint in lowestPoints)
    {
        List<PartPoint> unhandledPoints = new List<PartPoint>();

        if(lowestPoint.PartOfBassin)
        {
            unhandledPoints.Add(lowestPoint);
        }



        for (int i = 0; i < inputRaw.Count(); i++)
        {
            var currentRowPosition = i;

            for (int ii = 0; ii < inputRaw[currentRowPosition].Length; ii++)
            {
                int currentPosition = ii;

                var p = new PartPoint(inputRaw, currentRowPosition, currentPosition);

                if (p.IsSmallest)
                    unhandledPoints.Add(p);
            }
        }
        //2 calculate neighbours
        //  and add to unhandled neighbours
    }


    //3 get first unhandled neighbour
    //calculate neighbours
    //add neighbour to unhandled neighbours if it is greater then current but not lowest of 9
    //remove current from unhnadled and add to handled 

    //4 repeat 3

    //4 if there is no more unhandled neighobour,
    //count all numbers in bassin
    //write result to bassin size

    //repeat step 1

    // if there is no lowest
    //select top 3 bassins
    //multiply size of the 3 bassins
    var result = bassinSizes.OrderByDescending(x => x).ToList().Take(3).ToList();
    retval = result[0] * result[1] * result[3];

    return retval;
}

CalculateLowest(inputRaw).ForEach(p => Console.Write(p.CurrentValue));
Console.WriteLine();
Console.WriteLine($"Day 9, part 1: {CalculatePart1(inputRaw)}");
Console.ReadKey();

int CalculatePart1(List<string> inputRaw)
{
    lowestPoints = CalculateLowest(inputRaw);
    return CalculateLowest(inputRaw).Select(x => Convert.ToInt32(x.CurrentValue) + 1).ToList().Sum();
}

List<PartPoint> CalculateLowest(List<string> inputRaw)
{
    var retval = new List<PartPoint>();
    for (int i = 0; i < inputRaw.Count(); i++)
    {
        var currentRowPosition = i;

        for (int ii = 0; ii < inputRaw[currentRowPosition].Length; ii++)
        {
            int currentPosition = ii;

            var p = new PartPoint(inputRaw, currentRowPosition, currentPosition);

            if (p.IsSmallest)
                retval.Add(p);
        }
    }
    return retval;
}

public class PartPoint
{
    private List<string> inputRaw = new List<string>();
    private int currentRowPosition;
    private int currentPosition;

    public PartPoint(List<string> inputRaw, int currentRowPosition, int currentPosition)
    {
        this.inputRaw = inputRaw;
        this.currentRowPosition = currentRowPosition;
        this.currentPosition = currentPosition;
    }

    private string previousRow => currentRowPosition > 0 ? inputRaw[currentRowPosition - 1] : String.Empty;
    private string currentRow => inputRaw[currentRowPosition];
    private string nextRow => currentRowPosition <= inputRaw.Count - 2 ? inputRaw[currentRowPosition + 1] : String.Empty;

    public PartPoint? Above => !String.IsNullOrEmpty(previousRow) ? new PartPoint(inputRaw, currentRowPosition - 1, currentPosition) : null;

    public PartPoint? Left => currentPosition > 0 ? new PartPoint(inputRaw, currentRowPosition, currentPosition - 1) : null;
    public PartPoint? Right => currentPosition + 1 <= currentRow.Length - 1 ? new PartPoint(inputRaw, currentRowPosition, currentPosition+1) : null;
    public PartPoint? Below => !String.IsNullOrEmpty(nextRow) ? new PartPoint(inputRaw, currentRowPosition + 1, currentPosition) : null;

    public string CurrentValue => currentRow[currentPosition].ToString();

    public bool IsSmallest => CurrentValue.IsSmallerThen(Above?.CurrentValue) && CurrentValue.IsSmallerThen(Left?.CurrentValue)
        && CurrentValue.IsSmallerThen(Right?.CurrentValue) && CurrentValue.IsSmallerThen(Below?.CurrentValue);
    public bool PartOfBassin => Convert.ToInt32(CurrentValue) < 9;

    private PartPoint? GetPointIfRelevant(List<string> inputRaw, int neighbourPosition, int currentPosition, string currentValue)
    {
        var neighbour = new PartPoint(inputRaw, neighbourPosition, currentPosition);
        var neighbourValue = Convert.ToInt32(neighbour.CurrentValue);

        if (neighbourValue > Convert.ToInt32(currentValue) && neighbourValue < 9)
            return neighbour;

        return null;
    }

}

public static class Ext
{
    public static bool IsSmallerThen(this string current, string? neighbour)
    {
        bool retval;
        retval = String.IsNullOrEmpty(neighbour) || (Convert.ToInt32(current) < Convert.ToInt32(neighbour));
        return retval;
    }
}