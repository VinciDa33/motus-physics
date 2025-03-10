namespace PhysiXSharp.Core;

public class Time
{
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