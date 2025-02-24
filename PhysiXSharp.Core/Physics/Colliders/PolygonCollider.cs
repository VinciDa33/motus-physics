using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Colliders;

public class PolygonCollider : Collider
{
    private List<Vector> _baseVertices = new List<Vector>();
    private List<Vector> _vertices = new List<Vector>();

    public PolygonCollider(Vector size)
    { 
        
    }

    public override AABB GetAABB()
    {
        return base.GetAABB();
    }
}