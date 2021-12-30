// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/inputExample2.txt";
List<string> inputRaw = File.ReadAllLines(inputFile).ToList();

List<Octopus> octoPussies = new List<Octopus>();

ConvertToOctopussies(inputRaw, octoPussies);

IncreaseEnergyInSteps(1, octoPussies);

Console.WriteLine($"Day 11, part 1: ");

void ConvertToOctopussies(List<string> inputRaw, List<Octopus> octoPussies)
{  
    var rowPosition = 0;
    foreach (var line in inputRaw)
    {
        var currenPosition = 0;
        foreach(var item in line)
        {
            var o = new Octopus(inputRaw, octoPussies, rowPosition, currenPosition, Convert.ToInt32(item.ToString()));
            octoPussies.Add(o);

            currenPosition++;
        }
        rowPosition++;
    }
 }

void IncreaseEnergyInSteps(int numberOfSteps, List<Octopus> octoPussies)
{
    int i = 0;
    while (i <= numberOfSteps)
    {
        octoPussies.ForEach(octo => octo.CurrentValue++);
        octoPussies.ForEach(octo => octo.ConsiderNeighbourIncrease());

        i++;
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

    public Octopus(List<string> inputRaw, List<Octopus> octoPussies, int currentRowPosition, int currentPosition, int currentValue)
    {
        this.inputRaw = inputRaw;
        this.octoPussies = octoPussies;
        this.currentRowPosition = currentRowPosition;
        this.currentPosition = currentPosition;
        CurrentValue = currentValue;
    }

    public Octopus? MiddleUp => !String.IsNullOrEmpty(previousRow) ? Find(octoPussies, currentRowPosition - 1, currentPosition) : null;
    public Octopus? Left => currentPosition > 0 ? Find(octoPussies, currentRowPosition, currentPosition - 1) : null;
    public Octopus? Right => currentPosition + 1 <= currentRow.Length - 1 ? Find(octoPussies, currentRowPosition, currentPosition + 1) : null;
    public Octopus? MiddleDown => !String.IsNullOrEmpty(nextRow) ? Find(octoPussies, currentRowPosition + 1, currentPosition) : null;

    public Octopus? LeftUp => Left != null && MiddleUp != null ? Find(octoPussies, currentRowPosition - 1, currentPosition - 1) : null;
    public Octopus? RightUp => Right != null && MiddleUp != null ? Find(octoPussies, currentRowPosition - 1, currentPosition + 1) : null;
    public Octopus? LeftDown => Left != null && MiddleDown != null ? Find(octoPussies, currentRowPosition + 1, currentPosition - 1) : null;
    public Octopus? RightDown => Right != null && MiddleDown != null ? Find(octoPussies, currentRowPosition + 1, currentPosition + 1) : null;
 
    public int NumberOfFlashes { get; set; }

    private int currentValue;
    public int CurrentValue { get => currentValue;
        set
        {
            currentValue = value;
            if(CurrentValue > 9)
            {
                NumberOfFlashes++;
                CurrentValue = 0;
            }
        }
    }

    public void IncreaseNeighbours()
    {
        LeftUp?.Increase();
        MiddleUp?.Increase();
        RightUp?.Increase();
        Left?.Increase();
        Right?.Increase(); 
        LeftDown?.Increase();
        MiddleDown?.Increase();
        RightDown?.Increase();
    }

    internal void ConsiderNeighbourIncrease()
    {
        if (CurrentValue == 0)
        {
            IncreaseNeighbours();
        }
    }

    private Octopus? Find(List<Octopus> octopussies, int currentRowPosition, int currenPosition)
    {
        return octopussies.FirstOrDefault(o => o.currentRowPosition == currentRowPosition && o.currentPosition == currenPosition);
    }
}

public static class Ext
{
    public static void Increase(this Octopus octo)
    {
        // only auto increase after flash if value is not 0. Else it must be increased by step
        if (octo.CurrentValue > 0)
            octo.CurrentValue++;
    }
}