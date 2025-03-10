using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Collision;

public static class CollisionSeparator
{
    public static void SeparateCollisionBodies(PhysicsObject physicsObject1, PhysicsObject physicsObject2, Vector normal, double depth)
    {
        Vector correction = normal * depth;
        if (physicsObject1.IsStatic)
        {
            ((Rigidbody) physicsObject2).TranslatePosition(correction);
        }
        else if (physicsObject2.IsStatic)
        {
            ((Rigidbody) physicsObject1).TranslatePosition(-correction);
        }
        else
        {
            ((Rigidbody) physicsObject1).TranslatePosition(-correction / 2d);
            ((Rigidbody) physicsObject2).TranslatePosition(correction / 2d);
        }
    }
}