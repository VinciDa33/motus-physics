using PhysiXSharp.Core.Physics;

namespace PhysiXSharp.Core.Modularity;

public interface IPhysiXModule
{
    public void Initialize(PhysicsManager physicsManager); //May need some objects
    public void Update(); //May need some delta time
}