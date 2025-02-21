using PhysiXSharp.Core;
using PhysiXSharp.Core.Modularity;

namespace PhysiXSharp.Visualizer;

public class VisualizerModule : IPhysiXModule
{
    public void Initialize()
    {
        PhysiX.Logger.Log("Visualizer initialized!");
    }

    public void Update()
    {
        //throw new NotImplementedException();
    }
}