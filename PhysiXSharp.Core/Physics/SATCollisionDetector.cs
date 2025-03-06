

using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics;

public class SATCollisionDetector : ICollisionDetector
{
    public bool CheckCollision(PhysicsObject po1, PhysicsObject po2, out CollisionData data)
    {
        //Skip if an object is missing a collider
        if (po1.Collider == null || po2.Collider == null)
        {
            data = new CollisionData(po1, po2, false);
            return false;
        }
        //Check AABB overlap and skip if no overlap is found
        if (!Collider.OverlapAABB(po1.Collider, po2.Collider))
        {
            data = new CollisionData(po1, po2, false);
            return false;
        }
        
        //Check convex collision precisely
        bool isColliding = CheckSAT(po1, po2, out Vector normal, out double depth);
        if (!isColliding)
        {
            data = new CollisionData(po1, po2, false);
            return false;
        }

        data = new CollisionData(po1, po2, true);
        data.CollisionNormal = normal;
        data.PenetrationDepth = depth;
        
        //Return with this data if a collider is a trigger area
        if (po1.Collider.IsTrigger || po2.Collider.IsTrigger)
            return true;

        return true;
    }

    private bool CheckSAT(PhysicsObject po1, PhysicsObject po2, out Vector normal, out double depth)
    {
        if (po1.Collider is CircleCollider circle1 && po2.Collider is PolygonCollider poly1)
            return SATCirclePolygon(circle1, poly1, out normal, out depth);

        if (po1.Collider is PolygonCollider poly2 && po2.Collider is CircleCollider circle2)
            return SATCirclePolygon(circle2, poly2, out normal, out depth);
        
        if (po1.Collider is CircleCollider c1 && po2.Collider is CircleCollider c2)
            return SATCircleCircle(c1, c2, out normal, out depth);

        if (po1.Collider is PolygonCollider p1 && po2.Collider is PolygonCollider p2)
            return SATPolygonPolygon(p1, p2, out normal, out depth);

        normal = Vector.Zero;
        depth = 0d;
        return true;
    }

    private bool SATPolygonPolygon(PolygonCollider p1, PolygonCollider p2, out Vector normal, out double depth)
    {
        normal = Vector.Zero;
        depth = double.MaxValue;
        
        List<Vector> axes = new List<Vector>();
        axes.AddRange(p1.Normals);
        axes.AddRange(p2.Normals);
        
        foreach (Vector axis in axes)
        {
            (double min1, double max1) = p1.Project(axis);
            (double min2, double max2) = p2.Project(axis);
            
            if (max1 < min2 || max2 < min1)
                return false; // Separating axis found, no collision
            
            double axisDepth = Math.Min(max2 - min1, max1 - min2);

            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }
        }

        Vector direction = p2.Position - p1.Position;

        if (Vector.Dot(direction, normal) < 0f)
        {
            normal = -normal;
        }

        return true;
    }

    private bool SATCirclePolygon(CircleCollider c, PolygonCollider p, out Vector normal, out double depth)
    {
        normal = Vector.Zero;
        depth = double.MaxValue;

        List<Vector> axes = new List<Vector>();
        axes.AddRange(p.Normals);
        Vector closestVertex = p.ClosestVertexToPoint(c.Position);
        axes.Add((closestVertex - c.Position).Normalized());
        
        foreach (Vector axis in axes)
        {
            (double min1, double max1) = p.Project(axis);
            (double min2, double max2) = c.Project(axis);
            
            if (max1 < min2 || max2 < min1)
                return false; // Separating axis found, no collision
            
            double axisDepth = Math.Min(max2 - min1, max1 - min2);

            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }
        }

        Vector direction = p.Position - c.Position;

        
        if (Vector.Dot(direction, normal) < 0f)
        {
            normal = -normal;
        }

        return true;
    }
    
    private bool SATCircleCircle(CircleCollider c1, CircleCollider c2, out Vector normal, out double depth)
    {
        normal = Vector.Zero;
        depth = 0d;

        double distance = Vector.Distance(c1.Position, c2.Position);
        double radiusSum = c1.Radius + c2.Radius;

        if(distance >= radiusSum)
            return false;

        normal = (c2.Position - c1.Position).Normalized();
        depth = radiusSum - distance;

        return true;
    }

    
    private void FindContactPoints(CollisionData data)
    {
        
    }
    
}