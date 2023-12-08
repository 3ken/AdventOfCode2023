var lines = File.ReadAllLines("Data.txt");
// var lines = File.ReadAllLines("ExampleData.txt");
var data = GetData();

First();
Second();
return;

void First()
{
    var steps = 0;
    var instructionPosition = 0;
    var key = "AAA";
    while (key != "ZZZ")
    {
        key = data.Instructions[instructionPosition] == 'R' 
            ? data.PositionNodes[key].Item2 
            : data.PositionNodes[key].Item1;
        steps++;
        instructionPosition++;
        if (instructionPosition > data.Instructions.Length - 1) instructionPosition = 0;
    }
    
    Console.WriteLine(steps);
}

void Second()
{
    Console.WriteLine();
}

Data GetData()
{
    var instructions = lines[0].ToCharArray();
    var positionNodes = new Dictionary<string, (string, string)>();

    for (var i = 2; i < lines.Length; i++)
    {
        var key = lines[i].Split(" = ")[0];
        var leftValue = lines[i].Split(" = ")[1].Split(", ")[0].Remove(0, 1);
        var rightValue = lines[i].Split(" = ")[1].Split(", ")[1].Remove(3, 1);
        positionNodes.Add(key, new ValueTuple<string, string>(leftValue, rightValue));
    }

    return new Data(instructions, positionNodes);
}

public class Data()
{
    public char[] Instructions { get; } = Array.Empty<char>();
    public Dictionary<string, (string, string)> PositionNodes { get; } = new();

    public Data(char[] instructions,Dictionary<string, (string, string)> positionNodes) : this()
    {
        Instructions = instructions;
        PositionNodes = positionNodes;
    }
}