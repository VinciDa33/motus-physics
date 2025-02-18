using PhysiXSharp.Core.ModuleInterfaces;

namespace PhysiXSharp.Core.Testing;

public class ModuleLoadingTests
{
    [SetUp]
    public void Setup()
    {
        PhysiX.Initialize();
    }

    [Test]
    public void TestModulesLoaded()
    {
        List<IPhysiXModule> loadedModules = ModuleManager.Instance.GetLoadedModules();
        Console.WriteLine("Loaded modules count: " + loadedModules.Count);
        Assert.Greater(loadedModules.Count, 0);
    }
}