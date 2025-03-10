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
        Vector min = new Vector(-Radius,-Radius);
        Vector max = new Vector(Radius, Radius);

        AABB aabb = new AABB(min, max);
        AxisAlignedBoundingBox = aabb;
    }

    internal override void Rotate(float degrees)
    {
        //No need to rotate a circle
    }

    internal override void SetRotation(float degrees)
    {
        //No need to rotate a circle
    }

    internal override void CalculateNormals()
    {
        Normals = new List<Vector>();
    }
}