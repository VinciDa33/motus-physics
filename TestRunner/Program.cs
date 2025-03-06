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

        TestCircle t1 = new TestCircle(new Vector(100, 500), 50d);
        TestCircle t2 = new TestCircle(new Vector(300, 300), 50d);
        //TestCircle t3 = new TestCircle(new Vector(300, 300), 50d);
        //t3.SetVelocity(new Vector(0d, 40d));
        
        
        //TestRectangle r1 = new TestRectangle(new Vector(500, 500), new Vector(100, 50));
        //r1.Rotate(35f);

        
        TestRigid ri1 = new TestRigid(new Vector(500, 500), new Vector(50, 50));
        TestRigid ri2 = new TestRigid(new Vector(300, 500), new Vector(50, 50));
        ri2.Rotate(45f);
        //ri2.SetVelocity(new Vector(0d, 40d));
        
        t2.SetVelocity(new Vector(0d, 40d));
        
        //ri1.SetVelocity(new Vector(20d, 0d));
        //ri1.SetAngularVelocity(45f);

        /*
        TestPoly p1 = new TestPoly(new Vector(300, 300), [
            new Vector(-30, 0),
            new Vector(30, 20),
            new Vector(40, 0),
            new Vector(30, -20)
        ]);
        p1.SetVelocity(new Vector(0d, 40d));
        p1.SetAngularVelocity(-20f);
        */
        
        
        while (PhysiXVisualizer.IsVisualizerActive())
        {
            //PhysiX.Update();
        }
        
        PhysiX.Shutdown();
        
        Console.WriteLine("Test-sim ended");
    }
}