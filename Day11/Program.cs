// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
List<string> inputRaw = File.ReadAllLines(inputFile).ToList();

List<Octopus> octoPussies = ConvertToOctopussies(inputRaw);

Console.WriteLine($"Day 11, part 1: ");

List<Octopus> ConvertToOctopussies(List<string> inputRaw)
{
    List<Octopus> retval = new List<Octopus>();
    var rowPosition = 0;
    foreach (var line in inputRaw)
    {
        var currenPosition = 0;
        foreach(var item in line)
        {
            var o = new Octopus(inputRaw, rowPosition, currenPosition);
            retval.Add(o);

            currenPosition++;
        }
        rowPosition++;
    }

    return retval;
}

public class Octopus
{
    private List<string> inputRaw = new List<string>();

    private string previousRow;
    private string currentRow;
    private string nextRow;

    private Octopus? leftUp;
    private Octopus? middleUp ;
    private Octopus? rightUp;
    private Octopus? left;
    private Octopus? right;
    private Octopus? leftDown;
    private Octopus? middleDown;
    private Octopus? rightDown;

    public int currentRowPosition;
    public int currentPosition;

    public Octopus(List<string> inputRaw, int currentRowPosition, int currentPosition)
    {
        this.inputRaw = inputRaw;
        this.currentRowPosition = currentRowPosition;
        this.currentPosition = currentPosition;
        
        previousRow = currentRowPosition > 0 ? inputRaw[currentRowPosition - 1] : String.Empty;
        currentRow = inputRaw[currentRowPosition];
        nextRow = currentRowPosition <= inputRaw.Count - 2 ? inputRaw[currentRowPosition + 1] : String.Empty;

        Octopus? middleUp = !String.IsNullOrEmpty(previousRow) ? new Octopus(inputRaw, currentRowPosition - 1, currentPosition) : null;
        Octopus? left = currentPosition > 0 ? new Octopus(inputRaw, currentRowPosition, currentPosition - 1) : null;
        Octopus? right = currentPosition + 1 <= currentRow.Length - 1 ? new Octopus(inputRaw, currentRowPosition, currentPosition + 1) : null; 
        Octopus? middleDown = !String.IsNullOrEmpty(nextRow) ? new Octopus(inputRaw, currentRowPosition + 1, currentPosition) : null; 

        Octopus? leftUp = left != null && middleUp != null ? new Octopus(inputRaw, currentRowPosition-1, currentPosition-1) : null;
        Octopus? rightUp = right != null && middleUp != null ? new Octopus(inputRaw, currentRowPosition - 1, currentPosition + 1) : null; 
        Octopus? leftDown = left != null && middleDown != null ? new Octopus(inputRaw, currentRowPosition + 1, currentPosition - 1) : null;
        Octopus? rightDown = right != null && middleDown != null ? new Octopus(inputRaw, currentRowPosition + 1, currentPosition + 1) : null;
    }

    public Octopus? MiddleUp => middleUp;
    public Octopus? Left => left;
    public Octopus? Right => right;
    public Octopus? MiddleDown => middleDown;

    Octopus? LeftUp => leftUp;
    Octopus? RightUp => rightUp;
    Octopus? LeftDown => leftDown;
    Octopus? RightDown => rightDown;

    public int NumberOfFlashes { get; set; }

    public string CurrentValue => currentRow[currentPosition].ToString();

    public bool IsSmallest => CurrentValue.IsSmallerThen(MiddleUp?.CurrentValue) && CurrentValue.IsSmallerThen(Left?.CurrentValue)
        && CurrentValue.IsSmallerThen(Right?.CurrentValue) && CurrentValue.IsSmallerThen(MiddleDown?.CurrentValue);
    public bool IsPartOfBassin => Convert.ToInt32(CurrentValue) < 9;

    internal void CollectNeighbours()
    {
        throw new NotImplementedException();
    }
}

public static class Ext
{
    public static bool IsSmallerThen(this string current, string? neighbour)
    {
        return String.IsNullOrEmpty(neighbour) || (Convert.ToInt32(current) < Convert.ToInt32(neighbour));
    }

    public static bool IsRelevant(this Octopus currentPoint, List<Octopus> handledPoints, List<Octopus> unhandledPoints)
    {
        return currentPoint != null
            && currentPoint.IsPartOfBassin
            && !currentPoint.IsAlreadyHandled(handledPoints)
            && !unhandledPoints.Any(x => x.currentRowPosition == currentPoint.currentRowPosition && x.currentPosition == currentPoint.currentPosition);
    }

    public static bool IsAlreadyHandled(this Octopus currentPoint, List<Octopus> handledPoints)
    {
        return handledPoints.Any(p => p.currentPosition == currentPoint.currentPosition && p.currentRowPosition == currentPoint.currentRowPosition);
    }
}