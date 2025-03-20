using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Colliders;

public class RectangleCollider : PolygonCollider
{
    internal RectangleCollider(Vector size) : base(CreateRectangle(size))
    {
        
    }
    
    private static Vector[] CreateRectangle(Vector size)
    {
        Vector[] points = new Vector[4];
        
        points[0] = new Vector(-size.x / 2d, -size.y / 2d);
        points[1] = new Vector(-size.x / 2d, size.y / 2d);
        points[2] = new Vector(size.x / 2d, size.y / 2d);
        points[3] = new Vector(size.x / 2d, -size.y / 2d);

        return points;
    }
}