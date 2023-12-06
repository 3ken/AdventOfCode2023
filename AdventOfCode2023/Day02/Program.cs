First();
Second();
return;

void First()
{
    var dicesInGame = new Dictionary<string, int>
    {
        { "red", 12 },
        { "green", 13 },
        { "blue", 14 },
    };
    var possibleGameIds = new List<int>();
    foreach (var line in File.ReadLines(@"C:\Git\AdventOfCode2023\AdventOfCode2023\Day02\Data.txt"))
    {
        var gameId = int.Parse(line.Split(": ")[0].Split(' ')[1]);
        var gameSets = line.Split(": ")[1].Split("; ");
        var validGameSet = true;
        foreach (var gameSet in gameSets)
        {
            var gameSetDices = gameSet.Trim().Split(", ");

            foreach (var colorDices in gameSetDices)
            {
                var trimmedColorDices = colorDices.Trim();
                var numberOfDices = int.Parse(trimmedColorDices.Split(' ')[0]);
                var diceColor = trimmedColorDices.Split(' ')[1];
                dicesInGame.TryGetValue(diceColor, out var dicesCount);
                if (dicesCount < numberOfDices)
                    validGameSet = false;
            }
        }

        if (validGameSet)
            possibleGameIds.Add(gameId);
    }

    Console.WriteLine(possibleGameIds.Sum());
}

void Second()
{
    var powers = new List<int>();
    foreach (var line in File.ReadLines(@"C:\Git\AdventOfCode2023\AdventOfCode2023\Day02\Data.txt"))
    {
        var requiredDicesInGame = new Dictionary<string, int>
        {
            { "red", 1 },
            { "green", 1 },
            { "blue", 1 },
        };
        var gameSets = line.Split(": ")[1].Split("; ");
        foreach (var gameSet in gameSets)
        {
            var gameSetDices = gameSet.Trim().Split(", ");

            foreach (var colorDices in gameSetDices)
            {
                var trimmedColorDices = colorDices.Trim();
                var numberOfDices = int.Parse(trimmedColorDices.Split(' ')[0]);
                var diceColor = trimmedColorDices.Split(' ')[1];
                requiredDicesInGame.TryGetValue(diceColor, out var dicesCount);
                if (dicesCount < numberOfDices)
                {
                    requiredDicesInGame[diceColor] = numberOfDices;
                }
            }
        }

        powers.Add(requiredDicesInGame["red"] * requiredDicesInGame["blue"] * requiredDicesInGame["green"]);
    }

    Console.WriteLine(powers.Sum());
}