using MotusPhysics.Core.Utility;

namespace MotusPhysics.Visualizer;

public static class MotusVisualizer
{
    /// <summary>
    /// Whether the visualizer should be started or not.
    /// Must be set before Motus.Initialize is called.
    /// </summary>
    public static bool DoVisualization = true;
    
    /// <summary>
    /// The window size for the visualization in pixels.
    /// Must be set before Motus.Initialize is called.
    /// </summary>
    public static Vector WindowSize = new Vector(800, 600);
    
    /// <summary>
    /// How many pixel represent one meter.
    /// This means a rectangle collider with a width and height of 1 (meter),
    /// at 100 pixels per meter, will be rendered as a 100x100 pixel square.
    /// </summary>
    public static int PixelsPerMeter = 25;

    public static bool ShowPhysicsStepCalculationTime = true;
    public static bool ShowCollisionShapes = true;
    public static bool ShowBoundingBoxes = false;
    public static bool ShowRigidbodyOrigins = true;
    public static bool ShowEdgeNormals = false;
    public static bool ShowCollisionContactPoints = true;
    
    
    /// <summary>
    /// Returns whether the visualizer is actively running, or if the thread has finished.
    /// </summary>
    /// <returns></returns>
    public static bool IsVisualizerActive()
    {
        return VisualizerModule.IsVisualizationActive();
    }

    /// <summary>
    /// Returns the width and height of the viewport in simulated meters.
    /// Based on the window size in pixels and the current pixels per meter.
    /// </summary>
    /// <returns></returns>
    public static Vector GetViewportSizeInMeters()
    {
        return WindowSize / PixelsPerMeter;
    }
}