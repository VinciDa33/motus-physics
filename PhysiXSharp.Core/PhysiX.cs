using System.Diagnostics;
using System.Linq.Expressions;
using PhysiXSharp.Core.Logging;
using PhysiXSharp.Core.Modularity;
using PhysiXSharp.Core.Physics;

namespace PhysiXSharp.Core;

public static class PhysiX
{
    public static bool IsInitialized { get; private set; } = false;
    private static string _modulePath = ".";
    private static int _physicsStepsPerSecond = 50;

    
    /// <summary>
    /// Default PhysiX logger object for printing to the console.
    /// </summary>
    public static Logger Logger { get; } = new Logger("PhysiXSharp");
    private static readonly Stopwatch DeltaStopwatch = new Stopwatch();
    private static readonly Stopwatch FixedDeltaStopwatch = new Stopwatch();
    
    /// <summary>
    /// The time elapsed since last call of Update() in seconds.
    /// </summary>
    public static double DeltaTime { get; private set; }
    
    /// <summary>
    /// The time elapsed since PhysiXSharp was initialized in seconds.
    /// </summary>
    public static double ElapsedSecondsSinceInitialization { get; private set; } = 0;

    /// <summary>
    /// Time elapsed since the last fixed physics step was handled.
    /// This value remains mostly stable. Use standard DeltaTime when modifying values from outside the physics system.
    /// </summary>
    public static double FixedDeltaTime { get; private set; }
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

        IsInitialized = true;
        ModuleManager.Instance.Load(_modulePath);
        DeltaStopwatch.Start();
        FixedDeltaStopwatch.Start();
        
    }

    /// <summary>
    /// Update the physics system.
    /// Call this once every iteration of the main game loop.
    /// PhysiX must be initialized.
    /// </summary>
    public static void Update()
    {
        if (!IsInitialized)
        {
            Logger.LogError("PhysiX must be initialized before calling Step()!");
            return;
        }
        
        DeltaTime = DeltaStopwatch.Elapsed.TotalSeconds - ElapsedSecondsSinceInitialization;
        ElapsedSecondsSinceInitialization += DeltaTime;
        _deltaSum += DeltaTime;

        while (_deltaSum >= 1d / _physicsStepsPerSecond)
        {
            FixedDeltaTime = FixedDeltaStopwatch.Elapsed.TotalSeconds - _fixedSecondsElapsed;
            _fixedSecondsElapsed += FixedDeltaTime;
            Step();
            _deltaSum -= 1d / _physicsStepsPerSecond;
        }
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
    }
}