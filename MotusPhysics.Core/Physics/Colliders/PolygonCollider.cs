using Core_Utility_Vector = MotusPhysics.Core.Utility.Vector;
using MotusPhysics_Core_Utility_Vector = MotusPhysics.Core.Utility.Vector;
using Utility_Vector = MotusPhysics.Core.Utility.Vector;
using Vector = MotusPhysics.Core.Utility.Vector;

namespace MotusPhysics.Core.Physics.Colliders;

public class PolygonCollider : Collider
{
    private readonly MotusPhysics_Core_Utility_Vector[] _baseVertices;
    public MotusPhysics_Core_Utility_Vector[] Vertices { get; private set;  }
    
    internal PolygonCollider(params MotusPhysics_Core_Utility_Vector[] points)
    {
        _baseVertices = new MotusPhysics_Core_Utility_Vector[points.Length];
        Vertices = new MotusPhysics_Core_Utility_Vector[points.Length];
        
        for (int i = 0; i < points.Length; i++)
        {
            _baseVertices[i] = (MotusPhysics_Core_Utility_Vector) points[i].Clone();
            Vertices[i] = (MotusPhysics_Core_Utility_Vector)points[i].Clone();
        }
    }

    internal sealed override void CalculateAABB ()
    {
        double smallestX = Vertices[0].x;
        double smallestY = Vertices[0].y;
        double largestX = Vertices[0].x;
        double largestY = Vertices[0].y;

        foreach (MotusPhysics_Core_Utility_Vector vertex in Vertices)
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

        MotusPhysics_Core_Utility_Vector min =  new MotusPhysics_Core_Utility_Vector(smallestX, smallestY);
        MotusPhysics_Core_Utility_Vector max = new MotusPhysics_Core_Utility_Vector(largestX, largestY);

        AABB aabb = new AABB(min, max);
        AxisAlignedBoundingBox = aabb;
    }
    

    internal override void CalculateNormals()
    {
        MotusPhysics_Core_Utility_Vector[] normals = new MotusPhysics_Core_Utility_Vector[Vertices.Length];

        for (int i = 0; i < Vertices.Length; i++)
        {
            MotusPhysics_Core_Utility_Vector edge = Vertices[(i + 1) % Vertices.Length] - Vertices[i];
            normals[i] = edge.Normal();
        }
        Normals = normals;
    }

    internal override void UpdateRotation()
    {
        double rotation = Rigidbody?.Rotation ?? 0d;
        MotusPhysics_Core_Utility_Vector[] newVertices = new MotusPhysics_Core_Utility_Vector[_baseVertices.Length];
        for (int i = 0; i < _baseVertices.Length; i++)
            newVertices[i] = ((MotusPhysics_Core_Utility_Vector) _baseVertices[i].Clone()).Rotated(rotation);
        
        Vertices = newVertices;
        
        CalculateNormals();
        CalculateAABB();
    }
}