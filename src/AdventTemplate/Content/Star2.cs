using DanielCarey;
using System.Numerics;

namespace DanielCarey.AdventTemplate;

public class Star2
{
    record Data(BigInteger Num1, BigInteger Num2);

    public async ValueTask RunAsync()
    {
        var records = File.ReadAllText("Data2.txt")
            .LoadRecords(fields 
                => new Data(BigInteger.Parse(fields[0]), BigInteger.Parse(fields[1]))
            );



        WriteLine("Done");
    }
}