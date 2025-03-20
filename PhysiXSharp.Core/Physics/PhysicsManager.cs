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
    
    
    private int _rigidbodyIdTracker = 0;
    private readonly List<Rigidbody> _rigidbodies = new List<Rigidbody>();
    //Buffers
    private readonly List<Rigidbody> _newRigidbodiesBuffer = new List<Rigidbody>();
    private readonly List<Rigidbody> _removeRigidbodiesBuffer = new List<Rigidbody>();
    
    public List<CollisionManifold> Manifolds { get; private set; } = new List<CollisionManifold>();
    
    /// <summary>
    /// Schedule a rigidbody to be added into the system.
    /// The actual addition will occur on the following physics step.
    /// </summary>
    /// <param name="rigidbody"></param>
    public void AddRigidbody(Rigidbody rigidbody)
    {
        _newRigidbodiesBuffer.Add(rigidbody);
    }
    /// <summary>
    ///  Schedule a rigidbody to be removed from the system.
    /// The actual removal will occur on the following physics step.
    /// </summary>
    /// <param name="rigidbody"></param>
    public void RemoveRigidbody(Rigidbody rigidbody)
    {
        _removeRigidbodiesBuffer.Add(rigidbody);
    }

    public List<Rigidbody> GetRigidbodies()
    {
        return new List<Rigidbody>(_rigidbodies);
    }
    
    public int GetUniqueRigidbodyId()
    {
        //Return an id and post-increment the id tracker
        return _rigidbodyIdTracker++;
    }

    public void Update()
    {
        //Add and/or remove scheduled rigidbodies
        HandleBuffers();
        
        //Update all active rigidbodies
        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            if (!rigidbody.IsActive)
                continue;
            rigidbody.Update();
        }
        
        //Obtain all collision events to be handled
        List<CollisionEvent> collisionEvents = new List<CollisionEvent>();
        for (int i = 0; i < _rigidbodies.Count - 1; i++)
        {
            for (int j = i + 1; j < _rigidbodies.Count; j++)
            {
                //Skip collisions with inactive objects
                if (!_rigidbodies[i].IsActive || !_rigidbodies[j].IsActive)
                    continue;
                
                //Skip collisions between static objects
                if (_rigidbodies[i].IsStatic && _rigidbodies[j].IsStatic)
                    continue;
                
                if (SATCollisionDetector.CheckCollision(_rigidbodies[i], _rigidbodies[j], out CollisionEvent? collisionEvent))
                    collisionEvents.Add(collisionEvent);
            }
        }

        
        //Separate out overlapping colliders
        foreach (CollisionEvent collisionEvent in collisionEvents)
            CollisionSeparator.SeparateCollisionBodies(collisionEvent.RigidbodyA, collisionEvent.RigidbodyB, collisionEvent.CollisionNormal, collisionEvent.PenetrationDepth);

        
        List<CollisionManifold> newManifolds = new List<CollisionManifold>();
        //Find contact points of each collision
        foreach (CollisionEvent collisionEvent in collisionEvents)
        {
            Vector[] contactPoints = ContactPointFinder.FindContactPoints(collisionEvent.RigidbodyA.Collider, collisionEvent.RigidbodyB.Collider);
            
            CollisionManifold manifold = new CollisionManifold(collisionEvent, contactPoints, PhysiX.Time.SimStep);
            newManifolds.Add(manifold);
            manifold.RigidbodyA.OnCollision(manifold);
            manifold.RigidbodyB.OnCollision(manifold);
        }
        
        Manifolds = newManifolds;

        
        List<CollisionResolution> resolutions = new List<CollisionResolution>();
        foreach (CollisionManifold manifold in Manifolds)
        {
            CollisionResolution resolution = ImpulseSolver.SolveCollision(manifold);
            resolutions.Add(resolution);
        }

        foreach (CollisionResolution resolution in resolutions)
        {
            resolution.RigidbodyA.AddVelocity(resolution.VelocityChangeA);
            //resolution.RigidbodyA.AddAngularVelocity(resolution.AngularVelocityChangeA);
            
            resolution.RigidbodyB.AddVelocity(resolution.VelocityChangeB);
            //resolution.RigidbodyB.AddAngularVelocity(resolution.AngularVelocityChangeB);
        }
        
        
        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            if (!rigidbody.IsActive)
                continue;
            rigidbody.LateUpdate();
        }
    }

    private void HandleBuffers()
    {
        List<Rigidbody> newRigidbodies = new List<Rigidbody>(_newRigidbodiesBuffer);
        List<Rigidbody> removeRigidbodies = new List<Rigidbody>(_removeRigidbodiesBuffer);
        
        _rigidbodies.AddRange(newRigidbodies);
        _rigidbodies.RemoveRange(removeRigidbodies);
        
        //Remove objects that have been handled from the buffers
        _newRigidbodiesBuffer.RemoveRange(newRigidbodies);
        _removeRigidbodiesBuffer.RemoveRange(removeRigidbodies);
    }
}