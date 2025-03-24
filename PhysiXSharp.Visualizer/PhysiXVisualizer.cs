using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Visualizer;

public static class PhysiXVisualizer
{
    public static bool DoVisualization = true;
    public static Vector WindowSize = new Vector(800, 600);
    
    /// <summary>
    /// How many pixel represent one meter.
    /// This means a rectangle collider with a width and height of 1 (meter),
    /// at 100 pixels per meter, will be rendered as a 100x100 pixel square.
    /// </summary>
    public static int PixelsPerMeter = 25;

    public static bool ShowCollisionShapes = true;
    public static bool ShowBoundingBoxes = false;
    public static bool ShowRigidbodyOrigins = true;
    public static bool ShowEdgeNormals = false;
    public static bool ShowCollisionContactPoints = true;
    public static bool ShowPolygonCentroids = false;
    
    public static bool IsVisualizerActive()
    {
        return VisualizerModule.IsVisualizationActive();
    }

    public static Vector GetViewportSizeInMeters()
    {
        return WindowSize / PixelsPerMeter;
    }
}