

using SFML.Graphics;
using SFML.Window;

namespace PhysiXSharp.Visualizer;

public class VisualizationRunner
{
    public void RunVisualization()
    {
        // Create the window
        RenderWindow window = new RenderWindow(new VideoMode(800, 600), "SFML.NET Rectangle Example");
        window.Closed += (sender, e) => window.Close();

        // Create a rectangle shape
        RectangleShape rectangle = new RectangleShape(new SFML.System.Vector2f(200, 100))
        {
            FillColor = Color.Blue,
            Position = new SFML.System.Vector2f(100, 100)
        };

        // Main loop
        while (window.IsOpen)
        {
            window.DispatchEvents();
            window.Clear(Color.White);

            // Draw the rectangle
            window.Draw(rectangle);

            window.Display();
        }
        
    }
}