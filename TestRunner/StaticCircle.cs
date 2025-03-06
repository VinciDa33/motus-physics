using PhysiXSharp.Core.Physics;
using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Utility;

namespace TestRunner;

public class StaticCircle : Staticbody
{
    public StaticCircle(Vector position, double radius)
    {
        Position = position;
        AddCollider(new CircleCollider(radius));
    }
}