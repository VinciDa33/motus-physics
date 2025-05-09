namespace MotusPhysics.Core.Utility;

public class Vector : ICloneable
{
    public double x;
    public double y;

    public static Vector Zero => new Vector(0d, 0d);

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
    public static Vector operator *(double d, Vector v1) => new Vector(v1.x * d, v1.y * d);

    public static Vector operator /(Vector v1, double d) => new Vector(v1.x / d, v1.y / d);

    public bool Equals(Vector other)
    {
        return x == other.x && y == other.y;
    }
    
    public double Magnitude() {
        return Math.Sqrt(x * x + y * y);
    }
    
    public void Normalize() {
        double mag = Magnitude();
        
        if (mag == 0)
        {
            Motus.Logger.LogError("Division by zero during normalization!");
            return;
        }
        
        x /= mag;
        y /= mag;
    }
  
    public Vector Normalized() {
        double mag = Magnitude();
        
        if (mag == 0)
        {
            Motus.Logger.LogError("Division by zero during normalization!");
            return Vector.Zero;
        }
        
        return new Vector(x / mag, y / mag);
    }

    public Vector Normal()
    {
        return new Vector(-y, x).Normalized();
    }
    
    public void Rotate(double degrees)
    {
        double theta = (Math.PI / 180) * degrees;

        double cs = Math.Cos(theta);
        double sn = Math.Sin(theta);
    
        double px = x * cs - y * sn;
        double py = x * sn + y * cs;
    
        x = px;
        y = py;
    }
    
    public Vector Rotated(double degrees)
    {
        double theta = (Math.PI / 180) * degrees;

        double cs = Math.Cos(theta);
        double sn = Math.Sin(theta);
    
        double px = x * cs - y * sn;
        double py = x * sn + y * cs;

        return new Vector(px, py);
    }
    
    public static double Distance(Vector v1, Vector v2) {
        double dx = v1.x - v2.x;
        double dy = v1.y - v2.y;
    
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public static double DistanceSquared(Vector v1, Vector v2) {
        double dx = v1.x - v2.x;
        double dy = v1.y - v2.y;
    
        return dx * dx + dy * dy;
    }
    
    public static double Dot(Vector v1, Vector v2)
    {
        return v1.x * v2.x + v1.y * v2.y;
    }

    public static double Cross(Vector v1, Vector v2)
    {
        return v1.x * v2.y - v1.y * v2.x;
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