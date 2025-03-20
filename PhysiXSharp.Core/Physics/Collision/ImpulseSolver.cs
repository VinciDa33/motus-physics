﻿using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Physics.Data;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Collision;

public static class ImpulseSolver
{
    public static CollisionResolution SolveCollision(CollisionManifold manifold)
    {
        Vector velocityChangeA = Vector.Zero;
        Vector velocityChangeB = Vector.Zero;
        float angularChangeA = 0f;
        float angularChangeB = 0f;


        if (!manifold.RigidbodyA.IsStatic && !manifold.RigidbodyB.IsStatic)
        {
            foreach (Vector contactPoint in manifold.ContactPoints)
            {
                SolveRigidbodyRigidbody(manifold.RigidbodyA, manifold.RigidbodyB, manifold.CollisionNormal, manifold.ContactPoints.Length, out Vector velA, out Vector velB);
                velocityChangeA += velA;
                velocityChangeB += velB;
            }
            return new CollisionResolution(manifold.RigidbodyA, manifold.RigidbodyB, velocityChangeA, velocityChangeB, angularChangeA, angularChangeB);
        }

        if (!manifold.RigidbodyA.IsStatic)
        {
            foreach (Vector contactPoint in manifold.ContactPoints)
            {
                SolveRigidbodyStaticbody(manifold.RigidbodyA, manifold.RigidbodyB, manifold.CollisionNormal, manifold.ContactPoints.Length, out Vector velA, out Vector velB);
                velocityChangeA += velA;
                velocityChangeB += velB;
            }
            return new CollisionResolution(manifold.RigidbodyA, manifold.RigidbodyB, velocityChangeA, velocityChangeB, angularChangeA, angularChangeB);
        }

        if (!manifold.RigidbodyB.IsStatic)
        {
            foreach (Vector contactPoint in manifold.ContactPoints)
            {
                //Flip the rigidbodies and the normal when rigid body A is static
                SolveRigidbodyStaticbody(manifold.RigidbodyB, manifold.RigidbodyA, -manifold.CollisionNormal, manifold.ContactPoints.Length, out Vector velA, out Vector velB);
                velocityChangeA += velA;
                velocityChangeB += velB;
            }
            return new CollisionResolution(manifold.RigidbodyB, manifold.RigidbodyA, velocityChangeA, velocityChangeB, angularChangeA, angularChangeB);
        }

        return new CollisionResolution(manifold.RigidbodyA, manifold.RigidbodyB, velocityChangeA, velocityChangeB, angularChangeA, angularChangeB);
    }
    
    private static void SolveRigidbodyRigidbody(Rigidbody rbA, Rigidbody rbB, Vector collisionNormal, int contactCount, out Vector velA, out Vector velB)
    {
        velA = Vector.Zero;
        velB = Vector.Zero;
        
        Vector relativeVelocity = rbB.Velocity - rbA.Velocity;

        if (Vector.Dot(relativeVelocity, collisionNormal) > 0d)
            return;

        double e = Math.Min(rbA.Restitution, rbB.Restitution);

        double j = -(1f + e) * Vector.Dot(relativeVelocity, collisionNormal);
        j /= rbA.InverseMass + rbB.InverseMass;
        j /= contactCount;

        Vector impulse = j * collisionNormal;

        velA = -impulse * rbA.InverseMass;
        velB = impulse * rbB.InverseMass;
    }

    private static void SolveRigidbodyStaticbody(Rigidbody rb, Rigidbody staticRb, Vector collisionNormal, int contactCount, out Vector velA, out Vector velB)
    {
        velA = Vector.Zero;
        velB = Vector.Zero;
        
        Vector relativeVelocity = -rb.Velocity;

        if (Vector.Dot(relativeVelocity, collisionNormal) > 0d)
            return;

        double e = Math.Min(rb.Restitution, staticRb.Restitution);

        double j = -(1f + e) * Vector.Dot(relativeVelocity, collisionNormal);
        j /= rb.InverseMass;
        j /= contactCount;

        Vector impulse = j * collisionNormal;

        velA = -impulse * rb.InverseMass;
        velB = Vector.Zero;
    }
}