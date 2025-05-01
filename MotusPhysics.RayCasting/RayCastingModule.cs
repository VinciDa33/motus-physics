using MotusPhysics.Core;
using MotusPhysics.Core.Modularity;

namespace MotusPhysics.RayCasting;

public class RayCastingModule : IMotusModule
{
    public void Initialize()
    {
        Motus.Logger.Log("Ray casting module initialized!");
    }

    public void Update()
    {
        //throw new NotImplementedException();
    }

    public void Shutdown()
    {
        //throw new NotImplementedException();
    }
}