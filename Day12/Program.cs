// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/inputExample2.txt";
List<string> inputRaw = File.ReadAllLines(inputFile).ToList();
List<Cave> caves = ConvertToCaves(inputRaw);



List<Cave> ConvertToCaves(List<string> inputRaw)
{
    List<Cave> retval = new List<Cave>();

    foreach (var inp in inputRaw)
    {
        var left = inp.Split('-')[0];
        var right = inp.Split('-')[1];

        // an end cave can ONLY be used a a neighbour and not as a cave
        if (!retval.Any(c => c.Name.Equals(left)) && !left.Equals("end"))
            retval.Add(new Cave(left));
        if (!retval.Any(c => c.Name.Equals(right)) && !right.Equals("end"))
            retval.Add(new Cave(right));

        // a start cave cannot be set as a neighbour to travel to
        if (!CaveContainsNeighbour(retval, left, right) && !left.Equals("end") && !right.Equals("start"))
        {
            retval.Find(c => c.Name.Equals(left)).Neighbours.Add(new Cave(right));
        }

        if (!CaveContainsNeighbour(retval, right, left) && !right.Equals("end") && !left.Equals("start"))
        {
            retval.Find(c => c.Name.Equals(right)).Neighbours.Add(new Cave(left));
        }
    }

    return retval;
}

bool CaveContainsNeighbour(List<Cave> retval, string cavename, string neighbourname)
{
    // the start cave is not a neighbour to travel to, so don't add it as a neighbour
    return retval.Any(c => c.Name.Equals(cavename)) &&
        retval.Find(c => c.Name.Equals(cavename)).Neighbours.Any(n => n.Equals(neighbourname));
}

public class Cave
{
    public Cave(string name)
    {
        Name = name;
        Neighbours = new List<Cave>(); 
    }
    public string Name { get; set; }
    public bool IsLarge { get; set; }
    public bool IsSmall { get; set; }
    public bool IsPassed { get; set; }
    public List<Cave> Neighbours { get; set; }
}