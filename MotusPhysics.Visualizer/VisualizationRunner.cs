using MotusPhysics.Core;
using MotusPhysics.Core.Physics;
using MotusPhysics.Core.Physics.Colliders;
using MotusPhysics.Core.Physics.Data;
using MotusPhysics.Core.Utility;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MotusPhysics.Visualizer;

internal class VisualizationRunner
{
    internal readonly List<Shape> _shapesToRender = new List<Shape>();
    internal readonly List<Vertex[]> _lineShapesToRender = new List<Vertex[]>();
    internal readonly List<Vertex[]> _linesToRender = new List<Vertex[]>();
    internal bool Shutdown = false;
    
    public void RunVisualization()
    {
        VideoMode videoMode = new VideoMode((uint)MotusVisualizer.WindowSize.x, (uint)MotusVisualizer.WindowSize.y);
        RenderWindow window = new RenderWindow(videoMode, "Motus Visualizer");
        window.Closed += (sender, e) => window.Close();
        
        Font font = ResourceLoader.LoadEmbeddedFont();
        Text text = new Text("0", font, 18) { FillColor = Color.White, Position = new Vector2f(5, 5)};
        
        // Main loop
        while (window.IsOpen)
        {
            window.DispatchEvents();
            window.Clear(new Color(30, 30, 30));

            if (MotusVisualizer.ShowPhysicsStepCalculationTime)
            {
                text.DisplayedString = Motus.Time.LastStepMilliseconds + " millis";
                window.Draw(text);
            }

            GenerateShapes();

            foreach (Shape shape in _shapesToRender)
                window.Draw(shape);

            foreach (Vertex[] lineShape in _lineShapesToRender)
                window.Draw(lineShape, PrimitiveType.LineStrip);

            foreach (Vertex[] line in _linesToRender)
                window.Draw(line, PrimitiveType.Lines);

            window.Display();
            
            if (Shutdown)
                break;
        }
    }

    private void GenerateShapes()
    {
        _shapesToRender.Clear();
        _lineShapesToRender.Clear();
        _linesToRender.Clear();
        
        PhysicsManager physicsManager = PhysicsManager.Instance;
        List<RigidBody> rigidbodies = physicsManager.GetRigidbodies();
        
        if (MotusVisualizer.ShowCollisionShapes)
            GenerateCollisionShapes(rigidbodies);
        if (MotusVisualizer.ShowBoundingBoxes)
            GenerateAABBShapes(rigidbodies);
        if (MotusVisualizer.ShowRigidbodyOrigins)
            GeneratePhysicsOrigins(rigidbodies);
        if (MotusVisualizer.ShowEdgeNormals)
            GenerateNormals(rigidbodies);
        if (MotusVisualizer.ShowCollisionContactPoints)
            GenerateContactPoints(physicsManager.Manifolds);
    }

    private void GenerateCollisionShapes(List<RigidBody> rigidbodies)
    {
        foreach (RigidBody rigidbody in rigidbodies)
        {
            if (rigidbody.Collider is CircleCollider circleCollider)
            {
                float radius = (float) circleCollider.Radius;
                _shapesToRender.Add(new CircleShape(radius * MotusVisualizer.PixelsPerMeter)
                {
                    FillColor = new Color(0, 0, 0, 0),
                    OutlineColor = Color.Red,
                    OutlineThickness = 1,
                    Position = new Vector2f((float) (rigidbody.Position.x - radius), (float) (rigidbody.Position.y - radius)) * MotusVisualizer.PixelsPerMeter
                });
            }

            if (rigidbody.Collider is PolygonCollider polygonCollider)
            {
                Vector[] vertices = polygonCollider.Vertices;

                Vertex[] shape = new Vertex[vertices.Length + 1];
                for (int i = 0; i < vertices.Length; i++)
                {
                    shape[i] = new Vertex(new Vector2f((float)(rigidbody.Position.x + vertices[i].x), (float)(rigidbody.Position.y + vertices[i].y)) * MotusVisualizer.PixelsPerMeter, Color.Red);
                }
                shape[^1] = new Vertex(new Vector2f((float)(rigidbody.Position.x + vertices[0].x), (float)(rigidbody.Position.y + vertices[0].y)) * MotusVisualizer.PixelsPerMeter, Color.Red);
                
                _lineShapesToRender.Add(shape);
            }
        }
    }
    
    private void GenerateAABBShapes(List<RigidBody> rigidbodies)
    {
        foreach (RigidBody rigidbody in rigidbodies)
        {
            AABB aabb = rigidbody.Collider.AxisAlignedBoundingBox; 
            Vector size = new Vector(aabb.Max.x - aabb.Min.x, aabb.Max.y - aabb.Min.y) * MotusVisualizer.PixelsPerMeter;
            _shapesToRender.Add(new RectangleShape(new Vector2f((float) size.x, (float) size.y))
            {
                FillColor = new Color(0, 0, 0, 0),
                OutlineColor = Color.Magenta,
                OutlineThickness = 1,
                Position = new Vector2f((float) (rigidbody.Position.x + aabb.Min.x), (float) (rigidbody.Position.y + aabb.Min.y)) * MotusVisualizer.PixelsPerMeter
            });
        }
    }
    
    private void GeneratePhysicsOrigins(List<RigidBody> rigidbodies)
    {
        foreach (RigidBody rigidbody in rigidbodies)
        {
            _shapesToRender.Add(new CircleShape(2f)
            {
                FillColor = Color.Cyan,
                Position = new Vector2f((float) rigidbody.Position.x, (float) rigidbody.Position.y) * MotusVisualizer.PixelsPerMeter - new Vector2f(1, 1)
            });
        }
    }

    private void GenerateNormals(List<RigidBody> rigidbodies)
    {
        foreach (RigidBody rigidbody in rigidbodies)
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
                    line[0] = new Vertex(new Vector2f((float)point.x, (float)point.y) * MotusVisualizer.PixelsPerMeter, Color.Green);
                    line[1] = new Vertex(new Vector2f((float)(point.x + normals[i].x), (float)(point.y + normals[i].y)) * MotusVisualizer.PixelsPerMeter, Color.Green);
                    _linesToRender.Add(line);
                }
            }
        }
    }

    private void GenerateContactPoints(List<CollisionManifold> manifolds)
    {
        foreach (CollisionManifold manifold in manifolds)
        {
            foreach (Vector contactPoint in manifold.ContactPoints)
            {
                _shapesToRender.Add(new CircleShape(2f)
                {
                    FillColor = Color.Yellow,
                    Position = new Vector2f((float) contactPoint.x, (float) contactPoint.y) * MotusVisualizer.PixelsPerMeter - new Vector2f(1, 1)
                });
            }
        }
    }
}