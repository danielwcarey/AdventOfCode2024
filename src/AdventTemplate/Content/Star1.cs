using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.DanielCarey.AdventTemplate;

public class Star1(ILogger<Star1> logger) : IStar
{
    public string Name { get => "DanielCarey.AdventTemplate.Star1"; }

    record Data(BigInteger Num1, BigInteger Num2);

    public ValueTask RunAsync()
    {
        logger.LogInformation($"RunAsync");

        // Extract Data
        var records =
            FileReadAllLines("Data1.txt")
            .LoadRecords(fields
                => new Data(BigInteger.Parse(fields[0]), BigInteger.Parse(fields[1]))
            );

        // Process Data


        WriteLine("Done");
        return ValueTask.CompletedTask;
    }
}