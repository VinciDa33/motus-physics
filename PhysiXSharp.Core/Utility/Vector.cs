namespace PhysiXSharp.Core.Utility;

public class Vector : ICloneable
{
    public double x;
    public double y;
    
    public Vector(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public static Vector operator +(Vector v) => v;
    public static Vector operator -(Vector v) => new Vector(-v.x, -v.y);

    public static Vector operator +(Vector v1, Vector v2) => new Vector(v1.x + v2.x, v1.y + v2.y);
    public static Vector operator -(Vector v1, Vector v2) => new Vector(v1.x - v2.x, v1.y - v2.y);
    public static Vector operator *(Vector v1, double d) => new Vector(v1.x * d, v1.y * d);
    public static Vector operator /(Vector v1, double d) => new Vector(v1.x / d, v1.y / d);

    public static bool operator ==(Vector v1, Vector v2) => v1.x == v2.x && v1.y == v2.y;
    public static bool operator !=(Vector v1, Vector v2) => v1.x != v2.x || v1.y != v2.y;
    
    public double Magnitude() {
        return Math.Sqrt(x * x + y * y);
    }
    
    public void Normalize() {
        double mag = Magnitude();
        x /= mag;
        y /= mag;
    }
  
    public Vector Normalized() {
        double mag = Magnitude();
        return new Vector(x / mag, y / mag);
    }
    
    public void Rotate(float angle)
    {
        double theta = (Math.PI / 180) * angle;

        double cs = Math.Cos(theta);
        double sn = Math.Sin(theta);
    
        double px = x * cs - y * sn;
        double py = x * sn + y * cs;
    
        x = px;
        y = py;
    }
    
    public static double Distance(Vector v1, Vector v2) {
        double dx = v1.x - v2.x;
        double dy = v1.y - v2.y;
    
        return Math.Sqrt(dx * dx + dy * dy);
    }
    

    public override string ToString()
    {
        return x + " : " + y;
    }

    public object Clone()
    {
        return new Vector(x, y);
    }
}