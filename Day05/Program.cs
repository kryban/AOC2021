// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
var inputRaw = File.ReadAllLines(inputFile).ToList();

List<Line> relevantLinesPart1 = ImportRelevantLines(1,inputRaw);
List<Line> relevantLinesPart2 = ImportRelevantLines(2, inputRaw);

List<(int x, int y)> linePointsPart1 = CollectCoveredPoints(relevantLinesPart1);
List<(int x, int y)> linePointsPart2 = CollectCoveredPoints(relevantLinesPart2);

int answer1 = CountMatches(linePointsPart1);
int answer2 = CountMatches(linePointsPart2);

Console.WriteLine($"Answer Day 5, Part 1: {answer1}");
Console.WriteLine($"Answer Day 5, Part 2: {answer2}");
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

List<Line> ImportRelevantLines(int part, List<string> inputRaw)
{
    List<Line> retval = new List<Line>();

    var cleanedInput = inputRaw.Select(x => ReplaceArrow(x)).ToList();

    cleanedInput.ForEach(line => retval.Add(CreateLineFromInput(part, line)));

    retval = FilterForRelevantLines(part,retval);

    return retval;
}

string ReplaceArrow(string input)
{
    return input.Trim().Replace(" ", string.Empty).Replace("->", ",");
}

Line CreateLineFromInput(int part, string input)
{
    var points = input.Split(',').Select(x => int.Parse(x)).ToList();

    return new Line
    {
        startX = points[0],
        startY = points[1],
        endX = points[2],
        endY = points[3],

        series = part == 1 
        ? SetLineSeriePart2(points[0], points[1], points[2], points[3])
        : SetLineSeriePart2(points[0], points[1], points[2], points[3])
    };
}

List<(int x, int y)> SetLineSeriePart1(int startX, int startY, int endX, int endY)
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

List<(int x, int y)> SetLineSeriePart2(int startX, int startY, int endX, int endY)
{
    var retval = new List<(int x, int y)>();
    //var difference = Math.Abs(startX - endX);
    var sum = startX + startY;

    if (startX < endX && startY < endY)
    {
        for (int i = 0; i <= Math.Abs(startX - endX); i++)
        {
            retval.Add((startX + i, startY + i));
        }
    }

    else if (startX < endX && startY > endY)
    {
        for (int i = 0; i <= Math.Abs(startX - endX); i++)
        {
            retval.Add((startX + i, startY - i));
        }
    }

    else if (startX < endX && (startY == endY))
    {
        for (int i = 0; i <= Math.Abs(startX - endX); i++)
        {
            retval.Add((startX + i, startY));
        }
    }

    else if (startX > endX && startY > endY)
    {
        for (int i = 0; i <= Math.Abs(startX - endX); i++)
        {
            retval.Add((startX - i, startY - i));
        }
    }

    else if (startX > endX && startY < endY)
    {
        for (int i = 0; i <= Math.Abs(startX - endX); i++)
        {
            retval.Add((startX - i, startY + i));
        }
    }

    else if (startX > endX && (startY == endY))
    {
        for (int i = 0; i <= Math.Abs(startX - endX); i++)
        {
            retval.Add((startX - i, startY));
        }
    }

    else if ((startX == endX) && startY > endY)
    {
        for (int i = 0; i <= Math.Abs(startY - endY); i++)
        {
            retval.Add((startX, startY-i));
        }
    }
    else if ((startX == endX) && startY < endY)
    {
        for (int i = 0; i <= Math.Abs(startY - endY); i++)
        {
            retval.Add((startX, startY + i));
        }
    }
    else
    {
            retval.Add((startX, startY));
    }

    return retval;
}

List<Line> FilterForRelevantLines(int part, List<Line> unfiltered)
{
    var retval = new List<Line>(unfiltered);

    foreach (var line in unfiltered)
    {
        if (part == 1)
        {
            if (!HasSameX(line) && !HasSameY(line))
            {
                retval.Remove(line);
            }
        }
        else 
        {
            if (!HasSameX(line) && !HasSameY(line) && !Is45Degrees(line))
            {
                retval.Remove(line);
            }
        }
    }

    return retval;
}

bool Is45Degrees(Line line)
{
    return (Math.Abs(line.startX - line.endX)) == (Math.Abs(line.startY - line.endY));
}

bool HasSameX(Line line)
{
    return line.startX == line.endX;
}

bool HasSameY(Line line)
{
    return line.startY == line.endY;
}

internal class Line
{
    public int startX{ get; set; }
    public int startY { get; set; }
    public int endX { get; set; }
    public int endY { get; set; }

    public List<(int x, int y)> series { get; set; }
}