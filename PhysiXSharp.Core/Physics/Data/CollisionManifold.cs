﻿
using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Data;

public class CollisionManifold
{
    public readonly PhysicsObject PhysicsObject1;
    public readonly PhysicsObject PhysicsObject2;
    public readonly Vector CollisionNormal;
    public readonly double PenetrationDepth;
    public readonly Vector[] ContactPoints;


    public CollisionManifold(PhysicsObject physicsObject1, PhysicsObject physicsObject2, Vector normal, double depth, Vector[] contactPoints)
    {
        PhysicsObject1 = physicsObject1;
        PhysicsObject2 = physicsObject2;
        CollisionNormal = normal;
        PenetrationDepth = depth;
        ContactPoints = contactPoints;
    }
    
    public CollisionManifold(CollisionEvent collisionEvent, Vector[] contactPoints) 
    {
        PhysicsObject1 = collisionEvent.PhysicsObject1;
        PhysicsObject2 = collisionEvent.PhysicsObject2;
        CollisionNormal = collisionEvent.CollisionNormal;
        PenetrationDepth = collisionEvent.PenetrationDepth;
        ContactPoints = contactPoints;
    }
}