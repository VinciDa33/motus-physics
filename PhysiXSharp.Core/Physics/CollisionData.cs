
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics;

public class CollisionData(PhysicsObject physicsObject1, PhysicsObject physicsObject2, bool colliding)
{
    public bool Colliding = colliding;
    public PhysicsObject PhysicsObject1 = physicsObject1;
    public PhysicsObject PhysicsObject2 = physicsObject2;
    public Vector ContactPoint = Vector.Zero;
    public Vector CollisionNormal = Vector.Zero;
    public double PenetrationDepth = 0d;
}