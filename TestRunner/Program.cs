using PhysiXSharp.Core;
using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Utility;
using PhysiXSharp.Visualizer;

namespace TestRunner;

class Program
{
    static void Main(string[] args)
    {
        PhysiX.SetModulePath("../../../../TestRunner/bin/Debug/net8.0");
        PhysiX.Initialize();
        PhysiXVisualizer.ShowPhysicsObjectOrigins = true;
        
        /*
        Staticbody sb1 = new Staticbody(new Vector(200, 500), 40f);
        sb1.AddCollider(new CircleCollider(50));
        sb1.Restitution = 0.5d;
        
        Staticbody sb2 = new Staticbody(new Vector(300, 500), 40f);
        sb2.AddCollider(new CircleCollider(25));
        
        Staticbody sb3 = new Staticbody(new Vector(400, 500), 30f);
        sb3.AddCollider(new RectangleCollider(new Vector(100, 40)));
        
        Staticbody sb4 = new Staticbody(new Vector(500, 500), 135f);
        sb4.AddCollider(new RectangleCollider(new Vector(50, 50)));
        
        Staticbody sb5 = new Staticbody(new Vector(600, 500), 0);
        sb5.AddCollider(new PolygonCollider(
        [
            new Vector(30, 30),
            new Vector(40, -20),
            new Vector(0, -40),
            new Vector(-30, -25),
            new Vector(-45, 35),
        ]));
        
        
        Rigidbody rb1 = new Rigidbody(new Vector(200, 300), 25d);
        rb1.AddCollider(new CircleCollider(25d));
        rb1.SetVelocity(new Vector(0, 100d));
        rb1.Restitution = 0.5d;
        
        
        Rigidbody rb2 = new Rigidbody(new Vector(300, 250), 45f);
        rb2.AddCollider(new RectangleCollider(new Vector(70, 30)));
        rb2.SetAngularVelocity(25f);
        rb2.SetVelocity(new Vector(0, 75d));

        Rigidbody rb3 = new Rigidbody(new Vector(420, 200)); 
        rb3.AddCollider(new PolygonCollider(
        [
            new Vector(20, 20),
            new Vector(20, -20),
            new Vector(0, -30),
            new Vector(-20, -20),
            new Vector(-20, 20)
        ]));
        rb3.SetAngularVelocity(-90f);
        rb3.SetVelocity(new Vector(0d, 55d));
        
        Rigidbody rb4 = new Rigidbody(new Vector(500, 150), 45d);
        rb4.AddCollider(new CircleCollider(45d));
        rb4.SetVelocity(new Vector(0, 35d));
        
        Rigidbody rb5 = new Rigidbody(new Vector(550, -150), 45d);
        rb5.AddCollider(new CircleCollider(20d));
        rb5.SetVelocity(new Vector(0, 60d));
        
        //rb2.OnCollisionEnterSubscribe(manifold => {Console.WriteLine("Enter! " + manifold.PhysicsObject1.Id + " : " + manifold.PhysicsObject2.Id);});
        //rb2.OnCollisionExitSubscribe(manifold => {Console.WriteLine("Exit! " + manifold.PhysicsObject1.Id + " : " + manifold.PhysicsObject2.Id);});
        */

        Rigidbody rbA = Rigidbody.CreateRigidbody(new Vector(8, 4), 0d, Collider.CreateCircleCollider(1d), 1d, 1d, 1d);
        rbA.SetVelocity(new Vector(2d, 0d));
        
        Rigidbody rbB = Rigidbody.CreateRigidbody(new Vector(24, 4), 0d, Collider.CreateCircleCollider(1d), 1d, 1d, 1d);
        rbB.SetVelocity(new Vector(-2d, 0d));

        Rigidbody rbC = Rigidbody.CreateRigidbody(new Vector(8, 8), 60d, Collider.CreateRectangleCollider(new Vector(2d, 1d)), 1d, 1d, 1d);
        rbC.SetVelocity(new Vector(1.5d, 0d));
        
        Rigidbody rbD = Rigidbody.CreateRigidbody(new Vector(24, 8), 0d, Collider.CreateCircleCollider(1.2d), 1d, 1d, 1d);
        rbD.SetVelocity(new Vector(-1.5d, 0d));
        
        Rigidbody rbE = Rigidbody.CreateRigidbody(new Vector(8, 12), 15d, Collider.CreateRectangleCollider(new Vector(2.5d, 1d)), 1d, 1d, 1d);
        rbE.SetVelocity(new Vector(3d, 0d));
        
        Rigidbody rbF = Rigidbody.CreateRigidbody(new Vector(24, 12), 45d, Collider.CreateRectangleCollider(new Vector(1.5, 1.5)), 1d, 1d, 1d);
        rbF.SetVelocity(new Vector(-3d, 0d));
        
        Rigidbody rbG = Rigidbody.CreateRigidbody(new Vector(8, 16), 0d, 
            Collider.CreatePolygonCollider(
            new Vector(1, 1),
            new Vector(1, -1),
            new Vector(0, -1.5),
            new Vector(-1, -1),
            new Vector(-1, 1)
            ), 1d, 1d, 1d);
        rbG.SetVelocity(new Vector(4d, 0d));
        
        Rigidbody rbH = Rigidbody.CreateRigidbody(new Vector(24, 16), 0d, 
            Collider.CreatePolygonCollider(
            new Vector(1.5, 1.5),
                new Vector(2, -1),
                new Vector(0, -2),
                new Vector(-1.5, -1.25),
                new Vector(-2.25, 1.75)
            ), 1d, 1d, 1d);
        rbH.SetVelocity(new Vector(-0.25d, 0d));
        
        
        while (PhysiXVisualizer.IsVisualizerActive())
        {
            //Keep running program until visualizer thread dies
        }
        
        PhysiX.Shutdown();
        
        Console.WriteLine("Test-sim ended");
    }
}