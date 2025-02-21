using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics;

public abstract class PhysicsObject
{
    public readonly int Id;
    public bool IsActive { get; protected set; } = true;
    public Vector Position { get; protected set; } = new Vector(0d, 0d);
    public float Rotation { get; protected set; } = 0f;
    private Collider? _collider;
    
    public PhysicsObject()
    {
        Id = PhysicsManager.Instance.GetUniquePhysicsObjectId();
        PhysicsManager.Instance.AddPhysicsObject(this);
    }

    public void SetActive(bool b)
    {
        IsActive = b;
    }

    public void AddCollider(Collider collider)
    {
        _collider = collider;
        _collider.SetPhysicsObject(this);
    }

    public void Destroy()
    {
        PhysicsManager.Instance.RemovePhysicsObject(this);
    }
}