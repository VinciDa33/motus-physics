using System.Diagnostics.Contracts;
using System.Numerics;
using Vector = PhysiXSharp.Core.Utility.Vector;

namespace PhysiXSharp.Core.Physics.Colliders;

public class PolygonCollider : Collider
{
    private readonly Vector[] _baseVertices;
    public Vector[] Vertices { get; private set;  }
    
    internal PolygonCollider(params Vector[] points)
    {
        _baseVertices = new Vector[points.Length];
        Vertices = new Vector[points.Length];
        
        for (int i = 0; i < points.Length; i++)
        {
            _baseVertices[i] = (Vector) points[i].Clone();
            Vertices[i] = (Vector)points[i].Clone();
        }
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
    

    internal override void CalculateNormals()
    {
        Vector[] normals = new Vector[Vertices.Length];

        for (int i = 0; i < Vertices.Length; i++)
        {
            Vector edge = Vertices[(i + 1) % Vertices.Length] - Vertices[i];
            normals[i] = edge.Normal();
        }
        Normals = normals;
    }

    internal override void UpdateRotation()
    {
        double rotation = Rigidbody?.Rotation ?? 0d;
        Vector[] newVertices = new Vector[_baseVertices.Length];
        for (int i = 0; i < _baseVertices.Length; i++)
            newVertices[i] = ((Vector) _baseVertices[i].Clone()).Rotated(rotation);
        
        Vertices = newVertices;
        
        CalculateNormals();
        CalculateAABB();
    }

    public Vector GetPolygonCentroid()
    {
        Vector sum = Vector.Zero;
        foreach (Vector vertex in Vertices) sum += vertex;
        return sum / Vertices.Length;
    }
}