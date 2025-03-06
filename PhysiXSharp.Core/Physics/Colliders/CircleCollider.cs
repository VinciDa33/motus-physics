using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Colliders;

public class CircleCollider : Collider
{
    public double Radius { get; private set; }

    public CircleCollider(double radius)
    {
        Radius = radius;
    }

    internal sealed override void CalculateAABB ()
    {
        Vector origin = new Vector(Position.x - Radius, Position.y - Radius);
        Vector size = new Vector(Radius * 2d, Radius * 2d);

        AxisAlignedBoundingBox = new AABB(origin, size);
    }

    internal override void Rotate(float degrees)
    {
        //No need to rotate a circle
    }

    internal override void SetRotation(float degrees)
    {
        //No need to rotate a circle
    }

    internal override (double min, double max) Project(Vector axis)
    {
        double centerProjection = Vector.Dot(Position, axis);
        return (centerProjection - Radius, centerProjection + Radius);
    }

    internal override void CalculateNormals()
    {
        Normals = new List<Vector>();
    }
}