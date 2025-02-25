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

        TestCircle t1 = new TestCircle(new Vector(100, 100), 50d);
        TestCircle t2 = new TestCircle(new Vector(200, 300), 25d);
        TestRectangle r1 = new TestRectangle(new Vector(500, 400), new Vector(100, 50));
        r1.Rotate(35f);

        while (PhysiXVisualizer.IsVisualizerActive())
        {
            PhysiX.Step();
            r1.Rotate(25f * (float) PhysiX.DeltaTime);
        }
        
        Console.WriteLine("Test-sim ended");
    }
}