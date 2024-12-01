using System.Numerics;

using DanielCarey;

namespace Day01;
public class Star2
{
    record Data(BigInteger Num1, BigInteger Num2);

    public async ValueTask RunAsync()
    {
        var records = File.ReadAllText("Data2.txt")
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

        WriteLine(score);
    }
}