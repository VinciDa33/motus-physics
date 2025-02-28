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

        //TestCircle t1 = new TestCircle(new Vector(100, 100), 50d);
        //TestCircle t2 = new TestCircle(new Vector(200, 300), 25d);
        TestRectangle r1 = new TestRectangle(new Vector(500, 400), new Vector(100, 50));
        r1.Rotate(35f);

        //TestRigid ri1 = new TestRigid(new Vector(50, 300), new Vector(50, 50));
        //ri1.SetVelocity(new Vector(20d, 0d));
        //ri1.SetAngularVelocity(45f);

        TestPoly p1 = new TestPoly(new Vector(750, 450), [
            new Vector(-30, 0),
            new Vector(30, 20),
            new Vector(40, 0),
            new Vector(30, -20)
        ]);
        p1.SetVelocity(new Vector(-40d, -10d));
        p1.SetAngularVelocity(-20f);
        
        while (PhysiXVisualizer.IsVisualizerActive())
        {
            PhysiX.Update();
            r1.Rotate(25f * (float) PhysiX.DeltaTime);
        }
        
        Console.WriteLine("Test-sim ended");
    }
}