using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Physics.Data;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Collision;

public static class CollisionSeparator
{
    public static void SeparateCollisionBodies(CollisionEvent[] collisionEvents)
    {
        foreach (CollisionEvent collisionEvent in collisionEvents)
        {
            //Skip calculating seperation for collisions involving triggers
            if (collisionEvent.RigidbodyA.Collider.IsTrigger || collisionEvent.RigidbodyB.Collider.IsTrigger)
                return;

            Vector correction = collisionEvent.CollisionNormal * collisionEvent.PenetrationDepth;
            if (collisionEvent.RigidbodyA.IsStatic)
                collisionEvent.RigidbodyB.TranslatePosition(correction);
            else if (collisionEvent.RigidbodyB.IsStatic)
                collisionEvent.RigidbodyA.TranslatePosition(-correction);
            else
            {
                collisionEvent.RigidbodyA.TranslatePosition(-correction / 2d);
                collisionEvent.RigidbodyB.TranslatePosition(correction / 2d);
            }
        }
    }
}