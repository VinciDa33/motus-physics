using MotusPhysics.Core.Physics.Data;
using MotusPhysics.Core.Utility;

namespace MotusPhysics.Core.Physics.Collision;

public static class ImpulseSolver
{
    public static void SolveCollisions(CollisionManifold[] manifolds)
    {
        List<CollisionResolution> resolutions = new List<CollisionResolution>();
        foreach (CollisionManifold manifold in manifolds)
        {
            //Skip calculating impulses for collisions involving triggers
            if (manifold.RigidBodyA.Collider.IsTrigger || manifold.RigidBodyB.Collider.IsTrigger)
                continue;
            resolutions.Add(IdentifyAndSolve(manifold));
        }
        
        foreach (CollisionResolution resolution in resolutions)
        {
            resolution.RigidBodyA.AddVelocity(resolution.VelocityChangeA);
            resolution.RigidBodyB.AddVelocity(resolution.VelocityChangeB);
            
            resolution.RigidBodyA.AddAngularVelocity(resolution.AngularVelocityChangeA);
            resolution.RigidBodyB.AddAngularVelocity(resolution.AngularVelocityChangeB);
        }
    }

    private static CollisionResolution IdentifyAndSolve(CollisionManifold manifold)
    {
        Vector velocityChangeA = Vector.Zero;
        Vector velocityChangeB = Vector.Zero;
        double angularVelocityChangeA = 0d;
        double angularVelocityChangeB = 0d;

        if (!manifold.RigidBodyA.IsStatic && !manifold.RigidBodyB.IsStatic)
        {
            foreach (Vector contactPoint in manifold.ContactPoints)
            {
                SolveRigidbodyRigidbody(manifold.RigidBodyA, manifold.RigidBodyB, manifold.CollisionNormal, contactPoint, manifold.ContactPoints.Length, out Vector velA, out Vector velB, out double angA, out double angB);
                velocityChangeA += velA;
                velocityChangeB += velB;
                angularVelocityChangeA += angA;
                angularVelocityChangeB += angB;
            }
            return new CollisionResolution(manifold.RigidBodyA, manifold.RigidBodyB, velocityChangeA, velocityChangeB, angularVelocityChangeA, angularVelocityChangeB);
        }

        if (manifold.RigidBodyB.IsStatic)
        {
            foreach (Vector contactPoint in manifold.ContactPoints)
            {
                SolveRigidbodyStaticbody(manifold.RigidBodyA, manifold.RigidBodyB, manifold.CollisionNormal, contactPoint, manifold.ContactPoints.Length, out Vector velA, out Vector velB, out double angA, out double angB);
                velocityChangeA += velA;
                velocityChangeB += velB;
                angularVelocityChangeA += angA;
                angularVelocityChangeB += angB;
            }
            return new CollisionResolution(manifold.RigidBodyA, manifold.RigidBodyB, velocityChangeA, velocityChangeB, angularVelocityChangeA, angularVelocityChangeB);
        }

        if (manifold.RigidBodyA.IsStatic)
        {
            foreach (Vector contactPoint in manifold.ContactPoints)
            {
                //Flip the rigidbodies and the normal when rigid body A is static
                SolveRigidbodyStaticbody(manifold.RigidBodyB, manifold.RigidBodyA, -manifold.CollisionNormal, contactPoint, manifold.ContactPoints.Length, out Vector velA, out Vector velB, out double angA, out double angB);
                velocityChangeA += velA;
                velocityChangeB += velB;
                angularVelocityChangeA += angA;
                angularVelocityChangeB += angB;
            }
            return new CollisionResolution(manifold.RigidBodyB, manifold.RigidBodyA, velocityChangeA, velocityChangeB, angularVelocityChangeA, angularVelocityChangeB);
        }

        return new CollisionResolution(manifold.RigidBodyA, manifold.RigidBodyB, velocityChangeA, velocityChangeB, angularVelocityChangeA, angularVelocityChangeB);
    }
    
    private static void SolveRigidbodyRigidbody(RigidBody rbA, RigidBody rbB, Vector collisionNormal, Vector contactPoint, int contactCount, out Vector velA, out Vector velB, out double angA, out double angB)
    {
        velA = Vector.Zero;
        velB = Vector.Zero;
        angA = 0d;
        angB = 0d;

        double e = Math.Min(rbA.Restitution, rbB.Restitution);
        
        Vector ra = contactPoint - rbA.Position;
        Vector rb = contactPoint - rbB.Position;

        Vector raPerp = new Vector(-ra.y, ra.x);
        Vector rbPerp = new Vector(-rb.y, rb.x);
        
        Vector angularLinearVelocityA = rbA.AngularVelocity * raPerp;
        Vector angularLinearVelocityB = rbB.AngularVelocity * rbPerp;
        
        Vector relativeVelocity = (rbB.Velocity + angularLinearVelocityB) - (rbA.Velocity + angularLinearVelocityA);
        double contactVelocityMag = Vector.Dot(relativeVelocity, collisionNormal);
        
        if (contactVelocityMag > 0f)
            return;
        
        
        double raPerpDotN = Vector.Dot(raPerp, collisionNormal);
        double rbPerpDotN = Vector.Dot(rbPerp, collisionNormal);

        double denominator = rbA.InverseMass + rbB.InverseMass + raPerpDotN * raPerpDotN * rbA.InverseInertia + rbPerpDotN * rbPerpDotN * rbB.InverseInertia;
        
        double j = -(1d + e) * contactVelocityMag;
        j /= denominator;
        j /= contactCount;
        
        Vector impulse = j * collisionNormal;
        
        velA = -impulse * rbA.InverseMass;
        velB = impulse * rbB.InverseMass;
        
        angA = -Vector.Cross(ra, impulse) * rbA.InverseInertia;
        angB = Vector.Cross(rb, impulse) * rbB.InverseInertia;
    }

    private static void SolveRigidbodyStaticbody(RigidBody rbA, RigidBody rbStatic, Vector collisionNormal, Vector contactPoint, int contactCount, out Vector velA, out Vector velB, out double angA, out double angB)
    {
        velA = Vector.Zero;
        velB = Vector.Zero;
        angA = 0d;
        angB = 0d;

        double e = Math.Min(rbA.Restitution, rbStatic.Restitution);
        
        Vector ra = contactPoint - rbA.Position;

        Vector raPerp = new Vector(-ra.y, ra.x);
        
        Vector angularLinearVelocityA = rbA.AngularVelocity * raPerp;
        
        Vector relativeVelocity = -(rbA.Velocity + angularLinearVelocityA);
        double contactVelocityMag = Vector.Dot(relativeVelocity, collisionNormal);
        
        if (contactVelocityMag > 0f)
            return;
        
        
        double raPerpDotN = Vector.Dot(raPerp, collisionNormal);

        double denominator = rbA.InverseMass + raPerpDotN * raPerpDotN * rbA.InverseInertia;
        
        double j = -(1d + e) * contactVelocityMag;
        j /= denominator;
        j /= contactCount;
        
        Vector impulse = j * collisionNormal;
        
        velA = -impulse * rbA.InverseMass;
        angA = -Vector.Cross(ra, impulse) * rbA.InverseInertia;
    }
}