namespace Core.Utility;

public class Vector
{
    public double x;
    public double y;
    
    public Vector(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return x + " : " + y;
    }
}