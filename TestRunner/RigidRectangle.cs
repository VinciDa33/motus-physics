using PhysiXSharp.Core.Physics;
using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Utility;

namespace TestRunner;

public class RigidRectangle : Rigidbody
{
    public RigidRectangle(Vector position, Vector size)
    {
        Position = position;
        AddCollider(new RectangleCollider(size));
    }
}