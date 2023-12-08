using System.Text.RegularExpressions;

var lines = File.ReadAllLines("Data.txt");
var regex = new Regex(@"\d+");
var races = GetRacesData();

First();
Second();
return;

void First()
{
    var totalWaysOfWinning = new List<int>();
    foreach (var race in races)
    {
        var waysOfWinningRace = 0;
        for (var i = 0; i < race.Duration - 1; i++)
        {
            var distance = GetDistance(GetSpeed(i), race.Duration - i);
            if (distance > race.Record)
                waysOfWinningRace++;
        }

        totalWaysOfWinning.Add(waysOfWinningRace);
    }

    Console.WriteLine(totalWaysOfWinning.Aggregate(1, (current, t) => current * t));
}

void Second()
{
    var totalWaysOfWinning = new List<int>();

    var waysOfWinningRace = 0;
    var race = GetRacesData2();
    for (var i = 0; i < race.Duration - 1; i++)
    {
        var distance = GetDistance(GetSpeed(i), race.Duration - i);
        if (distance > race.Record)
            waysOfWinningRace++;
    }

    totalWaysOfWinning.Add(waysOfWinningRace);

    Console.WriteLine(totalWaysOfWinning.Aggregate(1, (current, t) => current * t));
}

long GetSpeed(long milliseconds)
{
    return 1 * milliseconds;
}

long GetDistance(long speed, long milliseconds)
{
    return speed * milliseconds;
}

Race GetRacesData2()
{
    var time = regex.Match(lines[0].Split(':')[1].Trim().Replace(" ", ""));
    var record = regex.Match(lines[1].Split(':')[1].Trim().Replace(" ", ""));
    return new Race(time.Value, record.Value);
}

List<Race> GetRacesData()
{
    var times = regex.Matches(lines[0]);
    var records = regex.Matches(lines[1]);
    var racesToReturn = new List<Race>();
    for (var i = 0; i < times.Count; i++)
    {
        racesToReturn.Add(new Race(times[i].Value, records[i].Value));
    }

    return racesToReturn;
}

public readonly struct Race(long duration, long record)
{
    public long Duration { get; } = duration;
    public long Record { get; } = record;

    public Race(string duration, string record) : this(long.Parse(duration), long.Parse(record))
    {
    }
}