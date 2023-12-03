
var matrix = CreateCharMatrix();
var rows = matrix.GetLength(0) - 1;
var cols = matrix.GetLength(1) - 1;
First();
Second();
return;

void First()
{
    var numbersToAdd = new List<int>();
    for (var rowIndex = 0; rowIndex <= rows; rowIndex++)
    {
        for (var columnIndex = 0; columnIndex <= cols; columnIndex++)
        {
            var cell = matrix[rowIndex, columnIndex];
            if (!char.IsDigit(cell)) continue;
            var adjacentSymbolExists = CheckIfAdjacentSymbolExists(rowIndex, columnIndex);
            var wholeNumber = "" + cell;
            var numberLenght = 1;
            var isANumber = true;
            while (isANumber)
            {
                if (columnIndex + numberLenght > cols)
                {
                    isANumber = false;
                    continue;
                }
                var nextCell = matrix[rowIndex, columnIndex + numberLenght];
                if (char.IsDigit(nextCell))
                {
                    wholeNumber += nextCell;
                    numberLenght++;
                    if (!adjacentSymbolExists)
                    {
                        adjacentSymbolExists = CheckIfAdjacentSymbolExists(rowIndex, columnIndex + numberLenght - 1);
                    }
                }
                else
                {
                    isANumber = false;
                }
            }

            if (!adjacentSymbolExists) continue;
            numbersToAdd.Add(int.Parse(wholeNumber));
            columnIndex += wholeNumber.Length - 1;
        }
    }
    
    Console.WriteLine(numbersToAdd.Sum());
}

void Second()
{
    var numbersToAdd = new List<int>();
    for (var rowIndex = 0; rowIndex <= rows; rowIndex++)
    {
        for (var columnIndex = 0; columnIndex <= cols; columnIndex++)
        {
            var cell = matrix[rowIndex, columnIndex];
            if (cell != '*') continue;
            numbersToAdd.Add(GetGearRatio(rowIndex, columnIndex));
        }
    }
    
    Console.WriteLine(numbersToAdd.Sum());
}

int GetGearRatio(int rowIndex, int columnIndex)
{
    var numbersAbove = GetNumbersTop(rowIndex, columnIndex);
    var numbersBottom = GetNumbersBottom(rowIndex, columnIndex);
    var numberRight = GetNumberRight(rowIndex, columnIndex);
    var numberLeft = GetNumberLeft(rowIndex, columnIndex);

    if (numbersAbove.Length + numbersBottom.Length + numberRight.Length + numberLeft.Length != 2) return 0;
    var numbers = new List<int>();
    numbers.AddRange(numbersAbove);
    numbers.AddRange(numbersBottom);
    numbers.AddRange(numberRight);
    numbers.AddRange(numberLeft);
    return numbers[0] * numbers[1];
}

char[,] CreateCharMatrix()
{
    var listOfStrings = File.ReadAllLines(@"C:\Git\AdventOfCode2023\AdventOfCode2023\Day03\Data.txt");
    var numberOfRows = listOfStrings.Length;
    var numberOfCols = listOfStrings.Max(s => s.Length);
    var charMatrix = new char[numberOfRows, numberOfCols];
    for (var i = 0; i < numberOfRows; i++)
    {
        var currentString = listOfStrings[i];
        for (var j = 0; j < currentString.Length; j++)
        {
            charMatrix[i, j] = currentString[j];
        }
    }

    return charMatrix;
}

bool CheckIfAdjacentSymbolExists(int rowIndex, int columnIndex)
{
    return GotAdjacentSymbolRight(rowIndex, columnIndex) 
           || GotAdjacentSymbolLeft(rowIndex, columnIndex) 
           || GotAdjacentSymbolTop(rowIndex, columnIndex) 
           || GotAdjacentSymbolBottom(rowIndex, columnIndex) ;
}

bool GotAdjacentSymbolLeft(int rowIndex, int columnIndex)
{
    {
        if (columnIndex <= 0) return false;
        if (rowIndex > 0)
        {
            if (IsAdjacentSymbol(rowIndex - 1, columnIndex - 1))
            {
                return true;
            }

            if (IsAdjacentSymbol(rowIndex, columnIndex - 1))
            {
                return true;
            }
        }

        if (rowIndex < rows)
        {
            if (IsAdjacentSymbol(rowIndex + 1, columnIndex - 1))
            {
                return true;
            }
        }

        return false;
    }
}

