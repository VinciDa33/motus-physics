using MotusPhysics.Core;
using MotusPhysics.Core.Utility;
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
        
        //ChaoticSetup.Setup();
        //StackSetup.Setup();
        //SimpleCollisionSetup.Setup();
        StressSetup.Setup();
        
        while (MotusVisualizer.IsVisualizerActive())
        {

        }
        
        Motus.Shutdown();
        
        Console.WriteLine("Test-sim ended");
    }
}