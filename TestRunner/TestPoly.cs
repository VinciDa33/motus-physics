using PhysiXSharp.Core.Physics;
using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Utility;

namespace TestRunner;

public class TestPoly : Rigidbody
{
    public TestPoly(Vector position, Vector[] points)
    {
        Position = position;
        AddCollider(new PolygonCollider(points));
    }
}