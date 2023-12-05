var lines = File.ReadAllLines(@"C:\Git\AdventOfCode2023\AdventOfCode2023\Day05\Data.txt");
var seedToLocationData = GetSeedToLocationData();

First();
Second();
return;

void First()
{
    var lowestLocationNumber = long.MaxValue;
    foreach (var seed in seedToLocationData.Seeds)
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
   
}

long MapToDestination(long source, List<SourceToDestination> sourceToDestinations)
{
    var x = sourceToDestinations.FirstOrDefault(sd => sd.SourceRangeStart <= source && sd.SourceRangeEnd >= source);
    if (x == null) return source;

    return x.DestinationRangeStart + (source - x.SourceRangeStart);
}

SeedToLocationData GetSeedToLocationData()
{
    var data = new SeedToLocationData
    {
        Seeds = lines.First(l => l.StartsWith("seeds: ")).Split(": ")[1].Split(' ').Select(long.Parse).ToArray()
    };

    for (var i = 0; i < lines.Length; i++)
    {
        if(string.IsNullOrEmpty(lines[i])) continue;
        if (lines[i].Contains(" map:"))
        {
            var maps = new List<SourceToDestination>();
            while (!string.IsNullOrEmpty(lines[i + 1]))
            {
                var ints = lines[i + 1].Split(' ');
                maps.Add(new SourceToDestination(ints[1],ints[0],ints[2]));
                i++;
                if(i == lines.Length - 1) break;
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
    public long SourceRangeStart { get; }
    public long SourceRangeEnd { get; }
    public long DestinationRangeStart { get; }
    public long Range { get; }

    public SourceToDestination(string sourceRangeStart, string destinationRangeStart, string range)
    {
        SourceRangeStart = long.Parse(sourceRangeStart);
        SourceRangeEnd = long.Parse(sourceRangeStart) + long.Parse(range) - 1;
        DestinationRangeStart = long.Parse(destinationRangeStart);
        Range = long.Parse(range);
    }
}