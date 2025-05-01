
using MotusPhysics.Core;
using MotusPhysics.Core.Modularity;

namespace MotusPhysics.Visualizer;

internal class VisualizerModule : IMotusModule
{
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
        MotusVisualizer.StartVisualizer();
    }

    public void Update()
    {
        //throw new NotImplementedException();
    }

    public void Shutdown()
    {
        MotusVisualizer.Shutdown();
    }
}