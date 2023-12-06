using System.Text.RegularExpressions;

First();
Second();
return;

void First()
{
    var regex = new Regex(@"\d+");
    var numbersToSum = new List<int>();
    foreach (var line in File.ReadLines(@"C:\Git\AdventOfCode2023\AdventOfCode2023\Day04\Data.txt"))
    {
        var winningNumbers = regex.Matches(line.Split(": ")[1].Split(" | ")[0]);
        var rowNumbers = regex.Matches(line.Split(": ")[1].Split(" | ")[1]);
        var numberOfWinningNumbers = winningNumbers.Count(w => rowNumbers.Any(rn => rn.Value == w.Value));
        var number = 0;
        for (var i = 0; i < numberOfWinningNumbers; i++)
        {
            if (number == 0)
            {
                number = 1;
                continue;
            }
            number *= 2;
        }

        if (numberOfWinningNumbers == 0) number = 0;

        numbersToSum.Add(number);
    }

    Console.WriteLine(numbersToSum.Sum());
}

void Second()
{
    var regex = new Regex(@"\d+");
    var wonCards = File.ReadLines(@"C:\Git\AdventOfCode2023\AdventOfCode2023\Day04\Data.txt")
        .Select(line => int.Parse(regex.Match(line.Split(": ")[0]).Value))
        .ToDictionary(cardNumber => cardNumber, _ => 1);

    foreach (var line in File.ReadLines(@"C:\Git\AdventOfCode2023\AdventOfCode2023\Day04\Data.txt"))
    {
        var cardNumber = int.Parse(regex.Match(line.Split(": ")[0]).Value);
        var quantityOfCard = wonCards[cardNumber];
        var numberOfWinningNumbers = regex.Matches(line.Split(": ")[1].Split(" | ")[0])
            .Count(w => regex
                .Matches(line.Split(": ")[1].Split(" | ")[1])
                .Any(rn => rn.Value == w.Value));
        if (numberOfWinningNumbers == 0) continue;

        for (var x = 1; x <= numberOfWinningNumbers; x++)
        {
            wonCards[cardNumber + x] += quantityOfCard;
        }
    }

    Console.WriteLine(wonCards.Sum(a => a.Value));
}