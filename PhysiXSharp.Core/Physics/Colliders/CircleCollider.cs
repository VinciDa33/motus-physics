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
        Vector position = PhysicsObject == null ? new Vector(0, 0) : PhysicsObject.Position;
        
        Vector origin = new Vector(position.x - Radius, position.y - Radius);
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
        Vector position = PhysicsObject == null ? new Vector(0, 0) : PhysicsObject.Position;
        double centerProjection = Vector.Dot(position, axis);
        return (centerProjection - Radius, centerProjection + Radius);
    }

    internal override List<Vector> GetNormals()
    {
        return new List<Vector>();
    }
}