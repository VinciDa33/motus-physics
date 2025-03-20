using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Colliders;

public class CircleCollider : Collider
{
    public double Radius { get; private set; }

    internal CircleCollider(double radius)
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
    

    internal override void CalculateNormals()
    {
        //No need to calculate normals for a circle
    }

    internal override void UpdateRotation()
    {
        //No need to rotate a circle
    }
}