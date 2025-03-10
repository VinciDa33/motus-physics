

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
        
        List<PhysicsObject> physicsObjects =  _physicsManager.GetPhysicsObjects();
        List<CollisionManifold> manifolds = _physicsManager.Manifolds;
        
        if (PhysiXVisualizer.ShowCollisionShapes)
            GenerateCollisionShapes(physicsObjects);
        if (PhysiXVisualizer.ShowBoundingBoxes)
            GenerateAABBShapes(physicsObjects);
        if (PhysiXVisualizer.ShowPhysicsObjectOrigins)
            GeneratePhysicsOrigins(physicsObjects);
        if (PhysiXVisualizer.ShowEdgeNormals)
            GenerateNormals(physicsObjects);
        if (PhysiXVisualizer.ShowCollisionContactPoints)
            GenerateContactPoints(_physicsManager.Manifolds);
    }

    private void GenerateCollisionShapes(List<PhysicsObject> physicsObjects)
    {
        foreach (PhysicsObject po in physicsObjects)
        {
            if (po.Collider == null)
                continue;

            if (po.Collider.GetType() == typeof(CircleCollider))
            {
                float radius = (float)((CircleCollider)po.Collider).Radius;
                _shapesToRender.Add(new CircleShape(radius)
                {
                    FillColor = new Color(0, 0, 0, 0),
                    OutlineColor = Color.Red,
                    OutlineThickness = 1,
                    Position = new Vector2f((float) po.Position.x - radius, (float) po.Position.y - radius)
                });
            }

            if (po.Collider is PolygonCollider collider)
            {
                List<Vector> vertices = collider.Vertices;

                Vertex[] shape = new Vertex[vertices.Count + 1];
                for (int i = 0; i < vertices.Count; i++)
                {
                    shape[i] = new Vertex(new Vector2f((float)(po.Position.x + vertices[i].x), (float)(po.Position.y + vertices[i].y)), Color.Red);
                }
                shape[^1] = new Vertex(new Vector2f((float)(po.Position.x + vertices[0].x), (float)(po.Position.y + vertices[0].y)), Color.Red);
                
                _lineShapesToRender.Add(shape);
            }
        }
    }
    
    private void GenerateAABBShapes(List<PhysicsObject> physicsObjects)
    {
        foreach (PhysicsObject po in physicsObjects)
        {
            if (po.Collider == null)
                continue;

            AABB aabb = po.Collider.AxisAlignedBoundingBox; 
            Vector size = new Vector(aabb.Max.x - aabb.Min.x, aabb.Max.y - aabb.Min.y);
            _shapesToRender.Add(new RectangleShape(new Vector2f((float) size.x, (float) size.y))
            {
                FillColor = new Color(0, 0, 0, 0),
                OutlineColor = Color.Magenta,
                OutlineThickness = 1,
                Position = new Vector2f((float) (po.Position.x + aabb.Min.x), (float) (po.Position.y + aabb.Min.y))
            });
        }
    }
    
    private void GeneratePhysicsOrigins(List<PhysicsObject> physicsObjects)
    {
        foreach (PhysicsObject po in physicsObjects)
        {
            _shapesToRender.Add(new CircleShape(2f)
            {
                FillColor = Color.Cyan,
                Position = new Vector2f((float) po.Position.x - 1f, (float) po.Position.y - 1f)
            });
        }
    }

    private void GenerateNormals(List<PhysicsObject> physicsObjects)
    {
        foreach (PhysicsObject po in physicsObjects)
        {
            if (po.Collider is PolygonCollider pc)
            {
                List<Vector> normals = new List<Vector>(pc.Normals);
                for (int i = 0; i < normals.Count; i++)
                {
                    Vector vertex1 = pc.Vertices[i];
                    Vector vertex2 = i + 1 >= pc.Vertices.Count ? pc.Vertices[0] : pc.Vertices[i + 1];
                    Vector point = pc.Position + vertex1 + (vertex2 - vertex1) * 0.5d;
                    
                    Vertex[] line = new Vertex[2];
                    line[0] = new Vertex(new Vector2f((float)point.x, (float)point.y), Color.Green);
                    line[1] = new Vertex(new Vector2f((float)(point.x + normals[i].x * 12d), (float)(point.y + normals[i].y * 12d)), Color.Green);
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
                    Position = new Vector2f((float) contactPoint.x - 1f, (float) contactPoint.y - 1f)
                });
            }
        }
    }
}