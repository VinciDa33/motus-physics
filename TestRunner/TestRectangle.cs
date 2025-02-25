using PhysiXSharp.Core.Physics;
using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Utility;

namespace TestRunner;

public class TestRectangle : PhysicsObject
{
    public TestRectangle(Vector position, Vector size)
    {
        Position = position;
        AddCollider(new RectangleCollider(size));
    }
}