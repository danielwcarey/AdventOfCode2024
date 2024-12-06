using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day05;

public class Star2(ILogger<Star2> logger) : IStar
{
    public string Name { get => "Day05.Star2"; }

    record Rule(BigInteger Num1, BigInteger Num2);

    public ValueTask RunAsync()
    {
        throw new NotImplementedException();

        logger.LogInformation($"RunAsync");

        // Extract Data
        var allLines = File
            .ReadAllLines("Data1.txt")
            .ToList();

        var rules = allLines
            .TakeWhile(line => line.Length > 0) // take all lines up to the empty line
            .Select(ParseRule) // read the rule section
            .ToList();

        var updates = allLines
            .SkipWhile(line => line.Length > 0) // skip all non-empty lines
            .Skip(1) // skip the empty line
            .Select(ParseUpdate) // read the update section
            .ToList();

        // Process Data
        var middleNumberList = updates
            .Select(update => CorrectedUpdate(update, rules)) // filter correct lines
            .Select(update => update[update.Count / 2])
            .ToList();

        BigInteger answer = 0;
        foreach (var number in middleNumberList)
        {
            answer = BigInteger.Add(answer, number);
        }

        WriteLine($"Answer: {answer}");
        return ValueTask.CompletedTask;
    }
    
    private Rule ParseRule(string line)
    {
        var parts = line.Split("|");
        return new Rule(
            BigInteger.Parse(parts[0]),
            BigInteger.Parse(parts[1])
        );
    }

    private List<BigInteger> ParseUpdate(string line)
    {
        return line
            .Split(",")
            .Select(BigInteger.Parse)
            .ToList();
    }

    private List<BigInteger> CorrectedUpdate(List<BigInteger> update, List<Rule> rules)
    {
        List<int> errorIndex = new();
        
        for (var index = 0; index < update.Count; index++)
        {
            // given an item in the update list
            var item = update[index];
            var cannotBeLeft = rules
                .Where(rule => rule.Num1 == item)
                .Select(rule => rule.Num2);

            var cannotBeRight = rules
                .Where(rule => rule.Num2 == item)
                .Select(rule => rule.Num1);

            var isError =
                update[..index].Intersect(cannotBeLeft).Any()
                || update[index..].Intersect(cannotBeRight).Any();

            if (isError) errorIndex.Add(index);
        }

        if(errorIndex.Count == 0) return [];

        // TBD - permutations based on index of errors ?
        return [];
    }
}