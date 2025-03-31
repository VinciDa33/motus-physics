using MotusPhysics.Core.Modularity;

namespace MotusPhysics.Core.Testing;

public class ModuleTestDummy : IMotusModule
{
    public void Initialize()
    {
        Console.WriteLine("Dummy Initialized!");
    }

    public void Update()
    {
        Console.WriteLine("Dummy Updated!");
    }

    public void Shutdown()
    {
        Console.WriteLine("Dummy Shutdown!");
    }
}