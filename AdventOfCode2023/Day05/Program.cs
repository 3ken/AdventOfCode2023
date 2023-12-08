var lines = File.ReadAllLines("Data.txt");
var seedToLocationData = GetSeedToLocationData();

// First();
Second();
return;

void First()
{
    var lowestLocationNumber = long.MaxValue;
    foreach (var seed in seedToLocationData!.Seeds)
    {
        var dest = seed;
        foreach (var map in seedToLocationData.Maps)
        {
            dest = MapToDestination(dest, map);
        }
        if (dest < lowestLocationNumber) lowestLocationNumber = dest;
    }

    Console.WriteLine(lowestLocationNumber);
}

void Second()
{
    long? lowestLocationNumber = null;
    var seedRanges = seedToLocationData.Seeds.Chunk(2)
        .Select(seedPair => new Range(seedPair[0], seedPair[1] - 1))
        .ToList();
    var locationRanges = seedToLocationData.Maps.Last()
        .Select(locationPair => new Range(start: locationPair.DestinationRange.Start, end: locationPair.DestinationRange.End))
        .OrderBy(r => r.Start)
        .ToList();
    for (var i = 0; i < locationRanges.Count; i++)
    {
        for (var location = locationRanges[i].Start; location <= locationRanges[i].End; location++)
        {
            var source = location;
            for (var j = seedToLocationData.Maps.Count - 1; j >= 0; j--)
            {
                source = MapToSource(source, seedToLocationData.Maps[j]);
            }
            Console.WriteLine($"i: {i} location: {location} seed: {source}");
            if (!seedRanges.Any(r => Range.Contains(r, source))) continue;
            lowestLocationNumber = location;
            break;
        }

        if (lowestLocationNumber != null) break;
    }

    Console.WriteLine(lowestLocationNumber);
}

long MapToDestination(long source, List<SourceToDestination> sourceToDestinations)
{
    var x = sourceToDestinations.FirstOrDefault(sd => Range.Contains(sd.SourceRange, source));
    if (x == null) return source;

    return x.DestinationRange.Start + (source - x.SourceRange.Start);
}

long MapToSource(long destination, List<SourceToDestination> destinationToSource)
{
    var x = destinationToSource.FirstOrDefault(sd => Range.Contains(sd.DestinationRange, destination));
    if (x == null) return destination;

    return x.SourceRange.Start + (destination - x.DestinationRange.Start);
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
            var maps = new List<SourceToDestination>();
            while (!string.IsNullOrEmpty(lines[i + 1]))
            {
                var ints = lines[i + 1].Split(' ');
                maps.Add(new SourceToDestination(ints[1], ints[0], ints[2]));
                i++;
                if (i == lines.Length - 1) break;
            }
            data.Maps.Add(maps);
        }
    }

    return data;
}

public class SeedToLocationData
{
    public long[] Seeds { get; set; } = Array.Empty<long>();
    public List<List<SourceToDestination>> Maps { get; set; } = new();
}

public record SourceToDestination
{
    public Range SourceRange { get; }
    public Range DestinationRange { get; }

    public SourceToDestination(string sourceRangeStart, string destinationRangeStart, string range)
    {
        SourceRange = new Range(long.Parse(sourceRangeStart), long.Parse(sourceRangeStart) + long.Parse(range) - 1);
        DestinationRange = new Range(long.Parse(destinationRangeStart), long.Parse(destinationRangeStart) + long.Parse(range) - 1);
    }
}

public readonly struct Range(long start, long end)
{
    public long Start { get; } = start;
    public long End { get; } = end;

    public static bool Contains(Range range, long x)
    {
        return range.Start <= x && range.End >= x;
    }
}