using MotusPhysics.Core.Physics.Colliders;
using MotusPhysics.Core.Utility;

namespace MotusPhysics.Core.Physics.Collision;

internal static class ContactPointFinder
{
    internal static Vector[] FindContactPoints(Collider colliderA, Collider colliderB)
    {
        if (colliderA is CircleCollider circle1 && colliderB is CircleCollider circle2)
            return [FindCircleCircleContactPoint(circle1, circle2)];

        if (colliderA is CircleCollider circleP1 && colliderB is PolygonCollider polyC1)
            return [FindCirclePolygonContactPoint(circleP1, polyC1)];
        
        if (colliderA is PolygonCollider polyC2 && colliderB is CircleCollider circleP2)
            return [FindCirclePolygonContactPoint(circleP2, polyC2)];

        if (colliderA is PolygonCollider poly1 && colliderB is PolygonCollider poly2)
            return FindPolygonPolygonContactPoints(poly1, poly2);

        return [];
    }
    
    internal static Vector[] FindPolygonPolygonContactPoints(PolygonCollider pA, PolygonCollider pB)
    {
        Vector contact1 = Vector.Zero;
        Vector contact2 = Vector.Zero;
        int contactCount = 0;

        double minDistSq = double.MaxValue;

        for(int i = 0; i < pA.Vertices.Length; i++)
        {
            Vector p = pA.Position + pA.Vertices[i];

            for(int j = 0; j < pB.Vertices.Length; j++)
            {
                Vector va = pB.Position + pB.Vertices[j];
                Vector vb = pB.Position + pB.Vertices[(j + 1) % pB.Vertices.Length];

                PointSegmentDistance(p, va, vb, out double distSq, out Vector cp);
                
                if(Math.Abs(distSq - minDistSq) < 0.0005d)
                {

                    if (Vector.DistanceSquared(cp, contact1) >= 0.0005d * 0.0005d)
                    {
                        contact2 = cp;
                        contactCount = 2;
                    }
                }
                else if(distSq < minDistSq)
                {
                    minDistSq = distSq;
                    contactCount = 1;
                    contact1 = cp;
                }
            }
        }

        for (int i = 0; i < pB.Vertices.Length; i++)
        {
            Vector p = pB.Position + pB.Vertices[i];

            for (int j = 0; j < pA.Vertices.Length; j++)
            {
                Vector va = pA.Position + pA.Vertices[j];
                Vector vb = pA.Position + pA.Vertices[(j + 1) % pA.Vertices.Length];

                PointSegmentDistance(p, va, vb, out double distSq, out Vector cp);

                if(Math.Abs(distSq - minDistSq) < 0.0005d)
                {
                    if (Vector.DistanceSquared(cp, contact1) >= 0.0005d * 0.0005d)
                    {
                        contact2 = cp;
                        contactCount = 2;
                    }
                }
                else if (distSq < minDistSq)
                {
                    minDistSq = distSq;
                    contactCount = 1;
                    contact1 = cp;
                }
            }
        }

        if (contactCount == 1)
            return [contact1];
        return [contact1, contact2];
    }
    
    internal static Vector FindCirclePolygonContactPoint(CircleCollider c, PolygonCollider p)
    {
        Vector contactPoint = Vector.Zero;

        double minDistSq = double.MaxValue;

        for(int i = 0; i < p.Vertices.Length; i++)
        {
            Vector va = p.Position + p.Vertices[i];
            Vector vb = p.Position + p.Vertices[(i + 1) % p.Vertices.Length];

            PointSegmentDistance(c.Position, va, vb, out double distSq, out Vector contact);
            
            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                contactPoint = contact;
            }
        }

        return contactPoint;
    }

    internal static Vector FindCircleCircleContactPoint(CircleCollider cA, CircleCollider cB)
    {
        Vector ab = (cB.Position - cA.Position).Normalized();
        return cA.Position + ab * cA.Radius;
    }
    
    internal static void PointSegmentDistance(Vector point, Vector segmentVertexA, Vector segmentVertexB, out double distanceSquared, out Vector contactPoint)
    {
        Vector segment = segmentVertexB - segmentVertexA;
        Vector ap = point - segmentVertexA;

        double proj = Vector.Dot(ap, segment);
        double mag = segment.Magnitude();
        double segmentMagnitudeSq = mag * mag;
        double d = proj / segmentMagnitudeSq;

        if(d <= 0f)
        {
            contactPoint = segmentVertexA;
        }
        else if(d >= 1f)
        {
            contactPoint = segmentVertexB;
        }
        else
        {
            contactPoint = segmentVertexA + segment * d;
        }

        distanceSquared = Vector.DistanceSquared(point, contactPoint);
    }
}