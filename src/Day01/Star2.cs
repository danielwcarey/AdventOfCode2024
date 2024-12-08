using System.Numerics;

namespace DanielCarey.Day01;

public class Star2(ILogger<Star2> logger, string dataPath = "Data2.txt") : IStar
{
    public string Name { get => "Day01.Star2"; }

    record Data(BigInteger Num1, BigInteger Num2);

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation($"RunAsync");

        var records = 
            FileReadAllLines(dataPath)
            .LoadRecords(fields 
                => new Data(BigInteger.Parse(fields[0]), BigInteger.Parse(fields[1]))
            );

        var countList = new Dictionary<BigInteger, BigInteger>();

        foreach (var record in records)
        {
            countList[record.Num2] = countList.GetValueOrDefault(record.Num2) + 1;
        }
        
        BigInteger answer = 0;
        foreach (var record in records)
        {
            countList.TryGetValue(record.Num1, out BigInteger multiply);
            answer += (record.Num1 * multiply);
        }

        WriteLine($"Solution: {answer}");
        return ValueTask.FromResult(answer);
    }
}