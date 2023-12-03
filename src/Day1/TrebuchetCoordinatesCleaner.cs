using System.Buffers;
using System.Collections.Frozen;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace Day1;

internal sealed partial class TrebuchetCoordinatesCleaner
{
    private static readonly SearchValues<char> DigitsSearchValues =
        SearchValues.Create("0123456789");

    private static readonly FrozenDictionary<string, char> WordToDigitMapping = new KeyValuePair<string, char>[]
    {
        new("one", '1'),
        new("two", '2'),
        new("three", '3'),
        new("four", '4'),
        new("five", '5'),
        new("six", '6'),
        new("seven", '7'),
        new("eight", '8'),
        new("nine", '9')
    }.ToFrozenDictionary();

    [GeneratedRegex("one|two|three|four|five|six|seven|eight|nine")]
    private static partial Regex DigitWordsRegex();
    
    [GeneratedRegex("one|two|three|four|five|six|seven|eight|nine",
        RegexOptions.RightToLeft)]
    private static partial Regex DigitWordsRightToLeftReadRegex();

    public static int ReadCalibrationValuesFromCoordinates(string trebuchetAlphanumericCoordinates)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(trebuchetAlphanumericCoordinates);

        var trebuchetCoordinatesSpan = trebuchetAlphanumericCoordinates.AsSpan();

        var numericIndexes = GetIndexesOfCalibrationValues(trebuchetCoordinatesSpan);

        var alphaMatches = GetMatchesOfCalibrationValues(trebuchetAlphanumericCoordinates);

        var isNumericValueUsedForFirstCalibrationValueFromLeftSide =
            alphaMatches.FirstMatchFromRightSide is null ||
            (numericIndexes.IndexOfFirstCalibrationValueFromLeftSide != -1 &&
            numericIndexes.IndexOfFirstCalibrationValueFromLeftSide < alphaMatches.FirstMatchFromLeftSide?.Index);

        var isNumericValueUsedForFirstCalibrationValueFromRightSide =
                alphaMatches.FirstMatchFromRightSide is null ||
            (numericIndexes.IndexOfFirstCalibrationValueFromRightSide != -1 &&
            numericIndexes.IndexOfFirstCalibrationValueFromRightSide > alphaMatches.FirstMatchFromRightSide?.Index);

        var firstCalibrationValue = isNumericValueUsedForFirstCalibrationValueFromLeftSide ? trebuchetCoordinatesSpan[numericIndexes.IndexOfFirstCalibrationValueFromLeftSide]
            : 
            MapDigitWordToCharValue(alphaMatches!.FirstMatchFromLeftSide!.Value);

        var secondCalibrationValue = isNumericValueUsedForFirstCalibrationValueFromRightSide ? trebuchetCoordinatesSpan[numericIndexes.IndexOfFirstCalibrationValueFromRightSide] : MapDigitWordToCharValue(alphaMatches!.FirstMatchFromRightSide!.Value);


        Span<char> coordinatesSpan = stackalloc char[2];
        
        coordinatesSpan[0] = firstCalibrationValue;
        coordinatesSpan[1] = secondCalibrationValue;

        return int.Parse(coordinatesSpan);
    }

    [Pure]
    private static (int IndexOfFirstCalibrationValueFromLeftSide, int IndexOfFirstCalibrationValueFromRightSide)
        GetIndexesOfCalibrationValues(ReadOnlySpan<char> trebuchetAlphanumericCoordinates)
            => (trebuchetAlphanumericCoordinates.IndexOfAny(DigitsSearchValues),
            trebuchetAlphanumericCoordinates.LastIndexOfAny(DigitsSearchValues));
    
    [Pure]
    private static (Match? FirstMatchFromLeftSide, Match? FirstMatchFromRightSide) GetMatchesOfCalibrationValues(
        string trebuchetAlphanumericCoordinates)
    {
        var matches = DigitWordsRegex().Matches(trebuchetAlphanumericCoordinates);
        var reverseMatches = DigitWordsRightToLeftReadRegex().Matches(trebuchetAlphanumericCoordinates);

        return (matches.FirstOrDefault(), reverseMatches.FirstOrDefault());
    }

    [Pure]
    private static char MapDigitWordToCharValue(string digitWord)
        => WordToDigitMapping[digitWord];
}