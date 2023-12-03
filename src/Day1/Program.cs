// See https://aka.ms/new-console-template for more information

using Day1;

var calibrationValuesSum = 0L;

await foreach (var alphanumericCoordinates in ReadCoordinatesFromFile())
{
    calibrationValuesSum += TrebuchetCoordinatesCleaner.ReadCalibrationValuesFromCoordinates(alphanumericCoordinates);
}

Console.WriteLine($"Sum of all calibration values is: {calibrationValuesSum}");

static IAsyncEnumerable<string> ReadCoordinatesFromFile()
{
    const string coordinatesFileName = "Coordinates.txt";

    return File.ReadLinesAsync(coordinatesFileName);
}