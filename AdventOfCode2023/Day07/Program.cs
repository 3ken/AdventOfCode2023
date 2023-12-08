var lines = File.ReadAllLines("Data.txt");
// var lines = File.ReadAllLines("ExampleData.txt");

First();
Second();
return;

void First()
{
    var allGroupedHands = GetAllHands(1)
        .GroupBy(h => h.HandType)
        .OrderBy(gh => gh.First().HandType)
        .Select(hg => hg
            .OrderByDescending(h => h.GetCardValue(0))
            .ThenByDescending(h => h.GetCardValue(1))
            .ThenByDescending(h => h.GetCardValue(2))
            .ThenByDescending(h => h.GetCardValue(3))
            .ThenByDescending(h => h.GetCardValue(4)));

    long totalWinning = 0;
    var all = allGroupedHands.SelectMany(g => g).ToList();
    var rank = all.Count;
    foreach (var hand in all)
    {
        var winning = hand.GetWinning(rank);
        totalWinning += winning;
        rank--;
    }
    Console.WriteLine(totalWinning);
}

void Second()
{
    var allGroupedHands = GetAllHands(2)
        .GroupBy(h => h.HandType)
        .OrderBy(gh => gh.First().HandType)
        .Select(hg => hg
            .OrderByDescending(h => h.GetCardValue(0))
            .ThenByDescending(h => h.GetCardValue(1))
            .ThenByDescending(h => h.GetCardValue(2))
            .ThenByDescending(h => h.GetCardValue(3))
            .ThenByDescending(h => h.GetCardValue(4)));

    long totalWinning = 0;
    var all = allGroupedHands.SelectMany(g => g).ToList();
    var rank = all.Count;
    foreach (var hand in all)
    {
        var winning = hand.GetWinning(rank);
        totalWinning += winning;
        rank--;
    }
    Console.WriteLine(totalWinning);
}

IEnumerable<Hand> GetAllHands(int task)
{
    return lines.Select(line => new Hand(line.Split(' ')[0].ToCharArray(), long.Parse(line.Split(' ')[1]), task));
}

public class Hand()
{
    private char[] Cards { get; } = Array.Empty<char>();
    private int[] IntCards { get; } = Array.Empty<int>();
    private long Bid { get; }
    public HandType HandType { get; }

    public Hand(char[] cards, long bid, int task) : this()
    {
        Cards = cards;
        Bid = bid;
        IntCards = GetIntCards(cards, task);
        HandType = GetHandType(cards, task);
    }

    private static int[] GetIntCards(IEnumerable<char> cards, int task)
    {
        var toReturn = new List<int>();
        var jInt = task == 1 ? 11 : 1;
        foreach (var card in cards)
        {
            switch (card)
            {
                case 'A':
                    toReturn.Add(14);
                    break;
                case 'K':
                    toReturn.Add(13);
                    break;
                case 'Q':
                    toReturn.Add(12);
                    break;
                case 'J':
                    toReturn.Add(jInt);
                    break;
                case 'T':
                    toReturn.Add(10);
                    break;
                default:
                    toReturn.Add(int.Parse(card.ToString()));
                    break;
            }
        }

        return toReturn.ToArray();
    }

    private static HandType GetHandType(IReadOnlyList<char> cards, int task)
    {
        var numberOfJokers = task == 1 ? 0 : cards.Count(c => c == 'J');
        if (FiveOfAKind(cards, numberOfJokers)) return HandType.FiveOfAKind;
        if (FourOfAKind(cards, numberOfJokers)) return HandType.FourOfAKind;
        if (FullHouse(cards, numberOfJokers)) return HandType.FullHouse;
        if (ThreeOfAKind(cards, numberOfJokers)) return HandType.ThreeOfAKind;
        if (TwoPair(cards, numberOfJokers)) return HandType.TwoPair;
        if (OnePair(cards, numberOfJokers)) return HandType.OnePair;
        return HandType.HighCard;
    }

    private static bool FiveOfAKind(IEnumerable<char> cards, int numberOfJokers = 0)
    {
        var differentCards = new Dictionary<char, int>();

        foreach (var t in cards)
        {
            if (!differentCards.TryAdd(t, 1))
            {
                differentCards[t] += 1;
            }
        }
        
        if (numberOfJokers is > 0 and < 5)
            differentCards.Remove('J');

        var highestValue = differentCards.MaxBy(dc => dc.Value).Value;

        return highestValue == 5 || highestValue + numberOfJokers == 5;
    }

    private static bool FourOfAKind(IEnumerable<char> cards, int numberOfJokers = 0)
    {
        var differentCards = new Dictionary<char, int>();

        foreach (var t in cards)
        {
            if (!differentCards.TryAdd(t, 1))
            {
                differentCards[t] += 1;
            }
        }

        if (numberOfJokers > 0)
            differentCards.Remove('J');
        var highestValue = differentCards.MaxBy(dc => dc.Value).Value;

        return highestValue + numberOfJokers == 4;
    }

    private static bool FullHouse(IEnumerable<char> cards, int numberOfJokers = 0)
    {
        var differentCards = new Dictionary<char, int>();

        foreach (var t in cards)
        {
            if (!differentCards.TryAdd(t, 1))
            {
                differentCards[t] += 1;
            }
        }

        return numberOfJokers switch
        {
            0 => differentCards.ContainsValue(3) && differentCards.ContainsValue(2),
            1 => differentCards.Count == 3 && (differentCards.ContainsValue(3) || differentCards.ContainsValue(2)),
            2 => differentCards.Count == 3 && differentCards.ContainsValue(2),
            3 => differentCards.Count == 3 && differentCards.ContainsValue(2),
            _ => false
        };
    }

    private static bool ThreeOfAKind(IEnumerable<char> cards, int numberOfJokers = 0)
    {
        var differentCards = new Dictionary<char, int>();

        foreach (var t in cards)
        {
            if (!differentCards.TryAdd(t, 1))
            {
                differentCards[t] += 1;
            }
        }
        
        if (numberOfJokers > 0)
            differentCards.Remove('J');

        var highestValue = differentCards.MaxBy(dc => dc.Value).Value;

        return highestValue + numberOfJokers == 3;
    }

    private static bool TwoPair(IEnumerable<char> cards, int numberOfJokers = 0)
    {
        var differentCards = new Dictionary<char, int>();

        foreach (var t in cards)
        {
            if (!differentCards.TryAdd(t, 1))
            {
                differentCards[t] += 1;
            }
        }

        return numberOfJokers switch
        {
            0 => differentCards.Count == 3 && differentCards.ContainsValue(2),
            1 => differentCards.Count == 4,
            2 => differentCards.Count == 4,
            _ => false
        };
    }

    private static bool OnePair(IEnumerable<char> cards, int numberOfJokers = 0)
    {
        var differentCards = new Dictionary<char, int>();

        foreach (var t in cards)
        {
            if (!differentCards.TryAdd(t, 1))
            {
                differentCards[t] += 1;
            }
        }

        return numberOfJokers switch
        {
            0 => differentCards.Count == 4,
            1 => differentCards.Count == 5,
            _ => false
        };
    }

    public int GetCardValue(int i)
    {
        return IntCards[i];
    }

    public long GetWinning(int rank)
    {
        return Bid * rank;
    }
}

public enum HandType
{
    FiveOfAKind,
    FourOfAKind,
    FullHouse,
    ThreeOfAKind,
    TwoPair,
    OnePair,
    HighCard,
}