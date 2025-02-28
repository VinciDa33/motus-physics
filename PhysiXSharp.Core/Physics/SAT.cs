using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics;

public class SAT
{
    public static bool DoCollision(PhysicsObject po1, PhysicsObject po2)
    {
        if (po1.Collider == null || po2.Collider == null)
            return false;

        List<Vector> axes = new List<Vector>();
        axes.AddRange(po1.Collider.GetNormals());
        axes.AddRange(po2.Collider.GetNormals());
        
        if (po1.Collider is CircleCollider && po2.Collider is PolygonCollider p2)
        {
            Vector closestVertex = p2.ClosestVertexToPoint(po1.Position);
            axes.Add((closestVertex - po1.Position).Normalized());
        }
        if (po1.Collider is PolygonCollider p1 && po2.Collider is CircleCollider)
        {
            Vector closestVertex = p1.ClosestVertexToPoint(po2.Position);
            axes.Add((closestVertex - po2.Position).Normalized());
        }

        if (po1.Collider is CircleCollider c1 && po2.Collider is CircleCollider c2)
        {
            double distanceSquared = Vector.DistanceSquared(po1.Position, po2.Position);
            double radiusSum = c1.Radius + c2.Radius;
            return distanceSquared <= radiusSum * radiusSum;
        }
        
        foreach (Vector axis in axes)
        {
            (double min1, double max1) = po1.Collider.Project(axis);
            (double min2, double max2) = po2.Collider.Project(axis);
            
            if (max1 < min2 || max2 < min1)
                return false; // Separating axis found, no collision
        }

        return true; // No separating axis found, collision detected
    }
}