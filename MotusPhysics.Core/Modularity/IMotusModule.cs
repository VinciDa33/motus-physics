using MotusPhysics.Core.Physics;

namespace MotusPhysics.Core.Modularity;

public interface IMotusModule
{
    public void Initialize();
    public void Update();

    public void Shutdown();
}