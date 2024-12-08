using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day07;

public class Star2(ILogger<Star2> logger) : IStar
{
    public string Name { get => "Day07.Star2"; }

    public ValueTask RunAsync()
    {
        logger.LogInformation($"RunAsync");

        // Extract Data
        var equations = File
            .ReadLines("Data2.txt")
            .Select(line => line.Split(":", StringSplitOptions.TrimEntries))
            .Select(item => new Equation(
                    Value: BigInteger.Parse(item[0]),
                    Numbers: item[1].Split(" ").Select(BigInteger.Parse).ToList()
                )
            ).ToList();

        // Process Data
        BigInteger answer = 0;

        foreach (Equation equation in equations)
        {
            Tree tree = new(equation);

            var isValid = tree
                .Nodes
                .Any(node => node.HaveChildren == false
                             && equation.Value == node.Value);

            if (isValid) answer += equation.Value;
        }

        // 354060705047464
        WriteLine($"Answer:{answer}");
        return ValueTask.CompletedTask;
    }

    record Equation(BigInteger Value, List<BigInteger> Numbers);

    record Node(BigInteger Value, string? Operator, bool HaveChildren, Node? ParentNode = null);

    class Tree
    {
        public List<Node> Nodes { get; } = [];

        public Tree(Equation equation)
        {
            AddNode(equation.Numbers[0], equation.Numbers[1..]);
        }

        private void AddNode(BigInteger value, List<BigInteger> numbers, Node? parentNode = null, string? op = "")
        {
            var node = new Node(Value: value, Operator: op, HaveChildren: numbers.Count > 0, ParentNode: parentNode);
            Nodes.Add(node);

            if (numbers.Count > 0)
            {
                AddNode(value + numbers[0], numbers[1..], node, "+");
                AddNode(value * numbers[0], numbers[1..], node, "*");

                var combinedNumber = BigInteger.Parse($"{value}{numbers[0]}");
                AddNode(combinedNumber, numbers[1..], node, "||");
            }
        }
    }
}


