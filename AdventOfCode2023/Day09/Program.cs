using System.Text.RegularExpressions;

var lines = File.ReadAllLines("Data.txt");
var regex = new Regex(@"-?\d+");
// var lines = File.ReadAllLines("ExampleData.txt");

First();
Second();
return;

void First()
{
    var resultNumberLines = new List<List<int>>();
    var numberLines = lines.Select(l => regex.Matches(l)).ToList();
    foreach (var t in numberLines)
    {
        var rowNumbers = t.Select(nl => int.Parse(nl.Value)).ToList();
        resultNumberLines.Add(rowNumbers);
        var nextRowNumbers = new List<int>();
        while (true)
        {
            for (var j = 1; j < rowNumbers.Count; j++)
            {
                nextRowNumbers.Add(rowNumbers[j] - rowNumbers[j - 1]);
            }
            resultNumberLines.Add(nextRowNumbers);
            rowNumbers = nextRowNumbers;
            if (rowNumbers.All(n => n == 0))
            {
                break;
            }
            nextRowNumbers = [];
        }
    }
    
    var previousRowNumbers = new List<int>();
    var intsToSum = new List<int>();
    for (var i = resultNumberLines.Count - 1; i >= 0 ; i--)
    {
        var rowNumbers = resultNumberLines[i];
        if (rowNumbers.All(rn => rn == 0))
        {
            rowNumbers.Add(0);
            previousRowNumbers = rowNumbers;
        }
        else
        {
            var number = rowNumbers.Last() + previousRowNumbers.Last();
            rowNumbers.Add(number);
            previousRowNumbers = rowNumbers;
            if (i == 0 || resultNumberLines[i-1].All(n => n == 0)) intsToSum.Add(number);
        }
    }
    Console.WriteLine(intsToSum.Sum());
}

void Second()
{
    var resultNumberLines = new List<LinkedList<int>>();
    var numberLines = lines.Select(l => regex.Matches(l)).ToList();
    foreach (var t in numberLines)
    {
        var rowNumbers = t.Select(nl => int.Parse(nl.Value)).ToList();
        resultNumberLines.Add(new LinkedList<int>(rowNumbers));
        var nextRowNumbers = new List<int>();
        while (true)
        {
            for (var j = 1; j < rowNumbers.Count; j++)
            {
                nextRowNumbers.Add(rowNumbers[j] - rowNumbers[j - 1]);
            }
            resultNumberLines.Add(new LinkedList<int>(nextRowNumbers));
            rowNumbers = nextRowNumbers;
            if (rowNumbers.All(n => n == 0))
            {
                break;
            }
            nextRowNumbers = [];
        }
    }
    
    var previousRowNumbers = new LinkedList<int>();
    var intsToSum = new List<int>();
    for (var i = resultNumberLines.Count - 1; i >= 0 ; i--)
    {
        var rowNumbers = resultNumberLines[i];
        if (rowNumbers.All(rn => rn == 0))
        {
            rowNumbers.AddFirst(0);
            previousRowNumbers = rowNumbers;
        }
        else
        {
            var number = rowNumbers.First() - previousRowNumbers.First();
            rowNumbers.AddFirst(number);
            previousRowNumbers = rowNumbers;
            if (i == 0 || resultNumberLines[i-1].All(n => n == 0)) intsToSum.Add(number);
        }
    }
    Console.WriteLine(intsToSum.Sum());
}