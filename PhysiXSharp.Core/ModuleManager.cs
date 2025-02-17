using System.Reflection;
using PhysiXSharp.Core.ModuleInterfaces;

namespace PhysiXSharp.Core;

internal sealed class ModuleManager
{
    private static ModuleManager _instance;
    private static readonly object _instanceLock = new ();

    public static ModuleManager Instance
    {
        get
        {
            lock (_instanceLock)
            {
                return _instance ??= new ModuleManager();
            }
        }
    }

    private readonly List<IPhysiXModule> _physiXModules = new List<IPhysiXModule>();
    
    public void Load(string path)
    {
        PhysiX.Logger.Log("Loading modules");

        if (!Directory.Exists(path))
        {
            PhysiX.Logger.LogWarning("Module directory not found!");
            return;
        }

        string[] moduleFiles = Directory.GetFiles(path, "*.dll");

        //Iterate through each file in the modules folder
        foreach (string module in moduleFiles)
        {
            Assembly assembly = Assembly.LoadFrom(module);
            PhysiX.Logger.Log("Loading module: " +  assembly.GetName());
            
            Type[] types = assembly.GetTypes();

            //Iterate over classes and interfaces defined by the module
            foreach (Type type in types)
            {
                //Check if type is a PhysiX module and that it is a concrete implementation
                if (typeof(IPhysiXModule).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    PhysiX.Logger.Log("...Loading type: " + type.Name);
                    object? instance = Activator.CreateInstance(type);
                    
                    //Skip types that result in a null
                    if (instance == null)
                    {
                        PhysiX.Logger.LogError("...Failed: Type resulted in null!");
                        continue;
                    }

                    IPhysiXModule moduleInstance = (IPhysiXModule)instance;
                    
                    //Run the initialization of the module
                    moduleInstance.Initialize();
                    
                    _physiXModules.Add(moduleInstance);
                }
            }
        }
    }
}