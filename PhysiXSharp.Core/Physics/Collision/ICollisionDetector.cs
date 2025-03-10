using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Physics.Data;

namespace PhysiXSharp.Core.Physics.Collision;

public interface ICollisionDetector
{
    public bool CheckCollision(PhysicsObject physicsObject1, PhysicsObject physicsObject2, out CollisionManifold? manifold);
}