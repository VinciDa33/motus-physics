using PhysiXSharp.Core.Physics;
using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Utility;

namespace TestRunner;

public class TestCircle : Rigidbody
{
    public TestCircle(Vector position, double radius)
    {
        Position = position;
        AddCollider(new CircleCollider(radius));
    }
}