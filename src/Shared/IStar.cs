namespace DanielCarey.Shared;

public interface IStar
{
    string Name { get; }
    ValueTask RunAsync();
}
