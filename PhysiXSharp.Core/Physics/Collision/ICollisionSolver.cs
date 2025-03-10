using PhysiXSharp.Core.Physics.Bodies;

namespace PhysiXSharp.Core.Physics.Collision;

public interface ICollisionSolver
{
    public void SolveCollision(PhysicsObject physicsObject1, PhysicsObject physicsObject2);
}