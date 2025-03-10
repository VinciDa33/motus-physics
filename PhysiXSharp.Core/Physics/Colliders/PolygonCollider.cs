using System.Diagnostics.Contracts;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Colliders;

public class PolygonCollider : Collider
{
    private List<Vector> _baseVertices;
    public List<Vector> Vertices { get; private set;  } = new List<Vector>();
    
    public PolygonCollider(params Vector[] points)
    {
        if (points.Length < 3)
            PhysiX.Logger.LogError("Polygon collider must have 3 or more points!");
        
        _baseVertices = points.ToList();
        foreach(Vector vertex in _baseVertices)
            Vertices.Add((Vector) vertex.Clone());
    }

    internal sealed override void CalculateAABB ()
    {
        double smallestX = Vertices[0].x;
        double smallestY = Vertices[0].y;
        double largestX = Vertices[0].x;
        double largestY = Vertices[0].y;

        foreach (Vector vertex in Vertices)
        {
            if (vertex.x < smallestX)
                smallestX = vertex.x;
            if (vertex.y < smallestY)
                smallestY = vertex.y;
            if (vertex.x > largestX)
                largestX = vertex.x;
            if (vertex.y > largestY)
                largestY = vertex.y;
        }

        Vector min =  new Vector(smallestX, smallestY);
        Vector max = new Vector(largestX, largestY);

        AABB aabb = new AABB(min, max);
        AxisAlignedBoundingBox = aabb;
    }
    
    internal override void Rotate(float degrees)
    {
        List<Vector> newVertices = new List<Vector>();
        foreach (Vector vertex in Vertices)
        {
            newVertices.Add(((Vector)vertex.Clone()).Rotated(degrees));
        }

        Vertices = newVertices;
        
        CalculateAABB();
        CalculateNormals();
    }

    internal override void SetRotation(float degrees)
    {
        //Reset vertices
        List<Vector> newVertices = new List<Vector>();
        foreach(Vector vertex in _baseVertices)
            newVertices.Add((Vector) vertex.Clone());

        Vertices = newVertices;
        
        Rotate(degrees);
    }

    internal override void CalculateNormals()
    {
        List<Vector> normals = new List<Vector>();

        for (int i = 0; i < Vertices.Count; i++)
        {
            Vector edge = Vertices[(i + 1) % Vertices.Count] - Vertices[i];
            normals.Add(edge.Normal());
        }
        Normals = normals;
    }
    
    public Vector GetPolygonCentroid()
    {
        Vector sum = Vector.Zero;
        foreach (Vector vertex in Vertices) sum += vertex;
        return sum / Vertices.Count;
    }
}