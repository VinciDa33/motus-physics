using MotusPhysics.Core.Physics.Colliders;
using MotusPhysics.Core.Physics.Data;
using MotusPhysics.Core.Utility;

namespace MotusPhysics.Core.Physics.Collision;

public static class SATCollisionDetector
{
    public static CollisionEvent[] CheckCollision(List<RigidBody> rigidbodies)
    {
        List<CollisionEvent> collisionEvents = new List<CollisionEvent>();
        
        for (int i = 0; i < rigidbodies.Count - 1; i++)
        {
            for (int j = i + 1; j < rigidbodies.Count; j++)
            {
                //Skip collisions with inactive objects
                if (!rigidbodies[i].IsActive || !rigidbodies[j].IsActive)
                    continue;
                
                //Skip collisions where one or both colliders are disabled
                if (!rigidbodies[i].Collider.IsEnabled || !rigidbodies[j].Collider.IsEnabled)
                    continue;
                
                //Skip collisions between static objects
                if (rigidbodies[i].IsStatic && rigidbodies[j].IsStatic)
                    continue;
                
                //Check AABB overlap and skip if no overlap is found
                if (!Collider.OverlapAABB(rigidbodies[i].Collider, rigidbodies[j].Collider))
                    continue;
                
                //Check precise collision for convex shapes
                bool isColliding = CheckSAT(rigidbodies[i], rigidbodies[j], out Vector collisionNormal, out double penetrationDepth);
                if (!isColliding) continue;
        
                collisionEvents.Add(new CollisionEvent(rigidbodies[i], rigidbodies[j], collisionNormal, penetrationDepth));
            }
        }

        return collisionEvents.ToArray();
    }

    private static bool CheckSAT(RigidBody rigidBodyA, RigidBody rigidBodyB, out Vector collisionNormal, out double penetrationDepth)
    {
        collisionNormal = Vector.Zero;
        penetrationDepth = 0d;
        
        //Circle circle collision
        if (rigidBodyA.Collider is CircleCollider c1 && rigidBodyB.Collider is CircleCollider c2)
            return SATCircleCircle(c1, c2, out collisionNormal, out penetrationDepth);
        
        //Circle polygon collision
        if (rigidBodyA.Collider is CircleCollider circle1 && rigidBodyB.Collider is PolygonCollider poly1)
            return SATCirclePolygon(circle1, poly1, out collisionNormal, out penetrationDepth, false);

        //Circle polygon collision
        if (rigidBodyA.Collider is PolygonCollider poly2 && rigidBodyB.Collider is CircleCollider circle2)
            return SATCirclePolygon(circle2, poly2, out collisionNormal, out penetrationDepth, true);

        //Polygon polygon collision
        if (rigidBodyA.Collider is PolygonCollider p1 && rigidBodyB.Collider is PolygonCollider p2)
            return SATPolygonPolygon(p1, p2, out collisionNormal, out penetrationDepth);
        
        return false;
    }

    #region SAT Checks
    private static bool SATPolygonPolygon(PolygonCollider p1, PolygonCollider p2, out Vector collisionNormal, out double penetrationDepth)
    {
        collisionNormal = Vector.Zero;
        penetrationDepth = double.MaxValue;
        
        List<Vector> axes = new List<Vector>();
        axes.AddRange(p1.Normals);
        axes.AddRange(p2.Normals);
        
        foreach (Vector axis in axes)
        {
            (double min1, double max1) = ProjectPolygonOnAxis(p1, axis);
            (double min2, double max2) = ProjectPolygonOnAxis(p2, axis);
            
            if (max1 < min2 || max2 < min1)
                return false; // Separating axis found, therefor no collision
            
            double axisDepth = Math.Min(max2 - min1, max1 - min2);

            if (axisDepth < penetrationDepth)
            {
                penetrationDepth = axisDepth;
                collisionNormal = axis;
            }
        }

        Vector direction = p2.Position - p1.Position;

        if (Vector.Dot(direction, collisionNormal) < 0f)
        {
            collisionNormal = -collisionNormal;
        }
        return true;
    }

    private static bool SATCirclePolygon(CircleCollider c, PolygonCollider p, out Vector collisionNormal, out double penetrationDepth, bool inverse)
    {
        collisionNormal = Vector.Zero;
        penetrationDepth = double.MaxValue;

        List<Vector> axes = new List<Vector>();
        axes.AddRange(p.Normals);
        Vector closestVertex = ClosestVertexToPoint(p, c.Position);
        axes.Add((c.Position - closestVertex).Normalized());
        
        foreach (Vector axis in axes)
        {
            (double min1, double max1) = ProjectPolygonOnAxis(p, axis);
            (double min2, double max2) = ProjectCircleOnAxis(c, axis);
            
            if (max1 < min2 || max2 < min1)
                return false; // Separating axis found, no collision
            
            double axisDepth = Math.Min(max2 - min1, max1 - min2);
            if (axisDepth < penetrationDepth)
            {
                penetrationDepth = axisDepth;
                collisionNormal = axis;
            }
        }
        
        Vector direction = c.Position - p.Position;
        if (Vector.Dot(collisionNormal, direction) > 0)
        {
            collisionNormal = -collisionNormal;
        }

        if (inverse)
            collisionNormal = -collisionNormal;

        return true;
    }
    
    private static bool SATCircleCircle(CircleCollider c1, CircleCollider c2, out Vector collisionNormal, out double penetrationDepth)
    {
        collisionNormal = Vector.Zero;
        penetrationDepth = 0d;

        double distance = Vector.Distance(c1.Position, c2.Position);
        double radiusSum = c1.Radius + c2.Radius;

        if(distance >= radiusSum)
            return false;

        collisionNormal = (c2.Position - c1.Position).Normalized();
        penetrationDepth = radiusSum - distance;

        return true;
    }
    #endregion


    #region Helper Functions
    private static (double min, double max) ProjectCircleOnAxis(CircleCollider c, Vector axis)
    {
        double centerProjection = Vector.Dot(c.Position, axis);
        return (centerProjection - c.Radius, centerProjection + c.Radius);
    }

    private static (double min, double max) ProjectPolygonOnAxis(PolygonCollider p, Vector axis)
    {
        double min = Vector.Dot(p.Position + p.Vertices[0], axis);
        double max = min;

        for (int i = 1; i < p.Vertices.Length; i++)
        {
            double projection = Vector.Dot(p.Position + p.Vertices[i], axis);
            if (projection < min) min = projection;
            if (projection > max) max = projection;
        }

        return (min, max);
    }
    
    private static Vector ClosestVertexToPoint(PolygonCollider p, Vector point)
    {
        Vector closest = p.Position + p.Vertices[0];
        double minDist = Vector.DistanceSquared(closest, point);

        foreach (Vector vertex in p.Vertices)
        {
            double dist = Vector.DistanceSquared(p.Position + vertex, point);
            if (dist < minDist)
            {
                minDist = dist;
                closest = vertex;
            }
        }
        
        return p.Position + closest;
    }
    #endregion
    
}