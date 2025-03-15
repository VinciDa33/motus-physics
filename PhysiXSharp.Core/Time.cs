namespace PhysiXSharp.Core;

public class Time
{
    /// <summary>
    /// The simulation timescale.
    /// Change this value to speed up, or slow down the simulation.
    /// Large values will have an effect on the robustness of the simulation.
    /// </summary>
    public double TimeScale = 1.0d;
    
    /// <summary>
    /// Time in seconds since PhysiX Sharp was initialized.
    /// </summary>
    public double ElapsedTime { get; internal set; }
    
    /// <summary>
    /// Most recent time between PhysiX update calls.
    /// </summary>
    public double DeltaTime { get; internal set; }
    
    /// <summary>
    /// Most recent time between physics step calls
    /// </summary>
    public double FixedDeltaTime { get; internal set; }
    
    /// <summary>
    /// The fixed time for each physics step
    /// </summary>
    public double FixedTimeStep { get; internal set; }
}