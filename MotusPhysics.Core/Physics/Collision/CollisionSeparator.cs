using MotusPhysics.Core.Physics.Data;
using MotusPhysics.Core.Utility;

namespace MotusPhysics.Core.Physics.Collision;

public static class CollisionSeparator
{
    public static void SeparateCollisionBodies(CollisionEvent[] collisionEvents)
    {
        foreach (CollisionEvent collisionEvent in collisionEvents)
        {
            //Skip calculating seperation for collisions involving triggers
            if (collisionEvent.RigidBodyA.Collider.IsTrigger || collisionEvent.RigidBodyB.Collider.IsTrigger)
                return;

            Vector correction = collisionEvent.CollisionNormal * collisionEvent.PenetrationDepth;
            if (collisionEvent.RigidBodyA.IsStatic)
                collisionEvent.RigidBodyB.TranslatePosition(correction);
            else if (collisionEvent.RigidBodyB.IsStatic)
                collisionEvent.RigidBodyA.TranslatePosition(-correction);
            else
            {
                collisionEvent.RigidBodyA.TranslatePosition(-correction / 2d);
                collisionEvent.RigidBodyB.TranslatePosition(correction / 2d);
            }
        }
    }
}