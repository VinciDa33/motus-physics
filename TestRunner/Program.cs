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
        //PhysiX.Time.TimeScale = 1d;
        //PhysiXVisualizer.ShowPolygonCentroids = true;
        //PhysiXVisualizer.ShowBoundingBoxes = true;
        PhysiXVisualizer.ShowCollisionContactPoints = false;
        PhysiXVisualizer.ShowRigidbodyOrigins = false;
        //PhysiXVisualizer.ShowEdgeNormals = true;
        
        ChaoticSetup.Setup();
        //StackSetup.Setup();
        //SimpleCollisionSetup.Setup();
        
        while (PhysiXVisualizer.IsVisualizerActive())
        {
            //Keep running program until visualizer thread dies
        }
        
        PhysiX.Shutdown();
        
        Console.WriteLine("Test-sim ended");
    }
}