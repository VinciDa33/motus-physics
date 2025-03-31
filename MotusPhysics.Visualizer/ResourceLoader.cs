using System.Reflection;
using System.Runtime.InteropServices;
using SFML.Graphics;

namespace MotusPhysics.Visualizer;

internal static class ResourceLoader
{
    internal static Font LoadEmbeddedFont()
    {
        // Get the assembly where the resource is embedded
        Assembly assembly = Assembly.GetExecutingAssembly();

        // Define the resource name (namespace + filename)
        string resourceName = "MotusPhysics.Visualizer.Resources.arial.ttf";

        // Open the resource stream
        using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
            {
                throw new Exception($"Embedded resource '{resourceName}' not found.");
            }

            // Read the font data into a byte array
            byte[] fontData = new byte[stream.Length];
            var read = stream.Read(fontData, 0, fontData.Length);

            // Load the font from memory
            return new Font(fontData);
        }
    }
    
    private static readonly Dictionary<string, Assembly> LoadedAssemblies = new();
    internal static Assembly? LoadEmbeddedAssembly(object? sender, ResolveEventArgs args)
    {
        if (!args.Name.Contains("SFML"))
            return null;
        
        string resourceName = "MotusPhysics.Visualizer.Resources." + new AssemblyName(args.Name).Name + ".dll";
        Console.WriteLine("Trying to load: " + resourceName);
        
        string? name = new AssemblyName(args.Name).Name;
        
        if (name == null)
            return null;
        
        if (LoadedAssemblies.TryGetValue(name, out var loadedAssembly))
        {
            Console.WriteLine($"Already loaded: {name}");
            return loadedAssembly; // Return cached version
        }
        
        using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        if (stream == null) 
            return null;
        
        byte[] assemblyData = new byte[stream.Length];
        var read = stream.Read(assemblyData, 0, assemblyData.Length);
        var assembly = Assembly.Load(assemblyData);
        
        LoadedAssemblies[name] = assembly; // Store reference

        return assembly;
    }
    
    
    
    
    private static bool _isDependenciesLoaded = false;
    internal static void LoadSFML()
    {
        if (_isDependenciesLoaded) return;
        _isDependenciesLoaded = true;

        // DLLs to extract
        string[] sfmlDlls = {
            "csfml-system.dll",
            "csfml-window.dll",
            "csfml-graphics.dll",
        };

        // Extract DLLs to a temporary directory
        string tempPath = Path.Combine(Path.GetTempPath(), "SFMLNative");
        Directory.CreateDirectory(tempPath);

        Console.WriteLine("Created directory: " + tempPath);
        
        foreach (string dll in sfmlDlls)
        {
            Console.WriteLine("Extracting: " + dll);
            ExtractEmbeddedDll($"MotusPhysics.Visualizer.NativeLibs.{dll}", Path.Combine(tempPath, dll));
        }

        // Set the extracted path as a DLL search directory
        AddToDllSearchPath(tempPath);
    }
    
    private static void ExtractEmbeddedDll(string resourceName, string outputPath)
    {
        using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        if (stream == null)
            throw new Exception($"Embedded resource '{resourceName}' not found.");

        using FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
        stream.CopyTo(fileStream);
    }
    
    private static void AddToDllSearchPath(string path)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            SetDllDirectory(path);
        }
        else
        {
            Environment.SetEnvironmentVariable("LD_LIBRARY_PATH", path);
        }
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetDllDirectory(string lpPathName);
}