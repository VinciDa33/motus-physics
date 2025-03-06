using PhysiXSharp.Core.Physics;
using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Utility;

namespace TestRunner;

public class RigidPolygon : Rigidbody
{
    public RigidPolygon(Vector position, Vector[] vertices)
    {
        Position = position;
        AddCollider(new PolygonCollider(vertices));
    }
}