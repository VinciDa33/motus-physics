using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Data;

public class CollisionEvent(PhysicsObject physicsObject1, PhysicsObject physicsObject2, Vector normal, double depth)
{
    public readonly PhysicsObject PhysicsObject1 = physicsObject1;
    public readonly PhysicsObject PhysicsObject2 = physicsObject2;
    public readonly Vector CollisionNormal = normal;
    public readonly double PenetrationDepth = depth;
}