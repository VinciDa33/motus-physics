using MotusPhysics.Core.Utility;

namespace MotusPhysics.Core.Physics.Colliders;

public class AABB
{
    /// <summary>
    /// The point of minimum x and y values of the collision shape relative to its position
    /// </summary>
    public readonly Vector Min;
    
    /// <summary>
    /// The point of maximum x and y values of the collision shape relative to its position
    /// </summary>
    public readonly Vector Max;

    public AABB(Vector min, Vector max)
    {
        Min = min;
        Max = max;
    }
}