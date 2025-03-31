
using MotusPhysics.Core;
using MotusPhysics.Core.Modularity;

namespace MotusPhysics.Visualizer;

internal class VisualizerModule : IMotusModule
{
    private static Thread? _visualizationThread = null;
    private VisualizationRunner runner;
    
    public void Initialize()
    {
        Motus.Logger.Log("Visualizer module initialized!");
        if (!MotusVisualizer.DoVisualization)
        {
            Motus.Logger.Log("Visualization disabled!");
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