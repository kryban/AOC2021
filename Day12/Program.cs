// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/inputExample2.txt";
List<string> inputRaw = File.ReadAllLines(inputFile).ToList();
List<Cave> caves = ConvertToCaves(inputRaw);

Console.WriteLine($"Day 12, part 1: {CalculateAllRoutes(caves)}");

int CalculateAllRoutes(List<Cave> caves)
{
    List<string> routes = new List<string>();
    int retval = 0;
    bool continueSearch = true;

    var startCave = caves.Find(x => x.Name.Equals("start"));

    while (continueSearch)
    {
        string route = string.Empty;
        startCave.RegisterNode(ref route);
        startCave.PassPassable(ref route, ref continueSearch);
        
        if(!routes.Any(c => c.Equals(route)))
            routes.Add(route);
        else
        {
            break;
        }
    }

    return routes.Count(); ;
}

List<Cave> ConvertToCaves(List<string> inputRaw)
{
    List<Cave> retval = new List<Cave>();

    foreach (var inp in inputRaw)
    {
        var left = inp.Split('-')[0];
        var right = inp.Split('-')[1];

        if (!retval.Any(c => c.Name.Equals(left)))
            retval.Add(new Cave(left));
        if (!retval.Any(c => c.Name.Equals(right)))
            retval.Add(new Cave(right));

        // a start cave cannot be set as a neighbour to travel to
        if (!CaveContainsNeighbour(retval, left, right) && !right.Equals("start"))
        {
            retval.Find(c => c.Name.Equals(left)).Neighbours.Add(retval.Find(cc => cc.Name.Equals(right)));
        }

        if (!CaveContainsNeighbour(retval, right, left) && !left.Equals("start"))
        {
            retval.Find(c => c.Name.Equals(right)).Neighbours.Add(retval.Find(cc => cc.Name.Equals(left)));
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
    public bool isPassedOnce;
    public Cave(string name)
    {
        Name = name;
        Neighbours = new List<Cave>(); 
    }
    public string Name { get; set; }
    public bool IsLarge => char.IsUpper(Name[0]);
    public bool IsPassable => IsLarge || (!IsLarge && !isPassedOnce);
    public List<Cave> Neighbours { get; set; }

    internal void RegisterNode(ref string route)
    {
        route += Name;
    }

    internal void PassPassable(ref string route, ref bool continueSearch)
    {
        Console.WriteLine($"Passed {Name}");
        var c = Neighbours.Find(x => x.IsPassable);

        if (c != null)
        {
            if(!c.Name.Equals("end"))
            {
                c.isPassedOnce = true;
                c.RegisterNode(ref route);
                c.PassPassable(ref route, ref continueSearch);
            }
            
            c.RegisterNode(ref route);
        }
        else
        {
            //continueSearch = false;
        }
    }
}