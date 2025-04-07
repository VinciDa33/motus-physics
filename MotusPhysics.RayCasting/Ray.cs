using MotusPhysics.Core.Utility;

namespace MotusPhysics.RayCasting;

public class Ray
{
    public readonly Vector Origin;
    public readonly Vector Direction;
    public readonly double MaxDistance;

    public Ray(Vector origin, Vector direction)
    {
        Origin = origin;
        Direction = direction;
        MaxDistance = double.MaxValue;
    }
    
    public Ray(Vector origin, Vector direction, double maxDistance)
    {
        Origin = origin;
        Direction = direction.Normalized();
        MaxDistance = maxDistance;
    }
}