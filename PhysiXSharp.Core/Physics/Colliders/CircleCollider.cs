using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Colliders;

public class CircleCollider(double radius) : Collider
{
    public double Radius { get; private set; } = radius;
    
    public override AABB GetAABB()
    {
        Vector position = PhysicsObject == null ? new Vector(0, 0) : PhysicsObject.Position;
        
        Vector origin = new Vector(position.x - Radius, position.y - Radius);
        Vector size = new Vector(Radius * 2d, Radius * 2d);
        return new AABB(origin, size);
    }

    public override void Rotate(float degrees)
    {
        //No need to rotate a circle
    }

    public override void SetRotation(float degrees)
    {
        //No need to rotate a circle
    }
}