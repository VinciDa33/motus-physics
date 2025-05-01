using MotusPhysics.Core.Physics;
using MotusPhysics.Core.Physics.Colliders;
using MotusPhysics.Core.Utility;

namespace TestRunner;

public class RayCastSetup
{
    public static void Setup()
    {
        RigidBody rbA = RigidBody.CreateStaticBody(new Vector(26, 8), 0, Collider.CreateRectangleCollider(new Vector(3d, 3d)));
        RigidBody rbB = RigidBody.CreateStaticBody(new Vector(26, 16), 0d, Collider.CreateCircleCollider(2d));
    }
}