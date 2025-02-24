using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Colliders;

public class RectangleCollider : Collider
{
    private Vector[] _baseVertices = new Vector[4];
    public Vector[] Vertices { get; private set; } = new Vector[4];

    public RectangleCollider(Vector size)
    {
        _baseVertices[0] = new Vector(-size.x / 2d, -size.y / 2d);
        _baseVertices[1] = new Vector(-size.x / 2d, size.y / 2d);
        _baseVertices[2] = new Vector(size.x / 2d, size.y / 2d);
        _baseVertices[3] = new Vector(size.x / 2d, -size.y / 2d);

        for (int i = 0; i < Vertices.Length; i++)
            Vertices[i] = (Vector) _baseVertices[i].Clone();
    }

    public override AABB GetAABB()
    {
        Vector position = PhysicsObject == null ? new Vector(0, 0) : PhysicsObject.Position;

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

        Vector origin = position + new Vector(smallestX, smallestY);
        Vector size = new Vector(largestX - smallestX, largestY - smallestY);

        return new AABB(origin, size);
    }

    public override void Rotate(float degrees)
    {
        foreach (Vector vertex in Vertices)
        {
            vertex.Rotate(degrees);
        }
    }

    public override void SetRotation(float degrees)
    {
        //Reset vertices
        for (int i = 0; i < Vertices.Length; i++)
            Vertices[i] = (Vector) _baseVertices[i].Clone();
        
        Rotate(degrees);
    }
}