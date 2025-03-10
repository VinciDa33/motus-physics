using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Collision;

internal static class ContactPointFinder
{
    internal static Vector[] FindContactPoints(Collider collider1, Collider collider2)
    {
        if (collider1 is CircleCollider circle1 && collider2 is CircleCollider circle2)
            return [FindCircleCircleContactPoint(circle1, circle2)];

        if (collider1 is CircleCollider circleP1 && collider2 is PolygonCollider polyC1)
            return [FindCirclePolygonContactPoint(circleP1, polyC1)];
        
        if (collider1 is PolygonCollider polyC2 && collider2 is CircleCollider circleP2)
            return [FindCirclePolygonContactPoint(circleP2, polyC2)];

        if (collider1 is PolygonCollider poly1 && collider2 is PolygonCollider poly2)
            return FindPolygonPolygonContactPoints(poly1, poly2);

        return [];
    }
    
    internal static Vector[] FindPolygonPolygonContactPoints(PolygonCollider p1, PolygonCollider p2)
    {
        Vector contact1 = Vector.Zero;
        Vector contact2 = Vector.Zero;
        int contactCount = 0;

        double minDistSq = double.MaxValue;

        for(int i = 0; i < p1.Vertices.Count; i++)
        {
            Vector p = p1.Position + p1.Vertices[i];

            for(int j = 0; j < p2.Vertices.Count; j++)
            {
                Vector va = p2.Position + p2.Vertices[j];
                Vector vb = p2.Position + p2.Vertices[(j + 1) % p2.Vertices.Count];

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

        for (int i = 0; i < p2.Vertices.Count; i++)
        {
            Vector p = p2.Position + p2.Vertices[i];

            for (int j = 0; j < p1.Vertices.Count; j++)
            {
                Vector va = p1.Position + p1.Vertices[j];
                Vector vb = p1.Position + p1.Vertices[(j + 1) % p1.Vertices.Count];

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

        for(int i = 0; i < p.Vertices.Count; i++)
        {
            Vector va = p.Position + p.Vertices[i];
            Vector vb = p.Position + p.Vertices[(i + 1) % p.Vertices.Count];

            PointSegmentDistance(c.Position, va, vb, out double distSq, out Vector contact);
            
            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                contactPoint = contact;
            }
        }

        return contactPoint;
    }

    internal static Vector FindCircleCircleContactPoint(CircleCollider c1, CircleCollider c2)
    {
        Vector ab = (c2.Position - c1.Position).Normalized();
        return c1.Position + ab * c1.Radius;
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