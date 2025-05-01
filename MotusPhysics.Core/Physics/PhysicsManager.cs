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
    private readonly List<RigidBody> _rigidbodies = new List<RigidBody>();
    //Buffers
    private readonly List<RigidBody> _newRigidbodiesBuffer = new List<RigidBody>();
    private readonly List<RigidBody> _removeRigidbodiesBuffer = new List<RigidBody>();
    
    public List<CollisionManifold> Manifolds { get; private set; } = new List<CollisionManifold>();
    private bool _clearSimulation = false;
    
    /// <summary>
    /// Schedule a rigidBody to be added into the system.
    /// The actual addition will occur on the following physics step.
    /// </summary>
    /// <param name="rigidBody"></param>
    internal void AddRigidbody(RigidBody rigidBody)
    {
        _newRigidbodiesBuffer.Add(rigidBody);
    }
    /// <summary>
    ///  Schedule a rigidBody to be removed from the system.
    /// The actual removal will occur on the following physics step.
    /// </summary>
    /// <param name="rigidBody"></param>
    internal void RemoveRigidbody(RigidBody rigidBody)
    {
        _removeRigidbodiesBuffer.Add(rigidBody);
    }
    
    public List<RigidBody> GetRigidbodies()
    {
        return new List<RigidBody>(_rigidbodies);
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
        foreach (RigidBody rigidbody in _rigidbodies)
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
            Vector[] contactPoints = ContactPointFinder.FindContactPoints(collisionEvent.RigidBodyA.Collider, collisionEvent.RigidBodyB.Collider);
            
            CollisionManifold manifold = new CollisionManifold(collisionEvent, contactPoints, Motus.Time.SimStep);
            newManifolds.Add(manifold);
            manifold.RigidBodyA.OnCollision(manifold);
            manifold.RigidBodyB.OnCollision(manifold);
        }
        
        Manifolds = newManifolds;

        //Solve applied forces on each rigidBody
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
        List<RigidBody> newRigidbodies = new List<RigidBody>(_newRigidbodiesBuffer);
        List<RigidBody> removeRigidbodies = new List<RigidBody>(_removeRigidbodiesBuffer);
        
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