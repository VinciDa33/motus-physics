using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics;

public abstract class PhysicsObject
{
    public readonly int Id;
    protected Vector Position = new Vector(0d, 0d);
    protected float Rotation = 0f;
    protected List<Collider> Colliders = new List<Collider>();

    protected PhysicsObject()
    {
        Id = PhysicsManager.Instance.GetUniquePhysicsObjectId();
        PhysicsManager.Instance.AddPhysicsObject(this);
    }
}