// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
List<string> inputRaw = File.ReadAllLines(inputFile).ToList();

List<Octopus> octoPussies = new List<Octopus>();

ConvertToOctopussies(inputRaw, octoPussies);

IncreaseEnergyInSteps(100, octoPussies);

Console.WriteLine("///////////////////////////////////////////////");
Console.WriteLine($"Day 11, part 1 (1743): {octoPussies.Sum(x => x.NumberOfFlashes)}");
Console.WriteLine("///////////////////////////////////////////////");
Console.WriteLine($"\nDay 11, part 2 (364): {CalculateStepOfTotalSynchronisation(octoPussies)}");
Console.WriteLine("///////////////////////////////////////////////");
Console.ReadKey();

void ConvertToOctopussies(List<string> inputRaw, List<Octopus> octoPussies)
{
    octoPussies.RemoveAll(x => x.CurrentValue > -1);
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
    int i = 1;
    while (i <= numberOfSteps)
    {
        //when set to zero, it freezes on zero during the whole step. 
        //so when property is increased by a neighbour, but is not processed yet by this increase, then it should NOT be increased untill the next step/
        octoPussies.ForEach(octo => { if (!octo.ZeroIsSetInCurrentStep) octo.CurrentValue++; });

        while(octoPussies.Any(octo => octo.ZeroIsSetInCurrentStep && !octo.NeighboursIncreaseFinished))
        {
            octoPussies.ForEach(o => { if (o.ZeroIsSetInCurrentStep && !o.NeighboursIncreaseFinished) o.IncreaseNeighbours(); });
        }

        Console.WriteLine($"After update with 1 in step {i}");
        Console.WriteLine(PrintCurrentState(octoPussies));

        //prepare for the next step, in which all octopusses can be increased to zero
        octoPussies.ForEach(o => o.ZeroIsSetInCurrentStep = false);

        i++;
    }
}

int CalculateStepOfTotalSynchronisation(List<Octopus> octoPussies)
{
    ConvertToOctopussies(inputRaw, octoPussies);

    int retval = 0;
    int i = 1;
    while (retval == 0)
    {
        //when set to zero, it freezes on zero during the whole step. 
        //so when property is increased by a neighbour, but is not processed yet by this increase, then it should NOT be increased untill the next step/
        octoPussies.ForEach(octo => { if (!octo.ZeroIsSetInCurrentStep) octo.CurrentValue++; });

        while (octoPussies.Any(octo => octo.ZeroIsSetInCurrentStep && !octo.NeighboursIncreaseFinished))
        {
            octoPussies.ForEach(o => { if (o.ZeroIsSetInCurrentStep && !o.NeighboursIncreaseFinished) o.IncreaseNeighbours(); });
        }

        Console.WriteLine($"Step {i}");
        Console.WriteLine(PrintCurrentState(octoPussies));

        if (!octoPussies.Any(o => o.CurrentValue > 0))
        {
            retval = i;
        }

        //prepare for the next step, in which all octopusses can be increased to zero
        octoPussies.ForEach(o => o.ZeroIsSetInCurrentStep = false);

        i++;
    }

    return retval;
}

string PrintCurrentState(List<Octopus> octos)
{
    // just for presentation purposes
    string retval = "";
    octoPussies.Take(10).ToList().ForEach(o => retval += $"{o.CurrentValue.ToString()}"); retval += Environment.NewLine;
    octoPussies.Skip(10).Take(10).ToList().ForEach(o => retval += $"{o.CurrentValue.ToString()}"); retval += Environment.NewLine;
    octoPussies.Skip(20).Take(10).ToList().ForEach(o => retval += $"{o.CurrentValue.ToString()}");retval += Environment.NewLine;
    octoPussies.Skip(30).Take(10).ToList().ForEach(o => retval += $"{o.CurrentValue.ToString()}");retval += Environment.NewLine;
    octoPussies.Skip(40).Take(10).ToList().ForEach(o => retval += $"{o.CurrentValue.ToString()}");retval += Environment.NewLine;
    octoPussies.Skip(50).Take(10).ToList().ForEach(o => retval += $"{o.CurrentValue.ToString()}");retval += Environment.NewLine;
    octoPussies.Skip(60).Take(10).ToList().ForEach(o => retval += $"{o.CurrentValue.ToString()}");retval += Environment.NewLine;
    octoPussies.Skip(70).Take(10).ToList().ForEach(o => retval += $"{o.CurrentValue.ToString()}");retval += Environment.NewLine;
    octoPussies.Skip(80).Take(10).ToList().ForEach(o => retval += $"{o.CurrentValue.ToString()}");retval += Environment.NewLine;
    octoPussies.Skip(90).Take(10).ToList().ForEach(o => retval += $"{o.CurrentValue.ToString()}");retval += Environment.NewLine;

    return retval;
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

    public bool ZeroIsSetInCurrentStep;
    public bool NeighboursIncreaseFinished;

    private int currentValue;
    public int CurrentValue { get => currentValue;
        set
        {
            currentValue = value;
            NeighboursIncreaseFinished = false;
            if(CurrentValue > 9)
            {
                NumberOfFlashes++;
                currentValue = 0;
                ZeroIsSetInCurrentStep = true;
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

        NeighboursIncreaseFinished = true;
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