using PhysiXSharp.Core;
using PhysiXSharp.Core.Modularity;
using PhysiXSharp.Core.Physics;

namespace PhysiXSharp.Raycasting;

public class RaycastingModule : IPhysiXModule
{
    public void Initialize(PhysicsManager physicsManager)
    {
        PhysiX.Logger.Log("Ray casting module successfully loaded!");
    }

    public void Update()
    {
        //throw new NotImplementedException();
    }
}