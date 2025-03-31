using MotusPhysics.Core.Utility;

namespace MotusPhysics.Core.Physics.Data;

public sealed class CollisionManifold
{
    public readonly Rigidbody RigidbodyA;
    public readonly Rigidbody RigidbodyB;
    public readonly Vector CollisionNormal;
    public readonly double PenetrationDepth;
    public readonly Vector[] ContactPoints;
    public readonly int SimStep;

    public CollisionManifold(Rigidbody rigidbodyA, Rigidbody rigidbodyB, Vector collisionNormal, double penetrationDepth, Vector[] contactPoints, int simStep)
    {
        RigidbodyA = rigidbodyA;
        RigidbodyB = rigidbodyB;
        CollisionNormal = collisionNormal;
        PenetrationDepth = penetrationDepth;
        ContactPoints = contactPoints;
        SimStep = simStep;
    }
    
    public CollisionManifold(CollisionEvent collisionEvent, Vector[] contactPoints, int simStep) 
    {
        RigidbodyA = collisionEvent.RigidbodyA;
        RigidbodyB = collisionEvent.RigidbodyB;
        CollisionNormal = collisionEvent.CollisionNormal;
        PenetrationDepth = collisionEvent.PenetrationDepth;
        ContactPoints = contactPoints;
        SimStep = simStep;
    }
}