using FluentAssertions;

namespace Day1.Tests.Unit;

public sealed class TrebuchetCoordinatesCleanerTests
{
    [Theory]
    [InlineData("1abc2", 12)]
    [InlineData("pqr3stu8vwx", 38)]
    [InlineData("a1b2c3d4e5f", 15)]
    [InlineData("treb7uchet", 77)]
    [InlineData("two1nine", 29)]
    [InlineData("eightwothree", 83)]
    [InlineData("abcone2threexyz", 13)]
    [InlineData("xtwone3four", 24)]
    [InlineData("4nineeightseven2", 42)]
    [InlineData("zoneight234", 14)]
    [InlineData("7pqrstsixteen", 76)]
    [InlineData("threefour86", 36)]
    [InlineData("dchrpzkfpgqzgjmpdcthreeeightsix82five", 35)]
    [InlineData("twone", 21)]
    public void
        ReadCalibrationValuesFromCoordinates_ReturnsFirstDigitFromLeftSideAndFirstDigitFromRightSide_WhenAlphaNumericInputIsProvided(
            string input, int expectedOutput)
    {
        var actualOutput = TrebuchetCoordinatesCleaner.ReadCalibrationValuesFromCoordinates(input);

        actualOutput
            .Should()
            .Be(expectedOutput);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ReadCalibrationValuesFromCoordinates_ThrowsArgumentException_WhenNullOrEmptyOrWhiteSpaceInputIsProvided(
        string invalidInput)
    {
        var readTrebuchetCoordinatesAction = () => TrebuchetCoordinatesCleaner.ReadCalibrationValuesFromCoordinates(invalidInput);

        readTrebuchetCoordinatesAction
            .Should()
            .Throw<ArgumentException>();
    }
}