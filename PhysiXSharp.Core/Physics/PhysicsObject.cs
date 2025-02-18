using PhysiXSharpe.Core.Utility;

namespace PhysiXSharp.Core.Physics;

public abstract class PhysicsObject
{
    public readonly int _id;
    protected Vector _position = new Vector(0d, 0d);
    protected float _rotation = 0f;
    protected Rigidbody _rigidbody;
    protected List<Collider> _colliders;

    public PhysicsObject()
    {
        _id = PhysicsManager.Instance.GetUniquePhysicsObjectId();
        PhysicsManager.Instance.AddPhysicsObject(this);
    }
}