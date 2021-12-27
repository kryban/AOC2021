// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
List<string> inputRaw = File.ReadAllLines(inputFile).ToList();
List<PartPoint> lowestPoints = new List<PartPoint>();

int CalculatePart1(List<string> inputRaw)
{
    lowestPoints = CalculateLowest(inputRaw);
    return CalculateLowest(inputRaw).Select(x => Convert.ToInt32(x.CurrentValue) + 1).ToList().Sum();
}

int CalculatePart2()
{
    int retval = 0;
    List<int> relevantNeighbours = new List<int>();

    List<PartPoint> lowestPoints = CalculateLowest(inputRaw);
    List<(PartPoint bassin, int size)> bassinSizes = new List<(PartPoint bassin, int size)>();

    foreach (var lowestPoint in lowestPoints)
    {
        List<PartPoint> unhandledPoints = new List<PartPoint>();
        List<PartPoint> handledPoints = new List<PartPoint>();

        handledPoints.Add(lowestPoint);

        AddNeighboursToUnhandled(handledPoints, unhandledPoints, lowestPoint);

        while (unhandledPoints.Count > 0)
        {
            var pointInProcess = unhandledPoints.First();
            AddNeighboursToUnhandled(handledPoints, unhandledPoints, pointInProcess);
            handledPoints.Add(pointInProcess);
            unhandledPoints.Remove(pointInProcess);
        }

        bassinSizes.Add((lowestPoint, handledPoints.Count));
    }

    var result = bassinSizes.OrderByDescending(x => x.size).ToList().Take(3).ToList();
    retval = result[0].size * result[1].size * result[2].size;

    return retval;
}

CalculateLowest(inputRaw).ForEach(p => Console.Write(p.CurrentValue));
Console.WriteLine();
Console.WriteLine($"Day 9, part 1: {CalculatePart1(inputRaw)}");
Console.WriteLine($"Day 9, part 2: {CalculatePart2()}");
Console.ReadKey();

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

void AddNeighboursToUnhandled(List<PartPoint> handledPoints, List<PartPoint> unhandledPoints, PartPoint partPoint)
{
    if (partPoint.Above.IsRelevant(handledPoints, unhandledPoints))
        unhandledPoints.Add(partPoint.Above);
    if (partPoint.Left.IsRelevant(handledPoints, unhandledPoints))
        unhandledPoints.Add(partPoint.Left);
    if (partPoint.Right.IsRelevant(handledPoints, unhandledPoints))
        unhandledPoints.Add(partPoint.Right);
    if (partPoint.Below.IsRelevant(handledPoints, unhandledPoints))
        unhandledPoints.Add(partPoint.Below);
}

public class PartPoint
{
    private List<string> inputRaw = new List<string>();
    public int currentRowPosition;
    public int currentPosition;

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
    public bool IsPartOfBassin => Convert.ToInt32(CurrentValue) < 9;
}

public static class Ext
{
    public static bool IsSmallerThen(this string current, string? neighbour)
    {
        return String.IsNullOrEmpty(neighbour) || (Convert.ToInt32(current) < Convert.ToInt32(neighbour));
    }

    public static bool IsRelevant(this PartPoint currentPoint, List<PartPoint> handledPoints, List<PartPoint> unhandledPoints)
    {
        return currentPoint != null
            && currentPoint.IsPartOfBassin
            && !currentPoint.IsAlreadyHandled(handledPoints)
            && !unhandledPoints.Any(x => x.currentRowPosition == currentPoint.currentRowPosition && x.currentPosition == currentPoint.currentPosition);
    }

    public static bool IsAlreadyHandled(this PartPoint currentPoint, List<PartPoint> handledPoints)
    {
        return handledPoints.Any(p => p.currentPosition == currentPoint.currentPosition && p.currentRowPosition == currentPoint.currentRowPosition);
    }
}