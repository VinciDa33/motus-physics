using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Bodies;

public abstract class PhysicsObject
{
    public readonly int Id;
    public bool IsActive { get; protected set; } = true;
    public bool IsStatic { get; protected set; }
    public Vector Position { get; protected set; } = new Vector(0d, 0d);
    public float Rotation { get; protected set; } = 0f;
    public Collider? Collider { get; private set; }
    
    
    protected PhysicsObject()
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
        Collider = collider;
        Collider.SetPhysicsObject(this);
        if (Rotation != 0)
            Collider.SetRotation(Rotation);
    }

    public void RemoveCollider()
    {
        Collider = null;
    }
    
    public void Destroy()
    {
        PhysicsManager.Instance.RemovePhysicsObject(this);
    }
}