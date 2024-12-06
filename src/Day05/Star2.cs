using System.Numerics;

//using UpdateList = System.Collections.Generic.List<System.Numerics.BigInteger>; // List<BigInteger>

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day05;

public class Star2(ILogger<Star2> logger) : IStar
{
    public string Name { get => "Day05.Star2"; }

    record Rule(BigInteger Num1, BigInteger Num2);

    public ValueTask RunAsync()
    {
        logger.LogInformation($"RunAsync");

        // Extract Data
        var allLines = File
            .ReadAllLines("Data2.txt")
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
            .Where(update => !IsCorrect(update, rules)) // filter incorrect lines
            .Select(update => FixUpdateList(update, rules))
            .Where(update => update.Count > 0)
            .Select(update => update[update.Count / 2])
            .ToList();

        BigInteger answer = 0;
        foreach (var number in middleNumberList)
        {
            answer = BigInteger.Add(answer, number);
        }

        // 6142
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

    private bool IsCorrect(List<BigInteger> update, List<Rule> rules)
    {
        for (var index = 0; index < update.Count; index++)
        {
            // given an item in the update list
            var item = update[index];

            // get numbers that cannot be to the left of item
            var cannotBeLeft = rules
                .Where(rule => rule.Num1 == item)
                .Select(rule => rule.Num2);

            // if there are any numbers, that is an error
            var cannotBeLeftError = update[..index].Intersect(cannotBeLeft).Any();

            // get numbers that cannot be to the right of item
            var cannotBeRight = rules
                .Where(rule => rule.Num2 == item)
                .Select(rule => rule.Num1);

            // if there are any numbers, that is an error
            var cannotBeRightError = update[index..].Intersect(cannotBeRight).Any();

            if (cannotBeLeftError || cannotBeRightError) return false;
        }
        return true;
    }

    private List<BigInteger> FixUpdateList(List<BigInteger> updateList, List<Rule> rules)
    {
        var orderedList = new System.Collections.Generic.SortedSet<BigInteger>(new BigIntegerComparer(rules));

        foreach (var item in updateList)
        {
            orderedList.Add(item);
        }

        return orderedList.ToList();
    }

    class BigIntegerComparer(List<Rule> rules) : IComparer<BigInteger>
    {
        public int Compare(BigInteger num1, BigInteger num2)
        {
            if (rules.Contains(new Rule(num1, num2))) return -1;
            if (rules.Contains(new Rule(num2, num1))) return 1;

            return 0;
        }
    }

}
