using PhysiXSharp.Core;
using PhysiXSharp.Core.Modularity;

namespace PhysiXSharp.Visualizer;

public class VisualizerModule : IPhysiXModule
{
    public void Initialize()
    {
        PhysiX.Logger.Log("Visualizer initialized!");
        if (!PhysiXVisualizer.DoVisualization)
        {
            PhysiX.Logger.Log("Visualization disabled!");
            return;
        }

        Thread visualizationThread = new Thread(new VisualizationRunner().RunVisualization);
        visualizationThread.Start();
    }

    public void Update()
    {
        //throw new NotImplementedException();
    }
}