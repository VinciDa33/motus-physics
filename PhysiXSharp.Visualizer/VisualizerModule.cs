using System.Reflection;
using PhysiXSharp.Core;
using PhysiXSharp.Core.Modularity;
using PhysiXSharp.Core.Physics;

namespace PhysiXSharp.Visualizer;

internal class VisualizerModule : IPhysiXModule
{
    private static Thread? _visualizationThread = null;
    private VisualizationRunner runner;
    
    public void Initialize()
    {
        PhysiX.Logger.Log("Visualizer module initialized!");
        if (!PhysiXVisualizer.DoVisualization)
        {
            PhysiX.Logger.Log("Visualization disabled!");
            return;
        }

        //Dynamically load the SFML.net wrappers
        AppDomain.CurrentDomain.AssemblyResolve += ResourceLoader.LoadEmbeddedAssembly;
        //Dynamically extract and reference the SFML native files
        ResourceLoader.LoadSFML();

        //Start the visualization thread
        runner = new VisualizationRunner();
        _visualizationThread = new Thread(runner.RunVisualization);
        _visualizationThread.Start();
        
    }

    public void Update()
    {
        //throw new NotImplementedException();
    }

    public void Shutdown()
    {
        runner.Shutdown = true;
    }

    internal static bool IsVisualizationActive()
    {
        if (_visualizationThread == null)
            return false;
        return _visualizationThread.IsAlive;
    }
}