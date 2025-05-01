using MotusPhysics.Core.Physics;
using MotusPhysics.Core.Utility;

namespace MotusPhysics.RayCasting;

public class RayCastHit
{
    public readonly RigidBody Body;
    public readonly Vector Point;
    public readonly Vector Normal;

    public RayCastHit(RigidBody rigidBody, Vector point, Vector normal)
    {
        Body = rigidBody;
        Point = point;
        Normal = normal;

    }

    public override string ToString()
    {
        return "Body: " + Body.Id + " | point: " + Point + " | normal: " + Normal;
    }
}