bool GotAdjacentSymbolRight(int rowIndex, int columnIndex)
{
    {
        if (columnIndex >= cols) return false;
        if (rowIndex > 0)
        {
            if (IsAdjacentSymbol(rowIndex - 1, columnIndex + 1))
            {
                return true;
            }
            if (IsAdjacentSymbol(rowIndex, columnIndex + 1))
            {
                return true;
            }
        }

        if (rowIndex < rows)
        {
            if (IsAdjacentSymbol(rowIndex + 1, columnIndex + 1))
            {
                return true;
            }
        }

        return false;
    }
}

bool GotAdjacentSymbolTop(int rowIndex, int columnIndex)
{
    {
        return rowIndex > 0 && IsAdjacentSymbol(rowIndex -1, columnIndex);
    }
}

bool GotAdjacentSymbolBottom(int rowIndex, int columnIndex)
{
    return rowIndex < rows && IsAdjacentSymbol(rowIndex + 1, columnIndex);
}

bool IsAdjacentSymbol(int rowIndex, int columnIndex)
{
    return !char.IsDigit(matrix[rowIndex, columnIndex]) && matrix[rowIndex, columnIndex] != '.';
}

int[] GetNumbersTop(int rowIndex, int columnIndex)
{
    if ((TopLeftIsANumber(rowIndex, columnIndex) && 
        TopIsANumber(rowIndex, columnIndex) && 
        TopRightIsANumber(rowIndex, columnIndex)) || 
        (TopLeftIsANumber(rowIndex, columnIndex) &&  
         !TopIsANumber(rowIndex, columnIndex) && 
         !TopRightIsANumber(rowIndex, columnIndex)) || 
        (TopLeftIsANumber(rowIndex, columnIndex) &&  
         TopIsANumber(rowIndex, columnIndex) && 
         !TopRightIsANumber(rowIndex, columnIndex)))
    {
        return new[] { int.Parse(GetWholeNumber(rowIndex - 1, columnIndex - 1)) };
    }
    
    if ((!TopLeftIsANumber(rowIndex, columnIndex) && 
         TopIsANumber(rowIndex, columnIndex) && 
         TopRightIsANumber(rowIndex, columnIndex)) || 
        (!TopLeftIsANumber(rowIndex, columnIndex) &&  
         TopIsANumber(rowIndex, columnIndex) && 
         !TopRightIsANumber(rowIndex, columnIndex)))
    {
        return new[] { int.Parse(GetWholeNumber(rowIndex - 1, columnIndex)) };
    }
    
    if (TopLeftIsANumber(rowIndex, columnIndex) && 
        !TopIsANumber(rowIndex, columnIndex) && 
        TopRightIsANumber(rowIndex, columnIndex))
    {
        return new[] { 
            int.Parse(GetWholeNumber(rowIndex - 1, columnIndex - 1)), 
            int.Parse(GetWholeNumber(rowIndex - 1, columnIndex + 1)) };
    }
    
    if (!TopLeftIsANumber(rowIndex, columnIndex) && 
        !TopIsANumber(rowIndex, columnIndex) && 
        TopRightIsANumber(rowIndex, columnIndex))
    {
        return new[] { int.Parse(GetWholeNumber(rowIndex - 1, columnIndex + 1)) };
    }

    return new int[] {  };
}

int[] GetNumbersBottom(int rowIndex, int columnIndex)
{
    if ((BottomLeftIsANumber(rowIndex, columnIndex) && 
         BottomIsANumber(rowIndex, columnIndex) && 
         BottomRightIsANumber(rowIndex, columnIndex)) || 
        (BottomLeftIsANumber(rowIndex, columnIndex) &&  
         !BottomIsANumber(rowIndex, columnIndex) && 
         !BottomRightIsANumber(rowIndex, columnIndex)) || 
        (BottomLeftIsANumber(rowIndex, columnIndex) &&  
         BottomIsANumber(rowIndex, columnIndex) && 
         !BottomRightIsANumber(rowIndex, columnIndex)))
    {
        return new[] { int.Parse(GetWholeNumber(rowIndex + 1, columnIndex - 1)) };
    }
    
    if ((!BottomLeftIsANumber(rowIndex, columnIndex) && 
         BottomIsANumber(rowIndex, columnIndex) && 
         BottomRightIsANumber(rowIndex, columnIndex)) || 
        (!BottomLeftIsANumber(rowIndex, columnIndex) &&  
         BottomIsANumber(rowIndex, columnIndex) && 
         !BottomRightIsANumber(rowIndex, columnIndex)))
    {
        return new[] { int.Parse(GetWholeNumber(rowIndex + 1, columnIndex)) };
    }
    
    if (BottomLeftIsANumber(rowIndex, columnIndex) && 
        !BottomIsANumber(rowIndex, columnIndex) && 
        BottomRightIsANumber(rowIndex, columnIndex))
    {
        return new[] { 
            int.Parse(GetWholeNumber(rowIndex + 1, columnIndex - 1)), 
            int.Parse(GetWholeNumber(rowIndex + 1, columnIndex + 1)) };
    }
    
    if (!BottomLeftIsANumber(rowIndex, columnIndex) && 
        !BottomIsANumber(rowIndex, columnIndex) && 
        BottomRightIsANumber(rowIndex, columnIndex))
    {
        return new[] { int.Parse(GetWholeNumber(rowIndex + 1, columnIndex + 1)) };
    }
    
    return new int[] {  };
}

