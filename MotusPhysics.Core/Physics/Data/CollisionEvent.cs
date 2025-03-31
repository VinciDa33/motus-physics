using MotusPhysics.Core.Utility;

namespace MotusPhysics.Core.Physics.Data;

public class CollisionEvent(Rigidbody rigidbodyA, Rigidbody rigidbodyB, Vector collisionNormal, double penetrationDepth)
{
    public readonly Rigidbody RigidbodyA = rigidbodyA;
    public readonly Rigidbody RigidbodyB = rigidbodyB;
    public readonly Vector CollisionNormal = collisionNormal;
    public readonly double PenetrationDepth = penetrationDepth;
}