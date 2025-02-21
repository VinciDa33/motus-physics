using System.Diagnostics;
using PhysiXSharp.Core.Logging;
using PhysiXSharp.Core.Modularity;
using PhysiXSharp.Core.Physics;

namespace PhysiXSharp.Core;

public static class PhysiX
{
    private static bool _isInitialized = false;
    private static string _modulePath = ".";
    
    /// <summary>
    /// The time elapsed since last physics step.
    /// </summary>
    public static double DeltaTime { get; private set; }
    
    /// <summary>
    /// The time elapsed since PhysiXSharp was initialized.
    /// </summary>
    public static double ElapsedSecondsSinceInitialization { get; private set; } = 0;
    
    /// <summary>
    /// Default PhysiX logger object for printing to the console.
    /// </summary>
    public static Logger Logger { get; } = new Logger("PhysiXSharp");
    private static readonly Stopwatch Stopwatch = new Stopwatch();
    
    /// <summary>
    /// Initializes PhysiXSharp.
    /// Initializes all installed PhysiXSharp modules.
    /// If you want to configure specific modules it should be done before calling this method.
    /// </summary>
    public static void Initialize()
    {
        if (_isInitialized)
        {
            Logger.LogWarning("Initialization should only occur once!");
            return;
        }

        _isInitialized = true;
        ModuleManager.Instance.Load(_modulePath);
        Stopwatch.Start();
    }
    
    /// <summary>
    /// 
    /// </summary>
    public static void Step()
    {
        if (!_isInitialized)
        {
            Logger.LogError("PhysiX must be initialized before calling Step()!");
            return;
        }
        
        DeltaTime = Stopwatch.Elapsed.TotalSeconds - ElapsedSecondsSinceInitialization;
        ElapsedSecondsSinceInitialization += DeltaTime;
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
}