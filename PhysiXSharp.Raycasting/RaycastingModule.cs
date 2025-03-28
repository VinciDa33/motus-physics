using PhysiXSharp.Core;
using PhysiXSharp.Core.Modularity;
using PhysiXSharp.Core.Physics;

namespace PhysiXSharp.Raycasting;

public class RaycastingModule : IPhysiXModule
{
    public void Initialize()
    {
        PhysiX.Logger.Log("Ray casting module initialized!");
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