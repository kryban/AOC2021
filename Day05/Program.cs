// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
var inputRaw = File.ReadAllLines(inputFile).ToList();

List<Line> relevantLines = ImportHorizontalOrVerticalLines(inputRaw);

List<(int x, int y)> linePoints = CollectCoveredPoints(relevantLines);

int answer = CountMatches(linePoints);

Console.WriteLine($"Answer Day 5, Part 1: {answer}");
Console.ReadKey();

int CountMatches(List<(int x, int y)> linePoints)
{
    var foo = linePoints.GroupBy(x => x)
        .Where(xx => xx.Count() > 1)
        .Select(xxx => xxx)
        .ToList();

    return foo.Count();
}

List<(int x, int y)> CollectCoveredPoints(List<Line> relevantLines)
{
    List<(int x, int y)> retval = new List<(int x, int y)>();

    relevantLines.ForEach(x => x.series.ForEach(xx => retval.Add(xx)));

    return retval;
}

List<Line> ImportHorizontalOrVerticalLines(List<string> inputRaw)
{
    List<Line> retval = new List<Line>();

    var cleanedInput = inputRaw.Select(x => ReplaceArrow(x)).ToList();

    cleanedInput.ForEach(line => retval.Add(CreateLineFromInput(line)));

    retval = FilterVerticalOrHorizontalOnly(retval);

    return retval;
}

string ReplaceArrow(string input)
{
    return input.Trim().Replace(" ", string.Empty).Replace("->", ",");
}

Line CreateLineFromInput(string input)
{
    var points = input.Split(',').Select(x => int.Parse(x)).ToList();

    return new Line
    {
        startX = points[0],
        startY = points[1],
        endX = points[2],
        endY = points[3],

        series = SetLineSerie(points[0], points[1], points[2], points[3])
    };
}

List<(int x, int y)> SetLineSerie(int startX, int startY, int endX, int endY)
{   
    var retval = new List<(int x,int y)>();

    if (startX == endX)
    {
        if (endY > startY)
        {
            for (int i = startY; i <= endY; i++)
            {
                retval.Add((startX, i));
            }
        }
        else
        {
            for (int i = endY; i <= startY; i++)
            {
                retval.Add((startX, i));
            }
        }
    }
    else // startY == endY
    {
        if (endX > startX)
        {
            for (int i = startX; i <= endX; i++)
            {
                retval.Add((i, startY));
            }
        }
        else
        {
            for (int i = endX; i <= startX; i++)
            {
                retval.Add((i, startY));
            }
        }
    }

    return retval;
}

static List<Line> FilterVerticalOrHorizontalOnly(List<Line> unfiltered)
{
    var retval = new List<Line>(unfiltered);

    foreach (var line in unfiltered)
    {
        if (line.startX != line.endX && line.startY != line.endY)
        {
            retval.Remove(line);
        }
    }

    return retval;
}
internal class Line
{
    public int startX{ get; set; }
    public int startY { get; set; }
    public int endX { get; set; }
    public int endY { get; set; }

    public List<(int x,int y)> series { get; set; }
}