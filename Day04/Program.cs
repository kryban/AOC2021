// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string inputFile = @"./ProjectItems/input.txt";
var inputRaw = File.ReadAllLines(inputFile).ToList();

List<int> drawnNumbers = inputRaw.First().Split(',').Select(x => int.Parse(x)).ToList();
string nrs = inputRaw.First();

List<BingoCard> cards = new List<BingoCard>();
int winningNumber = 0;
int sumOfRestUnmarked = 0;

ImportCards(inputRaw, cards);
CheckForBingo(1,drawnNumbers, cards, ref sumOfRestUnmarked, ref winningNumber);
Console.WriteLine($"All numbers to draw: {nrs}");
Console.WriteLine($"WinningNumber: {winningNumber} - Sum of rest of Unmarked numbers: {sumOfRestUnmarked}");
Console.WriteLine($"Answer Day 4, Part 1: {winningNumber * sumOfRestUnmarked}");
Console.WriteLine("-----------");

cards = new List<BingoCard>();
winningNumber = 0;
sumOfRestUnmarked = 0;

ImportCards(inputRaw, cards);
CheckForBingo(2, drawnNumbers, cards, ref sumOfRestUnmarked, ref winningNumber);
Console.WriteLine($"All numbers to draw: {nrs}");
Console.WriteLine($"WinningNumber: {winningNumber} - Sum of rest of Unmarked numbers: {sumOfRestUnmarked}");
Console.WriteLine($"Answer Day 4, Part 2: {winningNumber * sumOfRestUnmarked}");

Console.ReadKey();

void CheckForBingo(int part, List<int> drawnNumbers, List<BingoCard> cards, ref int sumOfRestUnmarked, ref int winningNumber)
{
    // for part 1 we stop at the FIRST winning bingo card 
    // for part 2 we stop at the LAST winning bingo card
    bool stop = false;
    List<BingoCard> shadowCards = new List<BingoCard>(cards);
    foreach (var nr in drawnNumbers)
    {
        foreach (var card in new List<BingoCard>(shadowCards)) // new List instead of cards directly, to be able to modify the collection of iteration
        {
            if (NumberPresentOnCard(nr, card))
            {
                MarkNumbers(nr, card);
                CheckBingo(shadowCards, card, nr, ref stop);
            }
            if (stop && part == 1) break;
        }
        if (stop && part == 1) break;
    }

    cards = new List<BingoCard>(shadowCards);
}


void CheckBingo(List<BingoCard> cards, BingoCard card, int nr, ref bool stop)
{
    for (int i = 0; i <= 4; i++)
    {
        var markedInSameRow = card.MarkedNumbers.Count(x => x.Row == i);
        var markedInSamerColumn = card.MarkedNumbers.Count(x => x.Column == i);

        if (markedInSameRow == 5 || markedInSamerColumn == 5)
        {
            Console.WriteLine($"BINGO with {nr}");
            winningNumber = nr;
            sumOfRestUnmarked = card.UnmarkedNumbers.Sum(x => x.Number);
            cards.Remove(card);
            stop = true;
            break;
        }
    }
}

bool NumberPresentOnCard(int nr, BingoCard card)
{
    return card.UnmarkedNumbers.Any(x => x.Number == nr);
}

void MarkNumbers(int nr, BingoCard card)
{
    card.MarkedNumbers.AddRange(card.UnmarkedNumbers.Where(x => x.Number == nr));
    card.UnmarkedNumbers.RemoveAll(x => x.Number == nr);
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
            foreach (var nr in row.Replace("  ", " ").Split(' ').Where(x => x.Length > 0))
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