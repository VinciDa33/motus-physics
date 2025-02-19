using PhysiXSharp.Core.Logging;
using PhysiXSharp.Core.Modularity;

namespace PhysiXSharp.Core;

public static class PhysiX
{
    private static bool _isInitialized = false;
    private static string _modulePath = ".";

    public static Logger Logger { get; } = new Logger("PhysiX#");

    /// <summary>
    /// 
    /// </summary>
    public static void Initialize()
    {
        if (_isInitialized)
            return;
        
        _isInitialized = true;
        ModuleManager.Instance.Load(_modulePath);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public static void Step()
    {
        
    }

    /// <summary>
    /// Set the path to directory where PhysiX# modules will be loaded from.
    /// </summary>
    public static void SetModulePath(string path)
    {
        _modulePath = path;
    }
}