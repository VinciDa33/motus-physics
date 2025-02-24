namespace PhysiXSharp.Core.Physics.Colliders;

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

    public virtual void Rotate(float degrees)
    {
        throw new NotImplementedException();
    }

    public virtual void SetRotation(float degrees)
    {
        throw new NotImplementedException();
    }
}