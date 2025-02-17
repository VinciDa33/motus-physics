using PhysiXSharp.Core.Logging;

namespace PhysiXSharp.Core;

public static class PhysiX
{
    private static string _modulePath = ".";

    public static PhysiXLogger Logger { get; } = new PhysiXLogger("PhysiX#");

    /// <summary>
    /// 
    /// </summary>
    public static void Initialize()
    {
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