using MotusPhysics.Core.Physics;
using MotusPhysics.Core.Physics.Colliders;
using MotusPhysics.Core.Utility;

namespace TestRunner;

public class SimpleCollisionSetup
{
    public static void Setup()
    {

        Rigidbody rbA = Rigidbody.CreateRigidbody(new Vector(8, 6.5), 0, Collider.CreateRectangleCollider(new Vector(1d, 3d)), 1d, 1d, 0.1d);
        rbA.SetVelocity(new Vector(10d, 0d));
        
        Rigidbody rbB = Rigidbody.CreateRigidbody(new Vector(24, 9), 0d, Collider.CreateRectangleCollider(new Vector(1d, 3d)), 1d, 1d, 0.1d);
        rbB.SetVelocity(new Vector(-4d, 0d));
        
        Rigidbody rbC = Rigidbody.CreateRigidbody(new Vector(8, 16), 0d, Collider.CreateCircleCollider(1d), 1d, 1d, 0.4d);
        rbC.SetVelocity(new Vector(4d, 0d));

        Rigidbody rbD = Rigidbody.CreateRigidbody(new Vector(24, 16), 0d, Collider.CreateCircleCollider(0.8d), 1d, 1d, 0.4d);
        rbD.SetVelocity(new Vector(-4d, 0d));

    }
}