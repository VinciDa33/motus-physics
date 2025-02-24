

using SFML.Graphics;
using SFML.Window;

namespace PhysiXSharp.Visualizer;

public class VisualizationRunner
{
    public void RunVisualization()
    {
        RenderWindow window = new RenderWindow(new VideoMode(800, 600), "PhysiXSharp Visualizer");
        window.Closed += (sender, e) => window.Close();

        // Create a rectangle shape
        CircleShape rectangle = new CircleShape(100)
        {
            FillColor = new Color(0, 0, 0, 0),
            OutlineColor = Color.Red,
            OutlineThickness = 5,
            Position = new SFML.System.Vector2f(100, 100)
        };

        // Main loop
        while (window.IsOpen)
        {
            window.DispatchEvents();
            window.Clear(new Color(30, 30, 30));

            // Draw the rectangle
            window.Draw(rectangle);

            window.Display();
        }
        
    }
}