int[] GetNumberRight(int rowIndex, int columnIndex)
{
    return RightIsANumber(rowIndex, columnIndex) 
        ? new[] { int.Parse(GetWholeNumber(rowIndex, columnIndex + 1)) } 
        : new int[] {  };
}

int[] GetNumberLeft(int rowIndex, int columnIndex)
{
    return LeftIsANumber(rowIndex, columnIndex) 
        ? new[] { int.Parse(GetWholeNumber(rowIndex, columnIndex - 1)) } 
        : Array.Empty<int>();
}

string GetWholeNumber(int rowIndex, int columnIndex)
{
    var isANumber = true;
    var length = 1;
    var startIndex = 0;
    var endIndex = 0;
    var numberToReturn = "";
    while (isANumber)
    {
        if (columnIndex - length < 0)
        {
            startIndex = columnIndex - (length - 1);
            isANumber = false;
            length = 1;
            continue;
        }
        if (char.IsDigit(matrix[rowIndex, columnIndex - length]))
        {
            length++;
        }
        else
        {
            startIndex = columnIndex - (length - 1);
            isANumber = false;
            length = 1;
        }
    }

    isANumber = true;
    while (isANumber)
    {
        if (startIndex + length > cols)
        {
            endIndex = startIndex + length;
            isANumber = false;
            continue;
        }
        if (char.IsDigit(matrix[rowIndex, startIndex + length]))
        {
            length++;
        }
        else
        {
            endIndex = startIndex + length;
            isANumber = false;
        }
    }

    for (var i = startIndex; i < endIndex; i++)
    {
        numberToReturn += matrix[rowIndex, i];
    }
    
    return numberToReturn;
}

bool TopLeftIsANumber(int rowIndex, int columnIndex)
{
    if (rowIndex > 0 && columnIndex > 0)
    {
        return char.IsDigit(matrix[rowIndex - 1, columnIndex - 1]);
    }

    return false;
}

bool TopIsANumber(int rowIndex, int columnIndex)
{
    return rowIndex > 0 && char.IsDigit(matrix[rowIndex - 1, columnIndex]);
}

bool TopRightIsANumber(int rowIndex, int columnIndex)
{
    if (rowIndex > 0 && columnIndex < cols)
    {
        return char.IsDigit(matrix[rowIndex - 1, columnIndex + 1]);
    }

    return false;
}

bool RightIsANumber(int rowIndex, int columnIndex)
{
    return columnIndex < cols && char.IsDigit(matrix[rowIndex, columnIndex + 1]);
}

bool BottomRightIsANumber(int rowIndex, int columnIndex)
{
    if (rowIndex < rows && columnIndex < cols)
    {
        return char.IsDigit(matrix[rowIndex + 1, columnIndex + 1]);
    }

    return false;
}

bool BottomIsANumber(int rowIndex, int columnIndex)
{
    return rowIndex < rows && char.IsDigit(matrix[rowIndex + 1, columnIndex]);
}

bool BottomLeftIsANumber(int rowIndex, int columnIndex)
{
    if (rowIndex < rows && columnIndex > 0)
    {
        return char.IsDigit(matrix[rowIndex + 1, columnIndex - 1]);
    }

    return false;
}

bool LeftIsANumber(int rowIndex, int columnIndex)
{
    return columnIndex > 0 && char.IsDigit(matrix[rowIndex, columnIndex - 1]);
}