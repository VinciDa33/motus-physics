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
        //Motus.SetPhysicsUpdateRate(50);
        //Motus.Time.TimeScale = 1d;
        //MotusVisualizer.ShowBoundingBoxes = true;
        MotusVisualizer.ShowCollisionContactPoints = false;
        MotusVisualizer.ShowRigidbodyOrigins = false;
        //MotusVisualizer.ShowEdgeNormals = true;
        MotusVisualizer.ShowPhysicsStepCalculationTime = false;
        
        ChaoticSetup.Setup();
        //StackSetup.Setup();
        //SimpleCollisionSetup.Setup();
        //StressSetup.Setup();
        //RayCastSetup.Setup();

        Vector toCast = new Vector(0, -1);
        int nextStep = 10;
        
        while (MotusVisualizer.IsVisualizerActive())
        {
            /*
            if (Motus.Time.SimStep >= nextStep)
            {
                Ray ray = new Ray(new Vector(10, 8), (Vector)toCast.Clone());
                MotusRaycast.CastAll(ray, out RayCastHit[] hits);
                
                Collider c = Collider.CreateCircleCollider(0.1d);
                c.SetRayCastTarget(false);
                RigidBody.CreateStaticBody(c, ray.Origin + ray.Direction * 1.5d);
                
                foreach (RayCastHit hit in hits)
                {
                    Collider c2 = Collider.CreateCircleCollider(0.1d);
                    c2.SetRayCastTarget(false);
                    RigidBody.CreateStaticBody(c2, position: hit.Point);
                    Console.WriteLine(hit);
                }

                toCast.Rotate(-5d);
                nextStep += 10;
            }
            */
        }
        
        Motus.Shutdown();
        
        Console.WriteLine("Test-sim ended");
    }
}