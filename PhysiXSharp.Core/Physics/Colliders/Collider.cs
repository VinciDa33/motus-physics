using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Colliders;

public abstract class Collider
{
    private bool _isTrigger = false;
    public PhysicsObject? PhysicsObject { get; private set; }
    public AABB AxisAlignedBoundingBox { get; protected set; }
    
    
    public void SetPhysicsObject(PhysicsObject physicsObject)
    {
        PhysicsObject = physicsObject;
    }

    /// <summary>
    /// Returns an axis-aligned bounding box for this collider.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    internal abstract void CalculateAABB();

    internal virtual void Rotate(float degrees)
    {
        throw new NotImplementedException();
    }

    internal virtual void SetRotation(float degrees)
    {
        throw new NotImplementedException();
    }

    internal abstract (double min, double max) Project(Vector axis);

    internal abstract List<Vector> GetNormals();
}