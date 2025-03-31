using MotusPhysics.Core.Modularity;

namespace MotusPhysics.Core.Testing;

public class ModuleLoadingTest
{
    [Test]
    public void Test_ModuleLoading()
    {
        var file = Directory
            .EnumerateFiles("../../../../../../", "*" + ".dll", SearchOption.AllDirectories).FirstOrDefault(f => Path.GetFileName(f).Contains("MotusPhysics.Raycasting.dll", StringComparison.OrdinalIgnoreCase));
        
        if (file == null)
            Assert.Fail();
        
        Console.WriteLine(Path.GetDirectoryName(file));
        Motus.SetModulePath(Path.GetDirectoryName(file));
        
        if (!Motus.IsInitialized)
            Motus.Initialize();
        
        if (ModuleManager.Instance.GetListOfLoadedModules().Length <= 0) 
            Assert.Fail();
        
        Console.WriteLine("Test found: " +  ModuleManager.Instance.GetListOfLoadedModules()[0]);
        Assert.Pass();
    }

    [TearDown]
    public void Cleanup()
    {
        if (Motus.IsInitialized)
            Motus.Shutdown();
    }

}