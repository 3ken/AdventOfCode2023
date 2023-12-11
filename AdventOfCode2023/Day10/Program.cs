var lines = File.ReadAllLines("Data.txt");
// var lines = File.ReadAllLines("ExampleData.txt");
var matrix = GetCharMatrix(lines);
var startPosition = FindChar(matrix, 'S');

First();
// Second();
return;

void First()
{
    var stepsAwayFromStart = 1;
    var allPositionPassed = new Dictionary<(int, int), int>();
    var firstPositionsPassed = new List<(int, int)>();
    var secondPositionsPassed = new List<(int, int)>();
    firstPositionsPassed.Add(startPosition);
    secondPositionsPassed.Add(startPosition);
    var possibleNextSteps = GetTwoNextStepsFromStart(startPosition, matrix);
    var firstPosition = possibleNextSteps.Item1;
    var secondPosition = possibleNextSteps.Item2;
    allPositionPassed.Add(firstPosition, stepsAwayFromStart);
    allPositionPassed.Add(secondPosition, stepsAwayFromStart);
    while (true)
    {
        stepsAwayFromStart++;
        if(firstPosition != (0,0)) firstPositionsPassed.Add(firstPosition);
        firstPosition = GetNextStep(firstPosition, matrix, firstPositionsPassed[^2]);
        if (allPositionPassed.ContainsKey(firstPosition))
        {
            firstPosition = (0,0);
        }
        if (firstPosition != (0, 0))
        {
            allPositionPassed.Add(firstPosition, stepsAwayFromStart);
        }
        
        if(secondPosition != (0,0)) secondPositionsPassed.Add(secondPosition);
        secondPosition = GetNextStep(secondPosition, matrix, secondPositionsPassed[^2]);
        if (allPositionPassed.ContainsKey(secondPosition))
        {
            secondPosition = (0,0);
        }
        if (secondPosition != (0, 0))
        {
            allPositionPassed.Add(secondPosition, stepsAwayFromStart);
        }

        if (firstPosition == (0, 0) && secondPosition == (0, 0))
        {
            break;
        }
    }

    var stepsAwayMax = allPositionPassed.Select(p => p.Value).Max();
    
    Console.WriteLine(stepsAwayMax);
}

void Second()
{
    Console.WriteLine();
}

static ((int, int), (int, int)) GetTwoNextStepsFromStart((int, int) startPosition, char[,] matrix)
{
    var toReturn = new List<(int, int)>();
    var topStepExists = TopStepPossible(startPosition, matrix, out var topPosition);
    var rightStepExists = RightStepPossible(startPosition, matrix, out var rightPosition);
    var bottomStepExists = BottomStepPossible(startPosition, matrix, out var bottomPosition);
    var leftStepExists = LeftStepPossible(startPosition, matrix, out var leftPosition);
    
    if(topStepExists) toReturn.Add(topPosition);
    if(rightStepExists) toReturn.Add(rightPosition);
    if(bottomStepExists) toReturn.Add(bottomPosition);
    if(leftStepExists) toReturn.Add(leftPosition);
    return (toReturn[0], toReturn[1]);
}

static (int, int) GetNextStep((int, int) currentPosition, char[,] matrix, (int, int) previousStep)
{
    if (currentPosition == (0,0)) return (0,0);
    var topStepExists = TopStepPossible(currentPosition, matrix, out var topPosition);
    var rightStepExists = RightStepPossible(currentPosition, matrix, out var rightPosition);
    var bottomStepExists = BottomStepPossible(currentPosition, matrix, out var bottomPosition);
    var leftStepExists = LeftStepPossible(currentPosition, matrix, out var leftPosition);

    if (matrix[currentPosition.Item1, currentPosition.Item2] is '-')
    {
        if(rightStepExists && previousStep != rightPosition) return rightPosition;
        if(leftStepExists && previousStep != leftPosition) return leftPosition;
    }
    if (matrix[currentPosition.Item1, currentPosition.Item2] is '|')
    {
        if(topStepExists && previousStep != topPosition) return topPosition;
        if(bottomStepExists && previousStep != bottomPosition) return bottomPosition;
    }
    if (matrix[currentPosition.Item1, currentPosition.Item2] is 'L')
    {
        if(rightStepExists && previousStep != rightPosition) return rightPosition;
        if(topStepExists && previousStep != topPosition) return topPosition;
    }
    if (matrix[currentPosition.Item1, currentPosition.Item2] is 'J')
    {
        if(leftStepExists && previousStep != leftPosition) return leftPosition;
        if(topStepExists && previousStep != topPosition) return topPosition;
    }
    if (matrix[currentPosition.Item1, currentPosition.Item2] is '7')
    {
        if(leftStepExists && previousStep != leftPosition) return leftPosition;
        if(bottomStepExists && previousStep != bottomPosition) return bottomPosition;
    }
    if (matrix[currentPosition.Item1, currentPosition.Item2] is 'F')
    {
        if(rightStepExists && previousStep != rightPosition) return rightPosition;
        if(bottomStepExists && previousStep != bottomPosition) return bottomPosition;
    }
    
    return (0,0);
}

