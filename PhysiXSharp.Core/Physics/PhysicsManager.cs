﻿using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics;

public class PhysicsManager
{
    private static PhysicsManager _instance = null!;
    private static readonly object InstanceLock = new ();

    internal static PhysicsManager Instance
    {
        get
        {
            lock (InstanceLock)
            {
                return _instance ??= new PhysicsManager();
            }
        }
    }
    
    private PhysicsManager() {}

    private ICollisionDetector _collisionDetector = new SATCollisionDetector();
    private int _physicsObjectIdTracker = 0;
    private readonly List<PhysicsObject> _physicsObjects = new List<PhysicsObject>();
    private readonly List<Rigidbody> _rigidbodies = new List<Rigidbody>();
    
    public void AddPhysicsObject(PhysicsObject physicsObject)
    {
        _physicsObjects.Add(physicsObject);
    }

    public void RemovePhysicsObject(PhysicsObject physicsObject)
    {
        _physicsObjects.Remove(physicsObject);
    }

    public void AddRigidbody(Rigidbody rigidbody)
    {
        _rigidbodies.Add(rigidbody);
    }

    public void RemoveRigidbody(Rigidbody rigidbody)
    {
        _rigidbodies.Remove(rigidbody);
    }

    public List<PhysicsObject> GetPhysicsObjects()
    {
        return _physicsObjects;
    }
    
    public int GetUniquePhysicsObjectId()
    {
        //Return an id and post-increment the id tracker
        return _physicsObjectIdTracker++;
    }

    public void Update()
    {
        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            rigidbody.Update();
        }
        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            foreach (PhysicsObject physicsObject in _physicsObjects)
            {
                if (rigidbody.Id == physicsObject.Id)
                    continue;
                _collisionDetector.CheckCollision(rigidbody, physicsObject, out CollisionData data);
                if (data.Colliding)
                    SeparateBodies(data.PhysicsObject1, data.PhysicsObject2, data.CollisionNormal * data.PenetrationDepth);
                //bool result = SAT.DoCollision(rigidbody, physicsObject);
                //Console.WriteLine(result);
            }
        }
    }

    private void SeparateBodies(PhysicsObject bodyA, PhysicsObject bodyB, Vector mtv)
    {
        Console.WriteLine(mtv);
        if (bodyA.IsStatic)
        {
            ((Rigidbody) bodyB).TranslatePosition(mtv);
        }
        else if (bodyB.IsStatic)
        {
            ((Rigidbody) bodyA).TranslatePosition(mtv);
        }
        else
        {
            ((Rigidbody) bodyA).TranslatePosition(-mtv / 2d);
            ((Rigidbody) bodyB).TranslatePosition(mtv / 2d);
        }
    }
    
}