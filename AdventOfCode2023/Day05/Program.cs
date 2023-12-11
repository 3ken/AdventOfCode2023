var lines = File.ReadAllLines("Data.txt");
var seedToLocationData = GetSeedToLocationData();

First();
Second();
return;

void First()
{
    var seedRanges = seedToLocationData.Seeds
        .Select(seedPair => new Range(seedPair, seedPair + 1))
        .ToList();

    var lowestLocationNumber = seedToLocationData.Maps.Aggregate(seedRanges, MapToDestination).Select(d => d.Start).Min();

    Console.WriteLine(lowestLocationNumber);
}

void Second()
{
    var seedRanges = seedToLocationData.Seeds.Chunk(2)
        .Select(seedPair => new Range(seedPair[0], seedPair[0] + seedPair[1] - 1))
        .ToList();

    var lowestLocationNumber = seedToLocationData.Maps.Aggregate(seedRanges, MapToDestination).Select(d => d.Start).Min();

    Console.WriteLine(lowestLocationNumber);
}

List<Range> MapToDestination(List<Range> sourceRanges, Dictionary<Range,Range> maps)
{
    var ranges = new Queue<Range>(sourceRanges);
    var destination = new List<Range>();

    while (ranges.Count != 0)
    {
        var range = ranges.Dequeue();
        
        var src = maps.Keys.FirstOrDefault(src => Intersects(src, range));
        if (src == null)
        {
            destination.Add(range);
        }
        else if (src.Start <= range.Start && range.End <= src.End) {
            var dest = maps[src];
            var shift = dest.Start - src.Start;
            destination.Add(new Range(range.Start + shift, range.End + shift));
        } 
        else if (range.Start < src.Start) {
            ranges.Enqueue(range with { End = src.Start - 1 });
            ranges.Enqueue(new Range(src.Start, range.End));
        } 
        else {
            ranges.Enqueue(new Range(range.Start, src.End));
            ranges.Enqueue(new Range(src.End + 1, range.End));
        }
    }
    return destination;
}

SeedToLocationData GetSeedToLocationData()
{
    var data = new SeedToLocationData
    {
        Seeds = lines.First(l => l.StartsWith("seeds: ", StringComparison.Ordinal)).Split(": ")[1].Split(' ').Select(long.Parse).ToArray()
    };

    for (var i = 0; i < lines.Length; i++)
    {
        if (string.IsNullOrEmpty(lines[i])) continue;
        if (lines[i].Contains(" map:"))
        {
            var maps = new Dictionary<Range, Range>();
            while (!string.IsNullOrEmpty(lines[i + 1]))
            {
                var longs = lines[i + 1].Split(' ');
                maps.Add(
                    new Range(long.Parse(longs[1]), long.Parse(longs[1]) + long.Parse(longs[2])),
                    new Range(long.Parse(longs[0]), long.Parse(longs[0]) + long.Parse(longs[2])));
                i++;
                if (i == lines.Length - 1) break;
            }
            data.Maps.Add(maps);
        }
    }

    return data;
}

bool Intersects(Range r1, Range r2) => r1.Start <= r2.End && r2.Start <= r1.End;

public class SeedToLocationData
{
    public long[] Seeds { get; set; } = Array.Empty<long>();
    public List<Dictionary<Range, Range>> Maps { get; set; } = new();
}

public record Map(Range SourceRange, Range DestinationRange);

public record Range(long Start, long End);