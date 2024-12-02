using System.Numerics;

using Microsoft.Extensions.Logging;

namespace DanielCarey.Day01;

public class Star2(ILogger<Star2> logger) : IStar
{
    public string Name { get => "Day01.Star2"; }

    record Data(BigInteger Num1, BigInteger Num2);

    public ValueTask RunAsync()
    {
        logger.LogInformation($"{Name}.RunAsync");

        var records = 
            FileReadAllLines("Data2.txt")
            .LoadRecords(fields 
                => new Data(BigInteger.Parse(fields[0]), BigInteger.Parse(fields[1]))
            );

        var countList = new Dictionary<BigInteger, BigInteger>();

        foreach (var record in records)
        {
            countList[record.Num2] = countList.GetValueOrDefault(record.Num2) + 1;
        }
        
        BigInteger score = 0;
        foreach (var record in records)
        {
            countList.TryGetValue(record.Num1, out BigInteger multiply);
            score += (record.Num1 * multiply);
        }

        WriteLine($"Solution: {score}");
        return ValueTask.CompletedTask;
    }
}