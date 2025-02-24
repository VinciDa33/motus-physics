using PhysiXSharp.Core;
using PhysiXSharp.Core.Modularity;
using PhysiXSharp.Core.Physics;

namespace PhysiXSharp.Visualizer;

internal class VisualizerModule : IPhysiXModule
{
    private static Thread? _visualizationThread = null;
    
    public void Initialize(PhysicsManager physicsManager)
    {
        PhysiX.Logger.Log("Visualizer initialized!");
        if (!PhysiXVisualizer.DoVisualization)
        {
            PhysiX.Logger.Log("Visualization disabled!");
            return;
        }

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
        if (!_visualizationThread.IsAlive)
            return false;
        return true;
    }
}