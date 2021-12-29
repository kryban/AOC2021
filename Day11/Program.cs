// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
List<string> inputRaw = File.ReadAllLines(inputFile).ToList();

List<Octopus> octoPussies = new List<Octopus>();

ConvertToOctopussies(inputRaw, octoPussies);

Console.WriteLine($"Day 11, part 1: ");

void ConvertToOctopussies(List<string> inputRaw, List<Octopus> octoPussies)
{  
    var rowPosition = 0;
    foreach (var line in inputRaw)
    {
        var currenPosition = 0;
        foreach(var item in line)
        {
            var o = new Octopus(inputRaw, octoPussies, rowPosition, currenPosition);
            octoPussies.Add(o);

            currenPosition++;
        }
        rowPosition++;
    }
 }

public class Octopus
{
    private List<string> inputRaw = new List<string>();
    private List<Octopus> octoPussies; 

    public int currentRowPosition;
    public int currentPosition;

    private string previousRow => currentRowPosition > 0 ? inputRaw[currentRowPosition - 1] : String.Empty;
    private string currentRow => inputRaw[currentRowPosition];
    private string nextRow => currentRowPosition <= inputRaw.Count - 2 ? inputRaw[currentRowPosition + 1] : String.Empty;

    public Octopus(List<string> inputRaw, List<Octopus> octoPussies, int currentRowPosition, int currentPosition)
    {
        this.inputRaw = inputRaw;
        this.octoPussies = octoPussies;
        this.currentRowPosition = currentRowPosition;
        this.currentPosition = currentPosition;
    }

    private Octopus? middleUp => !String.IsNullOrEmpty(previousRow) ? Find(octoPussies, currentRowPosition - 1, currentPosition) : null;
    private Octopus? left => currentPosition > 0 ? Find(octoPussies, currentRowPosition, currentPosition - 1) : null;
    private Octopus? right => currentPosition + 1 <= currentRow.Length - 1 ? Find(octoPussies, currentRowPosition, currentPosition + 1) : null;
    private Octopus? middleDown => !String.IsNullOrEmpty(nextRow) ? Find(octoPussies, currentRowPosition + 1, currentPosition) : null;

    private Octopus? leftUp => left != null && middleUp != null ? Find(octoPussies, currentRowPosition - 1, currentPosition - 1) : null;
    private Octopus? rightUp => right != null && middleUp != null ? Find(octoPussies, currentRowPosition - 1, currentPosition + 1) : null;
    private Octopus? leftDown => left != null && middleDown != null ? Find(octoPussies, currentRowPosition + 1, currentPosition - 1) : null;
    private Octopus? rightDown => right != null && middleDown != null ? Find(octoPussies, currentRowPosition + 1, currentPosition + 1) : null;

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

    private Octopus? Find(List<Octopus> octopussies, int currentRowPosition, int currenPosition)
    {
        return octoPussies.FirstOrDefault(o => o.currentRowPosition == currenPosition && o.currentPosition == currentPosition);
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