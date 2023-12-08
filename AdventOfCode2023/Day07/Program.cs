var lines = File.ReadAllLines("Data.txt");

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
        IntCards = task == 1
            ? GetIntCards(cards)
            : GetIntCardsWithJoker(cards);
        HandType = GetHandType(cards);
    }

    private static int[] GetIntCards(IEnumerable<char> cards)
    {
        var toReturn = new List<int>();
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
                    toReturn.Add(11);
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
    private static int[] GetIntCardsWithJoker(IEnumerable<char> cards)
    {
        var toReturn = new List<int>();
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
                case 'T':
                    toReturn.Add(10);
                    break;
                case 'J':
                    toReturn.Add(1);
                    break;
                default:
                    toReturn.Add(int.Parse(card.ToString()));
                    break;
            }
        }

        return toReturn.ToArray();
    }

    private static HandType GetHandType(IReadOnlyList<char> cards)
    {
        if (FiveOfAKind(cards)) return HandType.FiveOfAKind;
        if (FourOfAKind(cards)) return HandType.FourOfAKind;
        if (FullHouse(cards)) return HandType.FullHouse;
        if (ThreeOfAKind(cards)) return HandType.ThreeOfAKind;
        if (TwoPair(cards)) return HandType.TwoPair;
        if (OnePair(cards)) return HandType.OnePair;
        return HandType.HighCard;
    }

    private static HandType GetHandTypeWithJoker(IReadOnlyList<char> cards)
    {
        if (FiveOfAKind(cards)) return HandType.FiveOfAKind;
        if (FourOfAKind(cards)) return HandType.FourOfAKind;
        if (FullHouse(cards)) return HandType.FullHouse;
        if (ThreeOfAKind(cards)) return HandType.ThreeOfAKind;
        if (TwoPair(cards)) return HandType.TwoPair;
        if (OnePair(cards)) return HandType.OnePair;
        return HandType.HighCard;
    }

    private static bool FiveOfAKind(IReadOnlyList<char> cards)
    {
        var differentCards = new Dictionary<char, int>();

        foreach (var t in cards)
        {
            if (!differentCards.TryAdd(t, 1))
            {
                differentCards[t] += 1;
            }
        }

        return differentCards.ContainsValue(5);
    }

    private static bool FourOfAKind(IEnumerable<char> cards)
    {
        var differentCards = new Dictionary<char, int>();

        foreach (var t in cards)
        {
            if (!differentCards.TryAdd(t, 1))
            {
                differentCards[t] += 1;
            }
        }

        return differentCards.ContainsValue(4);
    }

    private static bool FullHouse(IEnumerable<char> cards)
    {
        var differentCards = new Dictionary<char, int>();

        foreach (var t in cards)
        {
            if (!differentCards.TryAdd(t, 1))
            {
                differentCards[t] += 1;
            }
        }

        return differentCards.ContainsValue(4);
    }

    private static bool ThreeOfAKind(IEnumerable<char> cards)
    {
        var differentCards = new Dictionary<char, int>();

        foreach (var t in cards)
        {
            if (!differentCards.TryAdd(t, 1))
            {
                differentCards[t] += 1;
            }
        }

        return differentCards.ContainsValue(3);
    }

    private static bool TwoPair(IEnumerable<char> cards)
    {
        var differentCards = new Dictionary<char, int>();

        foreach (var t in cards)
        {
            if (!differentCards.TryAdd(t, 1))
            {
                differentCards[t] += 1;
            }
        }

        return differentCards.Count == 3 && differentCards.ContainsValue(2);
    }

    private static bool OnePair(IEnumerable<char> cards)
    {
        var differentCards = new Dictionary<char, int>();

        foreach (var t in cards)
        {
            if (!differentCards.TryAdd(t, 1))
            {
                differentCards[t] += 1;
            }
        }

        return differentCards.Count == 4;
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