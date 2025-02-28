using PhysiXSharp.Core.Physics;
using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Utility;

namespace TestRunner;

public class TestRigid : Rigidbody
{
    public TestRigid(Vector position, Vector size)
    {
        Position = position;
        AddCollider(new RectangleCollider(size));
    }
}