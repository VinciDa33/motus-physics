using MotusPhysics.Core.Physics.Collision;
using MotusPhysics.Core.Physics.Data;
using MotusPhysics.Core.Utility;

namespace MotusPhysics.Core.Physics;

public class PhysicsManager
{
    private static PhysicsManager? _instance = null;
    private static readonly object InstanceLock = new ();

    public static PhysicsManager Instance
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
    private bool _clearSimulation = false;
    
    /// <summary>
    /// Schedule a rigidbody to be added into the system.
    /// The actual addition will occur on the following physics step.
    /// </summary>
    /// <param name="rigidbody"></param>
    internal void AddRigidbody(Rigidbody rigidbody)
    {
        _newRigidbodiesBuffer.Add(rigidbody);
    }
    /// <summary>
    ///  Schedule a rigidbody to be removed from the system.
    /// The actual removal will occur on the following physics step.
    /// </summary>
    /// <param name="rigidbody"></param>
    internal void RemoveRigidbody(Rigidbody rigidbody)
    {
        _removeRigidbodiesBuffer.Add(rigidbody);
    }
    
    public List<Rigidbody> GetRigidbodies()
    {
        return new List<Rigidbody>(_rigidbodies);
    }
    
    internal int GetUniqueRigidbodyId()
    {
        //Return an id and post-increment the id tracker
        return _rigidbodyIdTracker++;
    }

    internal void Update()
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
        
        //Obtain all collision events to be handled through SAT collision detection
        CollisionEvent[] collisionEvents = SATCollisionDetector.CheckCollision(_rigidbodies);
        
        //Separate out overlapping colliders using the data gained using SAT
        CollisionSeparator.SeparateCollisionBodies(collisionEvents);

        
        List<CollisionManifold> newManifolds = new List<CollisionManifold>();
        //Find contact points of each collision
        foreach (CollisionEvent collisionEvent in collisionEvents)
        {
            Vector[] contactPoints = ContactPointFinder.FindContactPoints(collisionEvent.RigidbodyA.Collider, collisionEvent.RigidbodyB.Collider);
            
            CollisionManifold manifold = new CollisionManifold(collisionEvent, contactPoints, Motus.Time.SimStep);
            newManifolds.Add(manifold);
            manifold.RigidbodyA.OnCollision(manifold);
            manifold.RigidbodyB.OnCollision(manifold);
        }
        
        Manifolds = newManifolds;

        //Solve applied forces on each rigidbody
        ImpulseSolver.SolveCollisions(Manifolds.ToArray());

        //Clear out the simulation if the clear flag has been set
        if (_clearSimulation)
        {
            _rigidbodies.Clear();
            _newRigidbodiesBuffer.Clear();
            _removeRigidbodiesBuffer.Clear();
            _clearSimulation = false;
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

    public void ClearSimulation()
    {
        _clearSimulation = true;
    }
}