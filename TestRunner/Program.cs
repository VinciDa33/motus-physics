using MotusPhysics.Core;
using MotusPhysics.Core.Physics;
using MotusPhysics.Core.Physics.Colliders;
using MotusPhysics.Core.Utility;
using MotusPhysics.RayCasting;
using MotusPhysics.Visualizer;

namespace TestRunner;

class Program
{
    static void Main(string[] args)
    {
        Motus.SetModulePath("../../../../TestRunner/bin/Debug/net8.0");
        MotusVisualizer.WindowSize = new Vector(800*1.8, 600*1.8);
        MotusVisualizer.PixelsPerMeter = 40;
        Motus.Initialize();
        //Motus.Time.TimeScale = 1d;
        MotusVisualizer.ShowBoundingBoxes = false;
        MotusVisualizer.ShowCollisionContactPoints = true;
        MotusVisualizer.ShowRigidBodyOrigins = false;
        MotusVisualizer.ShowEdgeNormals = false;
        MotusVisualizer.ShowPhysicsStepCalculationTime = false;
        
        //ChaoticSetup.Setup();
        //StackSetup.Setup();
        //SimpleCollisionSetup.Setup();
        //StressSetup.Setup();
        //RayCastSetup.Setup();

        RigidBody a = RigidBody.CreateStaticBody(Collider.CreateCircleCollider(2), position: new Vector(9, 21));
        RigidBody b = RigidBody.CreateStaticBody(Collider.CreateCircleCollider(2), position: new Vector(18, 21));
        RigidBody c = RigidBody.CreateStaticBody(Collider.CreateRectangleCollider(new Vector(4, 2.5d)), position: new Vector(27, 21), rotation: 50);

        RigidBody d = RigidBody.CreateRigidBody(Collider.CreateCircleCollider(1.5), position: new Vector(9, 10));
        d.SetGravity(new Vector(0, 2));
        d.OnCollisionEnterSubscribe(manifold =>
        {
            d.SetVelocity(Vector.Zero);
            d.SetGravity(new Vector(0, 0.3d));
        });
        
        RigidBody e = RigidBody.CreateRigidBody(Collider.CreatePolygonCollider(new Vector(0, -1.5d), new Vector(1.3d, 1d), new Vector(-1.3d, 1d)), position: new Vector(18, 10));
        e.SetGravity(new Vector(0, 2));
        //e.SetAngularVelocity(-0.5d);
        e.OnCollisionEnterSubscribe(manifold =>
        {
            e.SetVelocity(Vector.Zero);
            e.SetGravity(new Vector(0, 0.3d));
        });
        
        RigidBody f = RigidBody.CreateRigidBody(Collider.CreateRectangleCollider(new Vector(3, 3)), position: new Vector(27, 10), initialAngularVelocity: 1.2d);
        f.SetGravity(new Vector(0, 2));
        f.OnCollisionEnterSubscribe(manifold =>
        {
            f.SetVelocity(Vector.Zero);
            f.SetAngularVelocity(0d);
            f.SetGravity(new Vector(0, 0.3d));
        });
        
        while (MotusVisualizer.IsVisualizerActive())
        {

        }
        
        Motus.Shutdown();
        
        Console.WriteLine("Test-sim ended");
    }
}