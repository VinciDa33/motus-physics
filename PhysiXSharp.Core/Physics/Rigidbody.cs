using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics;

public abstract class Rigidbody : PhysicsObject
{
    protected Vector Velocity = new Vector(0d, 0d);
}