using System.Diagnostics;
using PhysiXSharp.Core.Logging;
using PhysiXSharp.Core.Modularity;
using PhysiXSharp.Core.Physics;

namespace PhysiXSharp.Core;

public static class PhysiX
{
    public static bool IsInitialized { get; private set; } = false;
    private static string _modulePath = ".";
    private static Thread _physicsThread;
    private static bool _abortThread = false;
    private static int _physicsStepsPerSecond = 50;

    
    /// <summary>
    /// Default PhysiX logger object for printing to the console.
    /// </summary>
    public static Logger Logger { get; } = new Logger("PhysiXSharp");
    public static Time Time { get; } = new Time();
    private static readonly Stopwatch DeltaStopwatch = new Stopwatch();
    private static readonly Stopwatch FixedDeltaStopwatch = new Stopwatch();
    
    private static double _fixedSecondsElapsed = 0;
    private static double _deltaSum = 0;
    
    /// <summary>
    /// Initializes PhysiXSharp.
    /// Initializes all installed PhysiXSharp modules.
    /// If you want to configure specific modules it should be done before calling this method.
    /// </summary>
    public static void Initialize()
    {
        if (IsInitialized)
        {
            Logger.LogWarning("Initialization should only occur once!");
            return;
        }

        Time.FixedTimeStep = 1d / _physicsStepsPerSecond;

        IsInitialized = true;
        ModuleManager.Instance.Load(_modulePath);
        DeltaStopwatch.Start();
        FixedDeltaStopwatch.Start();
        
        _physicsThread = new Thread(Update);
        _physicsThread.Start();
    }

    /// <summary>
    /// Update the physics system.
    /// Call this once every iteration of the main game loop.
    /// PhysiX must be initialized.
    /// </summary>
    private static void Update()
    {
        while (!_abortThread)
        {

            Time.DeltaTime = DeltaStopwatch.Elapsed.TotalSeconds - Time.ElapsedTime;
            Time.ElapsedTime += Time.DeltaTime;
            _deltaSum += Time.DeltaTime;

            while (_deltaSum >= 1d / _physicsStepsPerSecond)
            {
                Time.FixedDeltaTime = FixedDeltaStopwatch.Elapsed.TotalSeconds - _fixedSecondsElapsed;
                _fixedSecondsElapsed += Time.FixedDeltaTime;
                Step();
                Time.SimStep++;
                _deltaSum -= 1d / _physicsStepsPerSecond;
            }
        }

        _abortThread = false;
    }
    
    private static void Step()
    {
        PhysicsManager.Instance.Update();
        ModuleManager.Instance.UpdateModules();
    }

    /// <summary>
    /// Set the path to directory where PhysiX# modules will be loaded from.
    /// </summary>
    public static void SetModulePath(string path)
    {
        _modulePath = path;
    }

    /// <summary>
    /// Set the amount of physics updates to do each second.
    /// Higher values will take up more computing power.
    /// </summary>
    /// <param name="stepsPerSecond"></param>
    public static void SetPhysicsUpdateRate(int stepsPerSecond)
    {
        _physicsStepsPerSecond = stepsPerSecond;
        Time.FixedTimeStep = 1d / _physicsStepsPerSecond;
    }

    /// <summary>
    /// Stops the physics thread and ends the physics simulation.
    /// </summary>
    public static void Shutdown()
    {
        if (!_physicsThread.IsAlive)
        {
            Logger.LogWarning("Cannot abort physiX thread. It is already not running!");
            return;
        }
        
        _abortThread = true;
    }
}