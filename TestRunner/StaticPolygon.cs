using PhysiXSharp.Core.Physics;
using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Utility;

namespace TestRunner;

public class StaticPolygon : Staticbody
{
    public StaticPolygon(Vector position, Vector[] vertices)
    {
        Position = position;
        AddCollider(new PolygonCollider(vertices));
    }
    
    public StaticPolygon(Vector position, Vector[] vertices, float rotation)
    {
        Position = position;
        this.Rotation = rotation;
        AddCollider(new PolygonCollider(vertices));
    }
}