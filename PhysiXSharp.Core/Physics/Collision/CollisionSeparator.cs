using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Collision;

public static class CollisionSeparator
{
    public static void SeparateCollisionBodies(Rigidbody rigidbodyA, Rigidbody rigidbodyB, Vector normal, double depth)
    {
        Vector correction = normal * depth;
        if (rigidbodyA.IsStatic)
            rigidbodyB.TranslatePosition(correction);
        else if (rigidbodyB.IsStatic)
            rigidbodyA.TranslatePosition(-correction);
        else
        {
            rigidbodyA.TranslatePosition(-correction / 2d);
            rigidbodyB.TranslatePosition(correction / 2d);
        }
    }
}