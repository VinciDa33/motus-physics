﻿using MotusPhysics.Core.Physics;
using MotusPhysics.Core.Physics.Colliders;
using MotusPhysics.Core.Utility;

namespace TestRunner;

public class StackSetup
{
    public static void Setup()
    {
        Rigidbody floor = Rigidbody.CreateStaticbody(new Vector(16, 22), 0d, Collider.CreateRectangleCollider(new Vector(28, 2)));
        
        Rigidbody rbA = Rigidbody.CreateRigidbody(new Vector(16, 20), 0d, Collider.CreateRectangleCollider(new Vector(3d, 1d)), 1d, 1d, 0.2d);
        rbA.SetGravity(new Vector(0d, 2d));
        
        Rigidbody rbB = Rigidbody.CreateRigidbody(new Vector(16, 18), 0d, Collider.CreateRectangleCollider(new Vector(3d, 1d)), 1d, 1d, 0.2d);
        rbB.SetGravity(new Vector(0d, 2d));
        
        Rigidbody rbC = Rigidbody.CreateRigidbody(new Vector(16, 16), 0d, Collider.CreateRectangleCollider(new Vector(3d, 1d)), 1d, 1d, 0.2d);
        rbC.SetGravity(new Vector(0d, 2d));
        
        Rigidbody rbD = Rigidbody.CreateRigidbody(new Vector(16, 14), 0d, Collider.CreateRectangleCollider(new Vector(3d, 1d)), 1d, 1d, 0.2d);
        rbD.SetGravity(new Vector(0d, 2d));
        
        Rigidbody rbE = Rigidbody.CreateRigidbody(new Vector(16, 12), 0d, Collider.CreateRectangleCollider(new Vector(3d, 1d)), 1d, 1d, 0.2d);
        rbE.SetGravity(new Vector(0d, 2d));
    }
}