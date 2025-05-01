using MotusPhysics.Core.Utility;

namespace MotusPhysics.Core.Physics.Data;

public class CollisionEvent(RigidBody rigidBodyA, RigidBody rigidBodyB, Vector collisionNormal, double penetrationDepth)
{
    public readonly RigidBody RigidBodyA = rigidBodyA;
    public readonly RigidBody RigidBodyB = rigidBodyB;
    public readonly Vector CollisionNormal = collisionNormal;
    public readonly double PenetrationDepth = penetrationDepth;
}