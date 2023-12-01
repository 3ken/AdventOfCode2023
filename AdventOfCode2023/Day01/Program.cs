using System.Text.RegularExpressions;

var numbers = new List<Number> { Number.One, Number.Two, Number.Three, Number.Four, Number.Five, Number.Six, Number.Seven, Number.Eight, Number.Nine };
var first = First();
var second = Second();

Console.WriteLine(first);
Console.WriteLine(second);
Console.ReadLine();
return;

int First()
{
    var regex = new Regex(@"\d");
    var ints = new List<int>();

    foreach (var line in File.ReadLines(@"C:\Git\AdventOfCode2023\AdventOfCode2023\Day01\Data.txt"))
    {
        if (string.IsNullOrEmpty(line)) continue;
        var matches = regex.Matches(line);
        ints.Add(int.Parse(matches.First().Value + matches.Last().Value));
    }

    return ints.Sum();
}

int Second()
{
    var regex = new Regex(@"\d");
    var ints = new List<int>();

    foreach (var line in File.ReadLines(@"C:\Git\AdventOfCode2023\AdventOfCode2023\Day01\Data.txt"))
    {
        if (string.IsNullOrEmpty(line)) continue;
        var firstInt = "";
        var lastInt = "";
        var regexmatches = regex.Matches(line);
        var stringMatches = GetIndexOfStringNumber(line);

        if (regexmatches.First().Index < stringMatches.First().Item1)
            firstInt = regexmatches.First().Value;
        else
            firstInt = stringMatches.First().Item2.ToString();

        if (regexmatches.Last().Index > stringMatches.Last().Item1)
            lastInt = regexmatches.Last().Value;
        else
            lastInt = stringMatches.Last().Item2.ToString();

        ints.Add(int.Parse(firstInt + lastInt));
    }

    return ints.Sum();
}

List<(int?, int?)> GetIndexOfStringNumber(string line)
{
    var lowestIndex = int.MaxValue;
    int lowestIndexValue = default;
    var highestIndex = int.MinValue;
    int highestIndexValue = default;
    foreach (var number in numbers)
    {
        var firstIndex = line.IndexOf(number.ToString(), StringComparison.InvariantCultureIgnoreCase);
        var lastIndex = line.LastIndexOf(number.ToString(), StringComparison.InvariantCultureIgnoreCase);

        if (firstIndex != -1 && firstIndex < lowestIndex)
        {
            lowestIndex = firstIndex;
            lowestIndexValue = (int)number;
        }
        if (lastIndex != -1 && lastIndex > highestIndex)
        {
            highestIndex = lastIndex;
            highestIndexValue = (int)number;
        }
    }

    return new List<(int?, int?)> { (lowestIndex, lowestIndexValue), (highestIndex, highestIndexValue) };
}

internal enum Number
{
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9
}