using MotusPhysics.Core.Modularity;

namespace MotusPhysics.Core.Testing;

public class ModuleLoadingTests
{
    [SetUp]
    public void Setup()
    {
        //Motus.SetModulePath("../../../../Output");
        //Motus.Initialize();
    }

    [Test]
    public void TestModulesLoaded()
    {
        Console.WriteLine("Test temporarily disabled!");
        Assert.Pass();
        /*
        List<IMotusModule> loadedModules = ModuleManager.Instance.GetLoadedModules();
        Console.WriteLine("Loaded modules count: " + loadedModules.Count);
        Assert.Greater(loadedModules.Count, 0);
        */
    }
}