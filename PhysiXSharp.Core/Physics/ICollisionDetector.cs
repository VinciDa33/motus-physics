namespace PhysiXSharp.Core.Physics;

public interface ICollisionDetector
{
    public bool CheckCollision(PhysicsObject physicsObject1, PhysicsObject physicsObject2, out CollisionData data);
}