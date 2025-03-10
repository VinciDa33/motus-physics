using PhysiXSharp.Core;
using PhysiXSharp.Core.Utility;
using PhysiXSharp.Visualizer;

namespace TestRunner;

class Program
{
    static void Main(string[] args)
    {
        PhysiX.SetModulePath("../../../../TestRunner/bin/Debug/net8.0");
        PhysiX.Initialize();

        StaticCircle sc1 = new StaticCircle(new Vector(200, 500), 40d);
        StaticCircle sc2 = new StaticCircle(new Vector(300, 500), 40d);
        StaticRectangle sr1 = new StaticRectangle(new Vector(400, 500), new Vector(60, 40), 30);
        StaticRectangle sr2 = new StaticRectangle(new Vector(500, 500), new Vector(60, 40), 135);
        StaticPolygon sp1 = new StaticPolygon(new Vector(600, 500), [
            new Vector(30, 30),
            new Vector(40, -20),
            new Vector(0, -40),
            new Vector(-30, -25),
            new Vector(-45, 35),
        ]);

        RigidCircle rc1 = new RigidCircle(new Vector(200, 300), 25d);
        rc1.SetVelocity(new Vector(0, 100d));
        
        RigidRectangle rr1 = new RigidRectangle(new Vector(300, 250), new Vector(100, 60));
        rr1.SetAngularVelocity(25f);
        rr1.Rotate(45f);
        rr1.SetVelocity(new Vector(0, 75d));

        
        RigidPolygon rp1 = new RigidPolygon(new Vector(420, 200), [
            new Vector(20, 20),
            new Vector(20, -20),
            new Vector(0, -30),
            new Vector(-20, -20),
            new Vector(-20, 20)
        ]);
        rp1.SetAngularVelocity(-90f);
        rp1.SetVelocity(new Vector(0d, 55d));
        
        RigidCircle rc2 = new RigidCircle(new Vector(500, 150), 45d);
        rc2.SetVelocity(new Vector(0, 35d));
        
        RigidCircle rc3 = new RigidCircle(new Vector(550, -150), 45d);
        rc3.SetVelocity(new Vector(0, 60d));
        
        StaticPolygon sp2 = new StaticPolygon(new Vector(50, 300), [
            new Vector(30, 30),
            new Vector(-30, 30),
            new Vector(0, -40),
        ]);        
        while (PhysiXVisualizer.IsVisualizerActive())
        {
            //Keep running program until visualizer thread dies
        }
        
        PhysiX.Shutdown();
        
        Console.WriteLine("Test-sim ended");
    }
}