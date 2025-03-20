using PhysiXSharp.Core;
using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Utility;
using PhysiXSharp.Visualizer;
using SFML.Window;

namespace TestRunner;

class Program
{
    static void Main(string[] args)
    {
        PhysiX.SetModulePath("../../../../TestRunner/bin/Debug/net8.0");
        PhysiX.Initialize();
        //PhysiXVisualizer.ShowPolygonCentroids = true;
        
        
        Rigidbody rbA = Rigidbody.CreateRigidbody(new Vector(8, 4), 0d, Collider.CreateCircleCollider(1d), 5d, 1d, 0.4d);
        rbA.SetVelocity(new Vector(2d, 0d));
        rbA.SetGravity(new Vector(0d, 3d));
        //rbA.OnCollisionEnterSubscribe(manifold => { rbA.SetGravity(new Vector(0d, 4d));});
        
        /*
        rbA.OnCollisionExitSubscribe(manifold =>
        {
            if (manifold.RigidbodyA.Id == rbA.Id)
                manifold.RigidbodyB.Destroy();
            else
                manifold.RigidbodyA.Destroy();
        });*/
        
        
        Rigidbody rbB = Rigidbody.CreateRigidbody(new Vector(24, 4), 0d, Collider.CreateCircleCollider(1d), 1d, 1d, 0.4d);
        rbB.SetVelocity(new Vector(-2d, 0d));
        rbB.SetGravity(new Vector(0d, 3d));
        
        Rigidbody rbC = Rigidbody.CreateRigidbody(new Vector(8, 8), 60d, Collider.CreateRectangleCollider(new Vector(2d, 1d)), 1d, 1d, 0.4d);
        rbC.SetVelocity(new Vector(5d, 0d));
        rbC.SetGravity(new Vector(0d, 3d));
        
        Rigidbody rbD = Rigidbody.CreateRigidbody(new Vector(24, 8), 0d, Collider.CreateCircleCollider(1.2d), 1d, 1d, 0.4d);
        rbD.SetVelocity(new Vector(-1.5d, 0d));
        rbD.SetGravity(new Vector(0d, 3d));
        
        Rigidbody rbE = Rigidbody.CreateRigidbody(new Vector(8, 12), 15d, Collider.CreateRectangleCollider(new Vector(2.5d, 1d)), 1d, 1d, 0.4d);
        rbE.SetVelocity(new Vector(3d, 0d));
        rbE.SetGravity(new Vector(0d, 3d));
        
        Rigidbody rbF = Rigidbody.CreateRigidbody(new Vector(24, 12), 45d, Collider.CreateRectangleCollider(new Vector(1.5, 1.5)), 2d, 1d, 0.4d);
        rbF.SetVelocity(new Vector(-3d, 0d));
        rbF.SetGravity(new Vector(0d, 3d));
        
        Rigidbody rbG = Rigidbody.CreateRigidbody(new Vector(8, 16), 0d, 
            Collider.CreatePolygonCollider(
            new Vector(1, 1),
            new Vector(1, -1),
            new Vector(0, -1.5),
            new Vector(-1, -1),
            new Vector(-1, 1)
            ), 1d, 1d, 0.4d);
        rbG.SetVelocity(new Vector(4d, 0d));
        rbG.SetGravity(new Vector(0d, 3d));
        
        Rigidbody rbH = Rigidbody.CreateRigidbody(new Vector(24, 16), 0d, 
            Collider.CreatePolygonCollider(
            new Vector(1.5, 1.5),
                new Vector(2, -1),
                new Vector(0, -2),
                new Vector(-1.5, -1.25),
                new Vector(-2.25, 1.75)
            ), 1d, 1d, 0.4d);
        rbH.SetVelocity(new Vector(-0.25d, 0d));
        rbH.SetGravity(new Vector(0d, 3d));
        
        
        Rigidbody floor = Rigidbody.CreateStaticbody(new Vector(16, 22), 0d, Collider.CreateRectangleCollider(new Vector(28, 2)));
        
        Rigidbody rbX = Rigidbody.CreateRigidbody(new Vector(16, 0), 0d, Collider.CreateCircleCollider(1d), 1d, 1d, 0.4d);
        //rbX.SetVelocity(new Vector(-2d, 0d));
        rbX.SetGravity(new Vector(0d, 8d));
        
        while (PhysiXVisualizer.IsVisualizerActive())
        {
            //Keep running program until visualizer thread dies
        }
        
        PhysiX.Shutdown();
        
        Console.WriteLine("Test-sim ended");
    }
}