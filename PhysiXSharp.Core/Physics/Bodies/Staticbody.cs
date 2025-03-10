using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Bodies;

public sealed class Staticbody : PhysicsObject
{
    public Staticbody(Vector position, float rotation)
    {
        IsStatic = true;
        Position = position;
        Rotation = rotation;
    }
}