// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
List<string> inputRaw = File.ReadAllLines(inputFile).ToList();
List<int> lowestPoints = new List<int>();


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
        List<PartPoint> unhandleidPoints = new List<PartPoint>();

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

lowestPoints.ForEach(p => Console.Write(p));
Console.WriteLine();
Console.WriteLine($"Day 9, part 1: {CalculatePart1(inputRaw)}");
Console.ReadKey();

int CalculatePart1(List<string> inputRaw)
{
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

            if (p.Smallest)
                retval.Add(p);
        }
    }
    return retval;
}

public class PartPoint
{
    private List<string> inputRaw = new List<string>();
    private int rowPosition;
    private int hPosition;

    public PartPoint(List<string> inputRaw, int rowPosition, int hPosition)
    {
        this.inputRaw = inputRaw;
        this.rowPosition = rowPosition;
        this.hPosition = hPosition;
    }

    private string previousRow => rowPosition > 0 ? inputRaw[rowPosition - 1] : String.Empty;
    private string currentRow => inputRaw[rowPosition];
    private string nextRow => rowPosition <= inputRaw.Count - 2 ? inputRaw[rowPosition + 1] : String.Empty;

    private string above => !String.IsNullOrEmpty(previousRow) ? previousRow[hPosition].ToString() : String.Empty;
    private string left => hPosition > 0 ? currentRow[hPosition - 1].ToString() : String.Empty;
    private string right => hPosition + 1 <= currentRow.Length - 1 ? currentRow[hPosition + 1].ToString() : String.Empty;
    private string below => !String.IsNullOrEmpty(nextRow) ? nextRow[hPosition].ToString() : String.Empty;

    public string CurrentValue => currentRow[hPosition].ToString();

    public bool Smallest => CurrentValue.IsSmallerThen(above) && CurrentValue.IsSmallerThen(left)
        && CurrentValue.IsSmallerThen(right) && CurrentValue.IsSmallerThen(below);
    public bool PartOfBassin => !Smallest;

}

public static class Ext
{
    public static bool IsSmallerThen(this string current, string neighbour)
    {
        bool retval;
        retval = String.IsNullOrEmpty(neighbour) || (Convert.ToInt32(current) < Convert.ToInt32(neighbour));
        return retval;
    }
}