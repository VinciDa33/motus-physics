using MotusPhysics.Core.Utility;

namespace MotusPhysics.Core.Physics.Data;

public sealed class CollisionManifold
{
    public readonly RigidBody RigidBodyA;
    public readonly RigidBody RigidBodyB;
    public readonly Vector CollisionNormal;
    public readonly double PenetrationDepth;
    public readonly Vector[] ContactPoints;
    public readonly int SimStep;

    public CollisionManifold(RigidBody rigidBodyA, RigidBody rigidBodyB, Vector collisionNormal, double penetrationDepth, Vector[] contactPoints, int simStep)
    {
        RigidBodyA = rigidBodyA;
        RigidBodyB = rigidBodyB;
        CollisionNormal = collisionNormal;
        PenetrationDepth = penetrationDepth;
        ContactPoints = contactPoints;
        SimStep = simStep;
    }
    
    public CollisionManifold(CollisionEvent collisionEvent, Vector[] contactPoints, int simStep) 
    {
        RigidBodyA = collisionEvent.RigidBodyA;
        RigidBodyB = collisionEvent.RigidBodyB;
        CollisionNormal = collisionEvent.CollisionNormal;
        PenetrationDepth = collisionEvent.PenetrationDepth;
        ContactPoints = contactPoints;
        SimStep = simStep;
    }
}