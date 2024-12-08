using System.Numerics;

namespace DanielCarey.Day01;

public class Star1(ILogger<Star1> logger, string dataPath = "Data1.txt") : IStar
{
    public string Name { get => "Day01.Star1"; }

    record Data(BigInteger Num1, BigInteger Num2);

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation($"RunAsync");

        var records =
            FileReadAllLines(dataPath)
            .LoadRecords(fields
                => new Data(BigInteger.Parse(fields[0]), BigInteger.Parse(fields[1]))
            );

        var sortedList1 = records.Select(r => r.Num1).OrderBy(x => x).ToList();
        var sortedList2 = records.Select(r => r.Num2).OrderBy(x => x).ToList();

        BigInteger answer = 0;
        for (int index = 0; index < sortedList1.Count; index++)
        {
            answer += BigInteger.Abs(sortedList1[index] - sortedList2[index]);
        }

        WriteLine($"Solution: {answer}");
        return ValueTask.FromResult(answer);
    }
}