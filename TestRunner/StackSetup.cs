using MotusPhysics.Core.Physics;
using MotusPhysics.Core.Physics.Colliders;
using MotusPhysics.Core.Utility;

namespace TestRunner;

public class StackSetup
{
    public static void Setup()
    {
        RigidBody floor = RigidBody.CreateStaticBody(new Vector(16, 22), 0d, Collider.CreateRectangleCollider(new Vector(28, 2)));
        
        RigidBody rbA = RigidBody.CreateRigidBody(new Vector(16, 20), 0d, Collider.CreateRectangleCollider(new Vector(3d, 1d)), 1d, 1d, 0.2d);
        rbA.SetGravity(new Vector(0d, 2d));
        
        RigidBody rbB = RigidBody.CreateRigidBody(new Vector(16, 18), 0d, Collider.CreateRectangleCollider(new Vector(3d, 1d)), 1d, 1d, 0.2d);
        rbB.SetGravity(new Vector(0d, 2d));
        
        RigidBody rbC = RigidBody.CreateRigidBody(new Vector(16, 16), 0d, Collider.CreateRectangleCollider(new Vector(3d, 1d)), 1d, 1d, 0.2d);
        rbC.SetGravity(new Vector(0d, 2d));
        
        RigidBody rbD = RigidBody.CreateRigidBody(new Vector(16, 14), 0d, Collider.CreateRectangleCollider(new Vector(3d, 1d)), 1d, 1d, 0.2d);
        rbD.SetGravity(new Vector(0d, 2d));
        
        RigidBody rbE = RigidBody.CreateRigidBody(new Vector(16, 12), 0d, Collider.CreateRectangleCollider(new Vector(3d, 1d)), 1d, 1d, 0.2d);
        rbE.SetGravity(new Vector(0d, 2d));
    }
}