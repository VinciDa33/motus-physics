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
        PhysiXVisualizer.WindowSize = new Vector(800*1.8, 600*1.8);
        PhysiXVisualizer.PixelsPerMeter = 40;
        PhysiX.Initialize();
        //PhysiX.SetPhysicsUpdateRate(50);
        //PhysiX.Time.TimeScale = 1d;
        //PhysiXVisualizer.ShowBoundingBoxes = true;
        PhysiXVisualizer.ShowCollisionContactPoints = false;
        PhysiXVisualizer.ShowRigidbodyOrigins = false;
        //PhysiXVisualizer.ShowEdgeNormals = true;
        PhysiXVisualizer.ShowPhysicsStepCalculationTime = false;
        
        //ChaoticSetup.Setup();
        //StackSetup.Setup();
        //SimpleCollisionSetup.Setup();
        StressSetup.Setup();
        
        while (PhysiXVisualizer.IsVisualizerActive())
        {

        }
        
        PhysiX.Shutdown();
        
        Console.WriteLine("Test-sim ended");
    }
}