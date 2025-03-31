using System.Diagnostics;
using MotusPhysics.Core.Logging;
using MotusPhysics.Core.Modularity;
using MotusPhysics.Core.Physics;

namespace MotusPhysics.Core;

public static class Motus
{
    public static bool IsInitialized { get; private set; } = false;
    private static string _modulePath = ".";
    private static Thread _physicsThread;
    private static bool _loadModules = true;
    private static bool _shutdown = false;
    private static int _physicsStepsPerSecond = 50;

    
    /// <summary>
    /// Default Motus logger object for printing to the console.
    /// </summary>
    public static Logger Logger { get; } = new Logger("Motus");
    public static Time Time { get; } = new Time();
    private static readonly Stopwatch DeltaStopwatch = new Stopwatch();
    private static readonly Stopwatch FixedDeltaStopwatch = new Stopwatch();
    private static readonly Stopwatch StepTimerStopwatch = new Stopwatch();
    
    private static double _fixedSecondsElapsed = 0;
    private static double _deltaSum = 0;
    
    /// <summary>
    /// Initializes Motus.
    /// Initializes all installed Motus modules.
    /// If you want to configure specific modules it should be done before calling this method.
    /// </summary>
    public static void Initialize()
    {
        if (IsInitialized)
        {
            Logger.LogWarning("Motus is already initialized!");
            return;
        }

        Time.FixedTimeStep = 1d / _physicsStepsPerSecond;

        IsInitialized = true;
        
        if (_loadModules)
            ModuleManager.Instance.Load(_modulePath);
        
        DeltaStopwatch.Start();
        FixedDeltaStopwatch.Start();
        
        _physicsThread = new Thread(Update);
        _physicsThread.Start();
        
        Logger.Log("Motus is initialized!");
    }

    /// <summary>
    /// Update the physics system.
    /// Call this once every iteration of the main game loop.
    /// Motus must be initialized.
    /// </summary>
    private static void Update()
    {
        while (!_shutdown)
        {

            Time.DeltaTime = DeltaStopwatch.Elapsed.TotalSeconds - Time.ElapsedTime;
            Time.ElapsedTime += Time.DeltaTime;
            _deltaSum += Time.DeltaTime;

            while (_deltaSum >= 1d / _physicsStepsPerSecond)
            {
                Time.FixedDeltaTime = FixedDeltaStopwatch.Elapsed.TotalSeconds - _fixedSecondsElapsed;
                _fixedSecondsElapsed += Time.FixedDeltaTime;
                
                StepTimerStopwatch.Start();
                Step();
                StepTimerStopwatch.Stop();
                Time.LastStepMilliseconds = StepTimerStopwatch.Elapsed.TotalMilliseconds;
                StepTimerStopwatch.Reset();
                
                Time.SimStep++;
                _deltaSum -= 1d / _physicsStepsPerSecond;

                if (Time.LastStepMilliseconds / 1000d > Time.FixedTimeStep)
                    Logger.LogError($"Simulation can not keep up with update rate! \nFixed time step: {Time.FixedTimeStep} \nTime of last step: {Time.LastStepMilliseconds / 1000d} \nAt step: {Time.SimStep}");
            }
        }

        _shutdown = false;
        IsInitialized = false;
        _loadModules = true;
        Logger.Log("Shutdown: MotusPhysics.Core");
    }
    
    private static void Step()
    {
        if (_shutdown)
            PhysicsManager.Instance.ClearSimulation();
        
        PhysicsManager.Instance.Update();
        ModuleManager.Instance.UpdateModules();
    }

    /// <summary>
    /// Set the path to directory where Motus# modules will be loaded from.
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

    public static void DisableModuleLoading()
    {
        _loadModules = false;
    }
    
    
    /// <summary>
    /// Stops the physics thread and all loaded modules.
    /// </summary>
    public static void Shutdown()
    {
        if (!_physicsThread.IsAlive)
        {
            Logger.LogWarning("Cannot abort physics thread. It is already not running!");
            return;
        }

        _shutdown = true;

        ModuleManager.Instance.ShutdownModules();
    }
}