using System.Reflection;
using SFML.Graphics;

namespace PhysiXSharp.Visualizer;

public class ResourceLoader
{
    public static Font LoadEmbeddedFont()
    {
        // Get the assembly where the resource is embedded
        Assembly assembly = Assembly.GetExecutingAssembly();

        // Define the resource name (namespace + filename)
        string resourceName = "PhysiXSharp.Visualizer.Resources.arial.ttf";

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
}