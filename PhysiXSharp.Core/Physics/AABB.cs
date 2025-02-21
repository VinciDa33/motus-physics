using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics;

public class AABB
{
    /// <summary>
    /// The leftmost and lowest point of the axis-aligned bounding box
    /// </summary>
    public readonly Vector Origin;
    
    /// <summary>
    /// The rightmost and highest point relative to the origin point
    /// </summary>
    public readonly Vector Size;

    public AABB(Vector origin, Vector size)
    {
        Origin = origin;
        Size = size;
    }
}