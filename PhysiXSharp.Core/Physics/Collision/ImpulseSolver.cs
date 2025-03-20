using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Physics.Data;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Collision;

public static class ImpulseSolver
{
    public static void SolveCollision(CollisionManifold manifold, out CollisionResolution resolution)
    {
        Vector velocityChange1 = Vector.Zero;
        Vector velocityChange2 = Vector.Zero;
        float angularChange1 = 0f;
        float angularChange2 = 0f;
        
        /*
        if (!manifold.RigidbodyA.IsStatic && !manifold.RigidbodyB.IsStatic)
            foreach (Vector contactPoint in manifold.ContactPoints)
                SolveRigidbodyRigidbody(manifold.RigidbodyA, manifold.RigidbodyB, contactPoint, manifold.CollisionNormal, manifold.ContactPoints.Length);

        else if (!manifold.RigidbodyA.IsStatic)
            foreach (Vector contactPoint in manifold.ContactPoints)
                SolveRigidbodyStaticbody(manifold.RigidbodyA, manifold.RigidbodyB, contactPoint, manifold.CollisionNormal);

        else if (!manifold.RigidbodyB.IsStatic)
            foreach (Vector contactPoint in manifold.ContactPoints)
                SolveRigidbodyStaticbody(manifold.RigidbodyB, manifold.RigidbodyA, contactPoint, manifold.CollisionNormal);
        */
        resolution = new CollisionResolution(manifold.RigidbodyA, manifold.RigidbodyB, velocityChange1, velocityChange2, angularChange1, angularChange2);
    }
    
    private static void SolveRigidbodyRigidbody(Rigidbody rbA, Rigidbody rbB, Vector contactPoint, Vector normal, int contactCount)
    {

    }

    private static void SolveRigidbodyStaticbody(Rigidbody rb, Rigidbody staticRb, Vector contactPoint, Vector normal, int contactCount)
    {
        
    }
}