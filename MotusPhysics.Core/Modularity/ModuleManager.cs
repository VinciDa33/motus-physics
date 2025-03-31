using System.Reflection;

namespace MotusPhysics.Core.Modularity;

public sealed class ModuleManager
{
    private static ModuleManager? _instance = null;
    private static readonly object InstanceLock = new ();

    internal static ModuleManager Instance
    {
        get
        {
            lock (InstanceLock)
            {
                return _instance ??= new ModuleManager();
            }
        }
    }
    
    private ModuleManager() {}

    private readonly List<IMotusModule> _motusModules = new List<IMotusModule>();

    public String[] GetListOfLoadedModules()
    {
        string[] types = new string[_motusModules.Count];
        for (int i = 0; i < _motusModules.Count; i++)
            types[i] = _motusModules[i].GetType().ToString();
        return types;
    }
    
    internal void Load(string path)
    {
        Motus.Logger.Log("Loading modules");

        if (!Directory.Exists(path))
        {
            Motus.Logger.LogWarning("Module directory not found!");
            return;
        }

        string[] moduleFiles = Directory.GetFiles(path, "*.dll");
        
        //Iterate through each file in the modules folder
        foreach (string module in moduleFiles)
        {
            Assembly assembly = Assembly.LoadFrom(module);
            Motus.Logger.Log("Checking module: " +  assembly.GetName());
            
            Type[] types = assembly.GetTypes();

            //Iterate over classes and interfaces defined by the module
            foreach (Type type in types)
            {
                //Check if type is a Motus module and that it is a concrete implementation
                if (typeof(IMotusModule).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    Motus.Logger.Log("...Loading type: " + type.Name);
                    object? instance = Activator.CreateInstance(type);
                    
                    //Skip types that result in a null
                    if (instance == null)
                    {
                        Motus.Logger.LogError("...Failed: Type resulted in null!");
                        continue;
                    }

                    IMotusModule moduleInstance = (IMotusModule)instance;
                    _motusModules.Add(moduleInstance);
                }
            }
        }

        //Initialize all modules found
        foreach (IMotusModule module in _motusModules)
            module.Initialize();
    }

    internal void UpdateModules()
    {
        foreach (IMotusModule motusModule in _motusModules)
        {
            motusModule.Update();
        }
    }

    internal void ShutdownModules()
    {
        foreach (IMotusModule motusModule in _motusModules)
        {
            motusModule.Shutdown();
            Motus.Logger.Log("Shutdown: " + motusModule.GetType());
        }
    }
}