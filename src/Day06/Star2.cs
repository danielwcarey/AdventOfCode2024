using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day06;

public class Star2(ILogger<Star2> logger) : IStar
{
    public string Name { get => "Day06.Star2"; }

    record Data(BigInteger Num1, BigInteger Num2);

    public ValueTask RunAsync()
    {
        logger.LogInformation($"RunAsync");

        // Extract Data
        var records =
            FileReadAllLines("Data2.txt")
            .LoadRecords(fields
                => new Data(BigInteger.Parse(fields[0]), BigInteger.Parse(fields[1]))
            );

        // Process Data


        WriteLine("Done");
        return ValueTask.CompletedTask;
    }
}