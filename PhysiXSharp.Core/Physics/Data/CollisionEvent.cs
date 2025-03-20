using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Data;

public class CollisionEvent(Rigidbody rigidbodyA, Rigidbody rigidbodyB, Vector collisionNormal, double penetrationDepth)
{
    public readonly Rigidbody RigidbodyA = rigidbodyA;
    public readonly Rigidbody RigidbodyB = rigidbodyB;
    public readonly Vector CollisionNormal = collisionNormal;
    public readonly double PenetrationDepth = penetrationDepth;
}