using DanielCarey.Day05;

using Microsoft.Extensions.Logging;

namespace DailyTests;

[TestClass]
public sealed class Day05Tests : BaseTest
{
    private readonly string _star1DataPath = Path.Combine(TestSetup.RootPath, @"src\Day05\Data1.txt");
    private readonly string _star2DataPath = Path.Combine(TestSetup.RootPath, @"src\Day05\Data2.txt");

    public Day05Tests()
    {
        SetWorkingDirectory(@"src\Day05");
    }

    [TestMethod]
    public async Task Star1Test()
    {
        var logger = _loggerFactory.CreateLogger<Star1>();

        var star = new Star1(logger, _star1DataPath);

        var result = await star.RunAsync();

        Assert.IsTrue(result == 5391);
    }

    [TestMethod]
    public async Task Star2Test()
    {
        var logger = _loggerFactory.CreateLogger<Star2>();

        var star = new Star2(logger, _star2DataPath);

        var result = await star.RunAsync();

        Assert.IsTrue(result == 6142);
    }
}