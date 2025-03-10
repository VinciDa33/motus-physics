using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Physics.Collision;
using PhysiXSharp.Core.Physics.Data;
using PhysiXSharp.Core.Utility;

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
    public List<CollisionManifold> Manifolds { get; private set; } = new List<CollisionManifold>();
    
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
            if (!rigidbody.IsActive)
                continue;
            rigidbody.Update();
        }

        List<CollisionManifold> newManifolds = new List<CollisionManifold>();
        for (int i = 0; i < _physicsObjects.Count; i++)
        {
            for (int j = i + 1; j < _physicsObjects.Count; j++)
            {
                //Skip collisions with inactive objects
                if (!_physicsObjects[i].IsActive || !_physicsObjects[j].IsActive)
                    continue;
                
                //Skip collisions between static objects
                if (_physicsObjects[i] is not Rigidbody && _physicsObjects[j] is not Rigidbody)
                    continue;
                
                if (_collisionDetector.CheckCollision(_physicsObjects[i], _physicsObjects[j], out CollisionManifold? manifold))
                {
                    newManifolds.Add(manifold);
                    //SeparateBodies(manifold.PhysicsObject1, manifold.PhysicsObject2, manifold.CollisionNormal * manifold.PenetrationDepth);
                }
            }
        }

        Manifolds = newManifolds;
    }
}