static bool TopStepPossible((int, int) currentPosition, char[,] matrix, out (int, int) outPosition)
{
    outPosition = new ValueTuple<int, int>(0, 0);
    if (currentPosition.Item1 == 0) return false;

    if (matrix[currentPosition.Item1 - 1, currentPosition.Item2] is not ('|' or '7' or 'F')) return false;
    outPosition = new ValueTuple<int, int>(currentPosition.Item1 - 1, currentPosition.Item2);
    return true;
}

static bool RightStepPossible((int, int) currentPosition, char[,] matrix, out (int, int) outPosition)
{
    outPosition = new ValueTuple<int, int>(0, 0);
    if (currentPosition.Item2 == matrix.GetLength(1) - 1) return false;

    if (matrix[currentPosition.Item1, currentPosition.Item2 + 1] is not ('-' or '7' or 'J')) return false;
    outPosition = new ValueTuple<int, int>(currentPosition.Item1, currentPosition.Item2 + 1);
    return true;
}

static bool BottomStepPossible((int, int) currentPosition, char[,] matrix, out (int, int) outPosition)
{
    outPosition = new ValueTuple<int, int>(0, 0);
    if (currentPosition.Item1 == matrix.GetLength(0) - 1) return false;

    if (matrix[currentPosition.Item1 + 1, currentPosition.Item2] is not ('|' or 'L' or 'J')) return false;
    outPosition = new ValueTuple<int, int>(currentPosition.Item1 + 1, currentPosition.Item2);
    return true;
}

static bool LeftStepPossible((int, int) currentPosition, char[,] matrix, out (int, int) outPosition)
{
    outPosition = new ValueTuple<int, int>(0, 0);
    if (currentPosition.Item2 == 0) return false;

    if (matrix[currentPosition.Item1, currentPosition.Item2 - 1] is not ('-' or 'L' or 'F')) return false;
    outPosition = new ValueTuple<int, int>(currentPosition.Item1, currentPosition.Item2 - 1);
    return true;
}

static (int, int) FindChar(char[,] charArray, char targetChar)
{
    for (var y = 0; y < charArray.GetLength(0); y++)
    {
        for (var x = 0; x < charArray.GetLength(1); x++)
        {
            if (charArray[y, x] == targetChar)
            {
                return (y, x);
            }
        }
    }

    return new ValueTuple<int, int>();
}

static char[,] GetCharMatrix(IReadOnlyList<string> stringList)
{
    var rows = stringList.Count;
    var columns = stringList.Max(s => s.Length);

    var charArray = new char[columns, rows];

    for (var y = 0; y < rows; y++)
    {
        var currentString = stringList[y];
        for (var x = 0; x < columns; x++)
        {
            if (x < currentString.Length)
            {
                charArray[y, x] = currentString[x];
            }
            else
            {
                charArray[y, x] = ' ';
            }
        }
    }

    return charArray;
}

void PrintMatrixInConsole(char[,] chars)
{
    var rows = chars.GetLength(0);
    var cols = chars.GetLength(1);
    
    for (var y = 0; y < rows; y++)
    {
        for (var x = 0; x < cols; x++)
        {
            Console.Write(chars[y, x]);
        }
        Console.Write(" " + y);
        Console.WriteLine(); // Move to the next line after printing a row
    }
}

void RemoveAllUnnecessaryCharsInMatrix(char[,] chars, Dictionary<(int, int), int> dictionary, (int, int) valueTuple)
{
    for (var y = 0; y < chars.GetLength(0); y++)
    {
        for (var x = 0; x < chars.GetLength(1); x++)
        {
            if (dictionary.ContainsKey((y,x)) || valueTuple == (y,x)) continue;
            
            chars[y, x] = ' ';
        }
    }
}