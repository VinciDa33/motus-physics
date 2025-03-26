using System.Reflection;
using PhysiXSharp.Core;
using PhysiXSharp.Core.Modularity;
using PhysiXSharp.Core.Physics;

namespace PhysiXSharp.Visualizer;

internal class VisualizerModule : IPhysiXModule
{
    private static Thread? _visualizationThread = null;
    
    public void Initialize(PhysicsManager physicsManager)
    {
        PhysiX.Logger.Log("Visualizer module initialized!");
        if (!PhysiXVisualizer.DoVisualization)
        {
            PhysiX.Logger.Log("Visualization disabled!");
            return;
        }

        AppDomain.CurrentDomain.AssemblyResolve += ResourceLoader.LoadEmbeddedAssembly;
        
        ResourceLoader.LoadSFML();

        _visualizationThread = new Thread(new VisualizationRunner(physicsManager).RunVisualization);
        _visualizationThread.Start();
        
    }

    public void Update()
    {
        //throw new NotImplementedException();
    }

    public static bool IsVisualizationActive()
    {
        if (_visualizationThread == null)
            return false;
        return _visualizationThread.IsAlive;
    }
}