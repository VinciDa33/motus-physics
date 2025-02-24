

using PhysiXSharp.Core.Physics;
using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Utility;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace PhysiXSharp.Visualizer;

internal class VisualizationRunner(PhysicsManager physicsManager)
{
    private PhysicsManager _physicsManager = physicsManager;

    List<Shape> shapesToRender = new List<Shape>();
    List<Vertex[]> lineShapesToRender = new List<Vertex[]>();
    private List<Vertex[]> linesToRender = new List<Vertex[]>();
    
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
            
            foreach (Shape shape in shapesToRender)
                window.Draw(shape);
            
            foreach (Vertex[] lineShape in lineShapesToRender)
                window.Draw(lineShape, PrimitiveType.LineStrip);

            window.Display();
        }
        
    }

    private void GenerateShapes()
    {
        shapesToRender.Clear();
        lineShapesToRender.Clear();
        linesToRender.Clear();
        
        List<PhysicsObject> physicsObjects =  _physicsManager.GetPhysicsObjects();

        GenerateCollisionShapes(physicsObjects);
        GenerateAABBShapes(physicsObjects);
        GeneratePhysicsPointers(physicsObjects);
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
                shapesToRender.Add(new CircleShape(radius)
                {
                    FillColor = new Color(0, 0, 0, 0),
                    OutlineColor = Color.Red,
                    OutlineThickness = 1,
                    Position = new Vector2f((float) po.Position.x - radius, (float) po.Position.y - radius)
                });
            }

            if (po.Collider.GetType() == typeof(RectangleCollider))
            {
                Vector[] vertices = ((RectangleCollider)po.Collider).Vertices;
                Vertex[] rectangle =
                {
                    new Vertex(new Vector2f((float) (po.Position.x + vertices[0].x), (float) (po.Position.y + vertices[0].y)), Color.Red),
                    new Vertex(new Vector2f((float) (po.Position.x + vertices[1].x), (float) (po.Position.y + vertices[1].y)), Color.Red),
                    new Vertex(new Vector2f((float) (po.Position.x + vertices[2].x), (float) (po.Position.y + vertices[2].y)), Color.Red),
                    new Vertex(new Vector2f((float) (po.Position.x + vertices[3].x), (float) (po.Position.y + vertices[3].y)), Color.Red),
                    new Vertex(new Vector2f((float) (po.Position.x + vertices[0].x), (float) (po.Position.y + vertices[0].y)), Color.Red),
                };
                lineShapesToRender.Add(rectangle);
            }
        }
    }
    
    private void GenerateAABBShapes(List<PhysicsObject> physicsObjects)
    {
        foreach (PhysicsObject po in physicsObjects)
        {
            if (po.Collider == null)
                continue;

            AABB aabb = po.Collider.GetAABB();
            shapesToRender.Add(new RectangleShape(new Vector2f((float) aabb.Size.x, (float) aabb.Size.y))
            {
                FillColor = new Color(0, 0, 0, 0),
                OutlineColor = Color.Magenta,
                OutlineThickness = 1,
                Position = new Vector2f((float) aabb.Origin.x, (float) aabb.Origin.y)
            });
        }
    }
    
    private void GeneratePhysicsPointers(List<PhysicsObject> physicsObjects)
    {
        foreach (PhysicsObject po in physicsObjects)
        {
            shapesToRender.Add(new CircleShape(2f)
            {
                FillColor = Color.Cyan,
                Position = new Vector2f((float) po.Position.x - 1f, (float) po.Position.y - 1f)
            });
        }
    }
}