using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Visualizer;

public static class PhysiXVisualizer
{
    public static bool DoVisualization = true;
    public static Vector WindowSize = new Vector(800, 600);

    public static bool ShowCollisionShapes = true;
    public static bool ShowBoundingBoxes = false;
    public static bool ShowPhysicsObjectOrigins = false;
    public static bool ShowEdgeNormals = true;
    
    public static bool IsVisualizerActive()
    {
        return VisualizerModule.IsVisualizationActive();
    }
}