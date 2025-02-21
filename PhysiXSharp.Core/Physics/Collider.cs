namespace PhysiXSharp.Core.Physics;

public abstract class Collider
{
    private bool _isTrigger = false;
    public PhysicsObject? PhysicsObject { get; private set; }

    public void SetPhysicsObject(PhysicsObject physicsObject)
    {
        PhysicsObject = physicsObject;
    }
    
    /// <summary>
    /// Returns an axis-aligned bounding box for this collider.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public virtual AABB GetAABB()
    {
        throw new NotImplementedException();
    }
}