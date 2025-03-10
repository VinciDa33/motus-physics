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

        
        Staticbody sb1 = new Staticbody(new Vector(200, 500), 40f);
        sb1.AddCollider(new CircleCollider(50));
        
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
        
        
        while (PhysiXVisualizer.IsVisualizerActive())
        {
            //Keep running program until visualizer thread dies
        }
        
        PhysiX.Shutdown();
        
        Console.WriteLine("Test-sim ended");
    }
}