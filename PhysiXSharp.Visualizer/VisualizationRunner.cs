

using PhysiXSharp.Core.Physics;
using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Physics.Data;
using PhysiXSharp.Core.Utility;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace PhysiXSharp.Visualizer;

internal class VisualizationRunner(PhysicsManager physicsManager)
{
    private readonly PhysicsManager _physicsManager = physicsManager;

    private readonly List<Shape> _shapesToRender = new List<Shape>();
    private readonly List<Vertex[]> _lineShapesToRender = new List<Vertex[]>();
    private readonly List<Vertex[]> _linesToRender = new List<Vertex[]>();
    
    public void RunVisualization()
    {
        RenderWindow window = new RenderWindow(new VideoMode(800, 600), "PhysiXSharp Visualizer");
        window.Closed += (sender, e) => window.Close();
        
        
        // Main loop
        while (window.IsOpen)
        {
            window.DispatchEvents();
            window.Clear(new Color(30, 30, 30));

            GenerateShapes();
            
            foreach (Shape shape in _shapesToRender)
                window.Draw(shape);
            
            foreach (Vertex[] lineShape in _lineShapesToRender)
                window.Draw(lineShape, PrimitiveType.LineStrip);
            
            foreach(Vertex[] line in _linesToRender)
                window.Draw(line, PrimitiveType.Lines);

            window.Display();
        }
        
    }

    private void GenerateShapes()
    {
        _shapesToRender.Clear();
        _lineShapesToRender.Clear();
        _linesToRender.Clear();
        
        List<Rigidbody> rigidbodies = _physicsManager.GetRigidbodies();
        List<CollisionManifold> manifolds = _physicsManager.Manifolds;
        
        if (PhysiXVisualizer.ShowCollisionShapes)
            GenerateCollisionShapes(rigidbodies);
        if (PhysiXVisualizer.ShowBoundingBoxes)
            GenerateAABBShapes(rigidbodies);
        if (PhysiXVisualizer.ShowRigidbodyOrigins)
            GeneratePhysicsOrigins(rigidbodies);
        if (PhysiXVisualizer.ShowEdgeNormals)
            GenerateNormals(rigidbodies);
        if (PhysiXVisualizer.ShowCollisionContactPoints)
            GenerateContactPoints(_physicsManager.Manifolds);
        if (PhysiXVisualizer.ShowPolygonCentroids)
            GeneratePolygonCentroid(rigidbodies);
    }

    private void GenerateCollisionShapes(List<Rigidbody> rigidbodies)
    {
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            if (rigidbody.Collider is CircleCollider circleCollider)
            {
                float radius = (float) circleCollider.Radius;
                _shapesToRender.Add(new CircleShape(radius * PhysiXVisualizer.PixelsPerMeter)
                {
                    FillColor = new Color(0, 0, 0, 0),
                    OutlineColor = Color.Red,
                    OutlineThickness = 1,
                    Position = new Vector2f((float) (rigidbody.Position.x - radius), (float) (rigidbody.Position.y - radius)) * PhysiXVisualizer.PixelsPerMeter
                });
            }

            if (rigidbody.Collider is PolygonCollider polygonCollider)
            {
                Vector[] vertices = polygonCollider.Vertices;

                Vertex[] shape = new Vertex[vertices.Length + 1];
                for (int i = 0; i < vertices.Length; i++)
                {
                    shape[i] = new Vertex(new Vector2f((float)(rigidbody.Position.x + vertices[i].x), (float)(rigidbody.Position.y + vertices[i].y)) * PhysiXVisualizer.PixelsPerMeter, Color.Red);
                }
                shape[^1] = new Vertex(new Vector2f((float)(rigidbody.Position.x + vertices[0].x), (float)(rigidbody.Position.y + vertices[0].y)) * PhysiXVisualizer.PixelsPerMeter, Color.Red);
                
                _lineShapesToRender.Add(shape);
            }
        }
    }
    
    private void GenerateAABBShapes(List<Rigidbody> rigidbodies)
    {
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            AABB aabb = rigidbody.Collider.AxisAlignedBoundingBox; 
            Vector size = new Vector(aabb.Max.x - aabb.Min.x, aabb.Max.y - aabb.Min.y) * PhysiXVisualizer.PixelsPerMeter;
            _shapesToRender.Add(new RectangleShape(new Vector2f((float) size.x, (float) size.y))
            {
                FillColor = new Color(0, 0, 0, 0),
                OutlineColor = Color.Magenta,
                OutlineThickness = 1,
                Position = new Vector2f((float) (rigidbody.Position.x + aabb.Min.x), (float) (rigidbody.Position.y + aabb.Min.y)) * PhysiXVisualizer.PixelsPerMeter
            });
        }
    }
    
    private void GeneratePhysicsOrigins(List<Rigidbody> rigidbodies)
    {
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            _shapesToRender.Add(new CircleShape(2f)
            {
                FillColor = Color.Cyan,
                Position = new Vector2f((float) rigidbody.Position.x, (float) rigidbody.Position.y) * PhysiXVisualizer.PixelsPerMeter - new Vector2f(1, 1)
            });
        }
    }

    private void GenerateNormals(List<Rigidbody> rigidbodies)
    {
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            if (rigidbody.Collider is PolygonCollider polygonCollider)
            {
                Vector[] vertices = polygonCollider.Vertices;
                
                List<Vector> normals = new List<Vector>(polygonCollider.Normals);
                for (int i = 0; i < normals.Count; i++)
                {
                    Vector vertex1 = vertices[i];
                    Vector vertex2 = i + 1 >= vertices.Length ? vertices[0] : vertices[i + 1];
                    Vector point = polygonCollider.Position + vertex1 + (vertex2 - vertex1) * 0.5d;
                    
                    Vertex[] line = new Vertex[2];
                    line[0] = new Vertex(new Vector2f((float)point.x, (float)point.y) * PhysiXVisualizer.PixelsPerMeter, Color.Green);
                    line[1] = new Vertex(new Vector2f((float)(point.x + normals[i].x), (float)(point.y + normals[i].y)) * PhysiXVisualizer.PixelsPerMeter, Color.Green);
                    _linesToRender.Add(line);
                }
            }
        }
    }

    public void GenerateContactPoints(List<CollisionManifold> manifolds)
    {
        foreach (CollisionManifold manifold in manifolds)
        {
            foreach (Vector contactPoint in manifold.ContactPoints)
            {
                _shapesToRender.Add(new CircleShape(2f)
                {
                    FillColor = Color.Yellow,
                    Position = new Vector2f((float) contactPoint.x, (float) contactPoint.y) * PhysiXVisualizer.PixelsPerMeter - new Vector2f(1, 1)
                });
            }
        }
    }

    public void GeneratePolygonCentroid(List<Rigidbody> rigidbodies)
    {
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            if (rigidbody.Collider is PolygonCollider pc)
            {
                Vector centroid = pc.GetPolygonCentroid();
                _shapesToRender.Add(new CircleShape(2f)
                {
                    FillColor = new Color(200, 140, 200),
                    Position = new Vector2f((float) (pc.Position.x + centroid.x), (float) (pc.Position.y + centroid.y)) * PhysiXVisualizer.PixelsPerMeter - new Vector2f(1, 1)
                });
            }
        }
    }
}