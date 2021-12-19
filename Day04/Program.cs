// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
var inputRaw = File.ReadAllLines(inputFile).ToList();

List<int> drawnNumbers = inputRaw.First().Split(',').Select(x => int.Parse(x)).ToList();
List<BingoCard> cards = new List<BingoCard>();

int winningNumber = 0;
int sumOfRestUnmarked = 0;

ImportCards(inputRaw, cards);

MarkDrawnNumbers(drawnNumbers, cards, ref sumOfRestUnmarked, ref winningNumber);


Console.WriteLine($"WinningNumber: {winningNumber} - Sum of rest of Unmarked numbers: {sumOfRestUnmarked}");
Console.WriteLine($"Answer part 1: {winningNumber*sumOfRestUnmarked}"); 
Console.ReadKey();

void MarkDrawnNumbers(List<int> drawnNumbers, List<BingoCard> cards, ref int sumOfRestUnmarked, ref int winningNumber)
{
    var stop = false;
    foreach(var nr in drawnNumbers)
    {
        foreach (var card in cards)
        {
            if(card.UnmarkedNumbers.Any(x => x.Number == nr))
            {
                card.MarkedNumbers.AddRange(card.UnmarkedNumbers.Where(x => x.Number == nr));
                card.UnmarkedNumbers.RemoveAll(x => x.Number == nr);

                // check for bingo
                for (int i = 0; i < 4; i++)
                {
                    var foo = card.MarkedNumbers.Count(x => x.Row == i);
                    var bar = card.MarkedNumbers.Count(x => x.Column == i);

                    if(foo == 5 || bar == 5)
                    {
                        // calculate answer
                        Console.WriteLine("BINGO");
                        winningNumber = nr;
                        sumOfRestUnmarked = card.UnmarkedNumbers.Sum(x => x.Number);
                        stop = true;
                        break;
                    }
                }
            }
            if (stop) break;
        }
        if (stop) break;
    }
}

void ImportCards(List<string> inputRaw, List<BingoCard> cards)
{
    BingoCard card = new BingoCard(); ;
    int currentRow = 0;
    int currentColumn = 0;

    foreach (var row in inputRaw.Skip(1)) // skip first because drawn numbers 
    {
        if (row.Length == 0)
        {
            if (card.UnmarkedNumbers != null && card.UnmarkedNumbers.Any())
            {
                cards.Add(card);
            }

            currentRow = 0;
            currentColumn = 0;
            card = new BingoCard();
            card.UnmarkedNumbers = new List<BingoNumber>();
        }
        else
        {
            foreach (var nr in row.Replace("  "," ").Split(' ').Where(x => x.Length > 0))
            {
                card.UnmarkedNumbers.Add(
                    new BingoNumber { Number = Convert.ToInt32(nr), Row = currentRow, Column = currentColumn });

                currentColumn++;
            }

            currentColumn = 0;
            currentRow++;
        }

        if (inputRaw.IndexOf(row).Equals(inputRaw.FindLastIndex(x => x.Length > 0)))
        {
            cards.Add(card);
        }
    }
}

internal class BingoCard
{
    public BingoCard()
    {
        UnmarkedNumbers = new List<BingoNumber>();
        MarkedNumbers = new List<BingoNumber>();
    }

    public List<BingoNumber> UnmarkedNumbers { get; set; }
    public List<BingoNumber> MarkedNumbers { get; set; }
}

internal class BingoNumber
{
    public int Number { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
}