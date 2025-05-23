﻿using MotusPhysics.Core.Physics;
using MotusPhysics.Core.Physics.Colliders;
using MotusPhysics.Core.Utility;

namespace TestRunner;

public class StressSetup
{
    public static void Setup()
    {
        int rigidbodyCount = 100;
        
        RigidBody floor = RigidBody.CreateStaticBody(Collider.CreateRectangleCollider(new Vector(28, 1)), position: new Vector(16, 22), restitution: 1d);
        RigidBody wallLeft = RigidBody.CreateStaticBody(Collider.CreateRectangleCollider(new Vector(1, 20)), position: new Vector(2, 12), restitution: 1d);
        RigidBody wallRight = RigidBody.CreateStaticBody(Collider.CreateRectangleCollider(new Vector(1, 20)), position: new Vector(30, 12), restitution: 1d);
        RigidBody roof = RigidBody.CreateStaticBody(Collider.CreateRectangleCollider(new Vector(28, 1)), position: new Vector(16, 2), restitution: 1d);

        Random rand = new Random();
        
        for (int i = 0; i < rigidbodyCount; i++)
        {
            int r = rand.Next(0, 2);
            Vector pos = new Vector(rand.Next(4, 29), rand.Next(4, 21));
            Vector initialVel = new Vector(rand.Next(-2, 3), rand.Next(-2, 3));
            if (r == 0)
            {
                double radius = 0.4d + rand.NextDouble() * 0.6d;
                RigidBody.CreateRigidBody(Collider.CreateCircleCollider(radius), position: pos, initialVelocity: initialVel, restitution: 1d);
            }
            else if (r == 1)
            {
                Vector size = new Vector(0.5d + rand.NextDouble(), 0.5d + rand.NextDouble());
                RigidBody.CreateRigidBody(Collider.CreateRectangleCollider(size), position: pos, initialVelocity: initialVel, restitution: 1d);
            }
        }
    }
}