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
    
    
    private int _rigidBodyIdTracker = 0;
    private readonly List<RigidBody> _rigidBodies = new List<RigidBody>();
    //Buffers
    private readonly List<RigidBody> _newRigidBodiesBuffer = new List<RigidBody>();
    private readonly List<RigidBody> _removeRigidBodiesBuffer = new List<RigidBody>();
    
    public List<CollisionManifold> Manifolds { get; private set; } = new List<CollisionManifold>();
    private bool _clearSimulation = false;

    public Vector DefaultGravity { get; private set; } = new Vector(0, 0);
    
    /// <summary>
    /// Schedule a rigidBody to be added into the system.
    /// The actual addition will occur on the following physics step.
    /// </summary>
    /// <param name="rigidBody"></param>
    internal void AddRigidBody(RigidBody rigidBody)
    {
        _newRigidBodiesBuffer.Add(rigidBody);
    }
    /// <summary>
    ///  Schedule a rigidBody to be removed from the system.
    /// The actual removal will occur on the following physics step.
    /// </summary>
    /// <param name="rigidBody"></param>
    internal void RemoveRigidBody(RigidBody rigidBody)
    {
        _removeRigidBodiesBuffer.Add(rigidBody);
    }
    
    public List<RigidBody> GetRigidbodies()
    {
        return new List<RigidBody>(_rigidBodies);
    }

    public void SetDefaultGravity(Vector gravity)
    {
        DefaultGravity = gravity;
    }
    
    internal int GetUniqueRigidBodyId()
    {
        //Return an id and post-increment the id tracker
        return _rigidBodyIdTracker++;
    }

    internal void Update()
    {
        //Add and/or remove scheduled rigidbodies
        HandleBuffers();
        
        //Update all active rigidbodies
        foreach (RigidBody rigidbody in _rigidBodies)
        {
            if (!rigidbody.IsActive)
                continue;
            rigidbody.Update();
        }
        
        //Obtain all collision events to be handled through SAT collision detection
        CollisionEvent[] collisionEvents = SATCollisionDetector.CheckCollision(_rigidBodies);
        
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
            _rigidBodies.Clear();
            _newRigidBodiesBuffer.Clear();
            _removeRigidBodiesBuffer.Clear();
            _clearSimulation = false;
        }
    }

    private void HandleBuffers()
    {
        List<RigidBody> newRigidbodies = new List<RigidBody>(_newRigidBodiesBuffer);
        List<RigidBody> removeRigidbodies = new List<RigidBody>(_removeRigidBodiesBuffer);
        
        _rigidBodies.AddRange(newRigidbodies);
        _rigidBodies.RemoveRange(removeRigidbodies);
        
        //Remove objects that have been handled from the buffers
        _newRigidBodiesBuffer.RemoveRange(newRigidbodies);
        _removeRigidBodiesBuffer.RemoveRange(removeRigidbodies);
    }

    public void ClearSimulation()
    {
        _clearSimulation = true;
    }
}