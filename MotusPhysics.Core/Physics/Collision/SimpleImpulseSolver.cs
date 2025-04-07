using MotusPhysics.Core.Physics.Data;
using MotusPhysics.Core.Utility;

namespace MotusPhysics.Core.Physics.Collision;

public class SimpleImpulseSolver
{
    public static void SolveCollisions(CollisionManifold[] manifolds)
    {
        CollisionResolution[] resolutions = new CollisionResolution[manifolds.Length];
        for (int i = 0; i < manifolds.Length; i++)
        {
            CollisionResolution resolution = IdentifyAndSolve(manifolds[i]);
            resolutions[i] = resolution;
        }
        
        foreach (CollisionResolution resolution in resolutions)
        {
            resolution.RigidBodyA.AddVelocity(resolution.VelocityChangeA);
            resolution.RigidBodyB.AddVelocity(resolution.VelocityChangeB);
        }
    }

    private static CollisionResolution IdentifyAndSolve(CollisionManifold manifold)
    {
        Vector velocityChangeA = Vector.Zero;
        Vector velocityChangeB = Vector.Zero;

        if (!manifold.RigidBodyA.IsStatic && !manifold.RigidBodyB.IsStatic)
        {
            foreach (Vector contactPoint in manifold.ContactPoints)
            {
                SolveRigidbodyRigidbody(manifold.RigidBodyA, manifold.RigidBodyB, manifold.CollisionNormal, manifold.ContactPoints.Length, out Vector velA, out Vector velB);
                velocityChangeA += velA;
                velocityChangeB += velB;
            }
            return new CollisionResolution(manifold.RigidBodyA, manifold.RigidBodyB, velocityChangeA, velocityChangeB, 0d, 0d);
        }

        if (manifold.RigidBodyB.IsStatic)
        {
            foreach (Vector contactPoint in manifold.ContactPoints)
            {
                SolveRigidbodyStaticbody(manifold.RigidBodyA, manifold.RigidBodyB, manifold.CollisionNormal, manifold.ContactPoints.Length, out Vector velA, out Vector velB);
                velocityChangeA += velA;
                velocityChangeB += velB;
            }
            return new CollisionResolution(manifold.RigidBodyA, manifold.RigidBodyB, velocityChangeA, velocityChangeB, 0d, 0d);
        }

        if (manifold.RigidBodyA.IsStatic)
        {
            foreach (Vector contactPoint in manifold.ContactPoints)
            {
                //Flip the rigidbodies and the normal when rigid body A is static
                SolveRigidbodyStaticbody(manifold.RigidBodyB, manifold.RigidBodyA, -manifold.CollisionNormal, manifold.ContactPoints.Length, out Vector velA, out Vector velB);
                velocityChangeA += velA;
                velocityChangeB += velB;
            }
            return new CollisionResolution(manifold.RigidBodyB, manifold.RigidBodyA, velocityChangeA, velocityChangeB, 0d, 0d);
        }

        return new CollisionResolution(manifold.RigidBodyA, manifold.RigidBodyB, velocityChangeA, velocityChangeB, 0d, 0d);
    }
    
    private static void SolveRigidbodyRigidbody(RigidBody rbA, RigidBody rbB, Vector collisionNormal, int contactCount, out Vector velA, out Vector velB)
    {
        velA = Vector.Zero;
        velB = Vector.Zero;
        
        Vector relativeVelocity = rbB.Velocity - rbA.Velocity;

        if (Vector.Dot(relativeVelocity, collisionNormal) > 0d)
            return;

        double e = Math.Min(rbA.Restitution, rbB.Restitution);

        double j = -(1d + e) * Vector.Dot(relativeVelocity, collisionNormal);
        j /= rbA.InverseMass + rbB.InverseMass;
        j /= contactCount;

        Vector impulse = j * collisionNormal;

        velA = -impulse * rbA.InverseMass;
        velB = impulse * rbB.InverseMass;
    }

    private static void SolveRigidbodyStaticbody(RigidBody rb, RigidBody staticRb, Vector collisionNormal, int contactCount, out Vector velA, out Vector velB)
    {
        velA = Vector.Zero;
        velB = Vector.Zero;
        
        Vector relativeVelocity = -rb.Velocity;

        if (Vector.Dot(relativeVelocity, collisionNormal) > 0d)
            return;

        double e = Math.Min(rb.Restitution, staticRb.Restitution);

        double j = -(1d + e) * Vector.Dot(relativeVelocity, collisionNormal);
        j /= rb.InverseMass;
        j /= contactCount;

        Vector impulse = j * collisionNormal;

        velA = -impulse * rb.InverseMass;
        velB = Vector.Zero;
    }
}