using PhysiXSharp.Core.Physics;
using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Utility;

namespace TestRunner;

public class StaticRectangle : Staticbody
{
    public StaticRectangle(Vector position, Vector size)
    {
        Position = position;
        AddCollider(new RectangleCollider(size));
    }
    
    public StaticRectangle(Vector position, Vector size, float rotation)
    {
        Position = position;
        this.Rotation = rotation;
        AddCollider(new RectangleCollider(size));
    }
}