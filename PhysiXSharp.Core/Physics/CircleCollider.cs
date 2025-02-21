using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics;

public class CircleCollider(double radius) : Collider
{
    private double _radius = radius;

    public override AABB GetAABB()
    {
        Vector origin = new Vector(PhysicsObject.Position.x - _radius, this.PhysicsObject.Position.y - _radius);
        Vector size = new Vector(origin.x + _radius * 2, origin.y + _radius * 2);
        return new AABB(origin, size);
    }
}