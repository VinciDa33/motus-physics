using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Physics.Data;

namespace PhysiXSharp.Core.Physics.Collision;

public interface ICollisionSolver
{
    public void SolveCollision(CollisionManifold manifold);
}