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
    private ICollisionSolver _collisionSolver = new ImpulseSolver();
    
    private int _physicsObjectIdTracker = 0;
    private readonly List<PhysicsObject> _physicsObjects = new List<PhysicsObject>();
    //Buffers
    private readonly List<PhysicsObject> _newPhysicsObjectsBuffer = new List<PhysicsObject>();
    private readonly List<PhysicsObject> _removePhysicsObjectsBuffer = new List<PhysicsObject>();
    
    public List<CollisionManifold> Manifolds { get; private set; } = new List<CollisionManifold>();
    
    public void AddPhysicsObject(PhysicsObject physicsObject)
    {
        _newPhysicsObjectsBuffer.Add(physicsObject);
    }

    public void RemovePhysicsObject(PhysicsObject physicsObject)
    {
        _removePhysicsObjectsBuffer.Add(physicsObject);
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
        HandleBuffers();
        
        foreach (PhysicsObject physicsObject in _physicsObjects)
        {
            if (!physicsObject.IsActive)
                continue;
            physicsObject.Update();
        }

        List<CollisionManifold> newManifolds = new List<CollisionManifold>();
        List<CollisionEvent> collisionEvents = new List<CollisionEvent>();
        
        //Obtain all collision events to be handled
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
                
                if (_collisionDetector.CheckCollision(_physicsObjects[i], _physicsObjects[j], out CollisionEvent? collisionEvent))
                    collisionEvents.Add(collisionEvent);
            }
        }

        //Separate out overlapping colliders
        foreach (CollisionEvent collisionEvent in collisionEvents)
            CollisionSeparator.SeparateCollisionBodies(collisionEvent.PhysicsObject1, collisionEvent.PhysicsObject2, collisionEvent.CollisionNormal, collisionEvent.PenetrationDepth);

        //Find contact points of each collision
        foreach (CollisionEvent collisionEvent in collisionEvents)
        {
            Vector[] contactPoints = ContactPointFinder.FindContactPoints(collisionEvent.PhysicsObject1.Collider, collisionEvent.PhysicsObject2.Collider);
            CollisionManifold manifold = new CollisionManifold(collisionEvent, contactPoints, PhysiX.Time.SimStep);
            newManifolds.Add(manifold);
            manifold.PhysicsObject1.CollisionTrack(manifold);
            manifold.PhysicsObject2.CollisionTrack(manifold);
        }
        
        Manifolds = newManifolds;
        
        foreach (PhysicsObject physicsObject in _physicsObjects)
        {
            if (!physicsObject.IsActive)
                continue;
            physicsObject.LateUpdate();
        }
    }

    private void HandleBuffers()
    {
        List<PhysicsObject> newObjects = new List<PhysicsObject>(_newPhysicsObjectsBuffer);
        List<PhysicsObject> removeObjects = new List<PhysicsObject>(_removePhysicsObjectsBuffer);
        
        _physicsObjects.AddRange(newObjects);
        _physicsObjects.RemoveRange(removeObjects);
        
        //Remove objects that have been handled from the buffers
        _newPhysicsObjectsBuffer.RemoveRange(newObjects);
        _removePhysicsObjectsBuffer.RemoveRange(removeObjects);
    }
}