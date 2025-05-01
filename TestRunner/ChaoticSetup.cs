using MotusPhysics.Core.Physics;
using MotusPhysics.Core.Physics.Colliders;
using MotusPhysics.Core.Physics.Data;
using MotusPhysics.Core.Utility;

namespace TestRunner;

public class ChaoticSetup
{
    public static void Setup()
    { 
        RigidBody rbA = RigidBody.CreateRigidBody(new Vector(8, 4), 0d, Collider.CreateCircleCollider(1d), Vector.Zero, 0d, 5d, 1d, 0.1d, 0d, 0.4d);
        rbA.SetVelocity(new Vector(2d, 0d));
        rbA.SetGravity(new Vector(0d, 3d));
        
        RigidBody rbB = RigidBody.CreateRigidBody(new Vector(24, 4), 0d, Collider.CreateCircleCollider(1d), Vector.Zero, 0d, 1d, 1d, 0.1d, 0d, 0.4d);
        rbB.SetVelocity(new Vector(-1d, 0d));
        rbB.SetGravity(new Vector(0d, 3d));
        
        RigidBody rbC = RigidBody.CreateRigidBody(new Vector(8, 8), 60d, Collider.CreateRectangleCollider(new Vector(2d, 1d)), Vector.Zero, 0d, 1d, 1d, 0.1d, 0d, 0.4d);
        rbC.SetVelocity(new Vector(5d, 0d));
        rbC.SetGravity(new Vector(0d, 3d));
        
        RigidBody rbD = RigidBody.CreateRigidBody(new Vector(24, 8), 0d, Collider.CreateCircleCollider(1.2d), Vector.Zero, 0d, 1d, 1d, 0.1d, 0d, 0.4d);
        rbD.SetVelocity(new Vector(-1.5d, 0d));
        rbD.SetGravity(new Vector(0d, 3d));
        
        RigidBody rbE = RigidBody.CreateRigidBody(new Vector(8, 12), 15d, Collider.CreateRectangleCollider(new Vector(2.5d, 1d)), Vector.Zero, 0d, 1d, 1d, 0.1d, 0d, 0.4d);
        rbE.SetVelocity(new Vector(3d, 0d));
        rbE.SetGravity(new Vector(0d, 3d));
        
        RigidBody rbF = RigidBody.CreateRigidBody(new Vector(24, 12), 45d, Collider.CreateRectangleCollider(new Vector(1.5, 1.5)), Vector.Zero, 0d, 2d, 1d, 0.1d, 0d, 0.4d);
        rbF.SetVelocity(new Vector(-3d, 0d));
        rbF.SetGravity(new Vector(0d, 3d));
        
        RigidBody rbG = RigidBody.CreateRigidBody(new Vector(8, 16), 0d, 
            Collider.CreatePolygonCollider(
            new Vector(1, 1),
            new Vector(1, -1),
            new Vector(0, -1.5),
            new Vector(-1, -1),
            new Vector(-1, 1)
            ), Vector.Zero, 0d, 1d, 1d, 0.1d, 0d, 0.4d);
        rbG.SetVelocity(new Vector(4d, 0d));
        rbG.SetGravity(new Vector(0d, 3d));
        
        RigidBody rbH = RigidBody.CreateRigidBody(new Vector(24, 16), 0d, 
            Collider.CreatePolygonCollider(
            new Vector(1.5, 1.5),
                new Vector(2, -1),
                new Vector(0, -2),
                new Vector(-1.5, -1.25),
                new Vector(-2.25, 1.75)
            ), Vector.Zero, 0d, 1d, 1d, 0.1d, 0d, 0.4d);
        rbH.SetVelocity(new Vector(-0.25d, 0d));
        rbH.SetGravity(new Vector(0d, 3d));
        
        RigidBody floor = RigidBody.CreateStaticBody(new Vector(16, 22), 0d, Collider.CreateRectangleCollider(new Vector(28, 2)));

        RigidBody rbX = RigidBody.CreateRigidBody(Collider.CreateCircleCollider(1d), position: new Vector(16, 4), drag: 0.1d);
        rbX.SetGravity(new Vector(0d, 3d));
        rbX.OnCollisionEnterSubscribe(manifold => { Console.WriteLine($"Collision contact point: {manifold.RigidBodyA.Id}");});
    }
}