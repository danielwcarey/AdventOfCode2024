using DanielCarey;
using System.Numerics;

namespace Day01;

public class Star1
{
    record Data(BigInteger Num1, BigInteger Num2);

    public async ValueTask RunAsync()
    {
        var records = File.ReadAllText("Data1.txt")
            .LoadRecords(fields
                => new Data(BigInteger.Parse(fields[0]), BigInteger.Parse(fields[1]))
            );

        var sortedList1 = records.Select(r => r.Num1).OrderBy(x => x).ToList();
        var sortedList2 = records.Select(r => r.Num2).OrderBy(x => x).ToList();

        BigInteger counter = 0;
        for (int index = 0; index < sortedList1.Count; index++)
        {
            counter += BigInteger.Abs(sortedList1[index] - sortedList2[index]);
        }

        WriteLine(counter);
    }
}