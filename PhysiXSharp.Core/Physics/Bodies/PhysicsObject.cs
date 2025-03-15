using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Physics.Data;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Bodies;

public abstract class PhysicsObject
{
    public readonly int Id;
    public bool IsActive { get; protected set; } = true;
    public bool IsStatic { get; protected set; }
    public Vector Position { get; protected set; } = new Vector(0d, 0d);
    public float Rotation { get; protected set; } = 0f;
    public Collider? Collider { get; private set; }
    private readonly List<CollisionTracker> _currentCollisions = new List<CollisionTracker>();
    
    public delegate void CollisionDelegate(CollisionManifold manifold);
    private event CollisionDelegate OnCollisionEnter;
    private event CollisionDelegate OnCollisionExit;
    
    
    protected PhysicsObject()
    {
        Id = PhysicsManager.Instance.GetUniquePhysicsObjectId();
        PhysicsManager.Instance.AddPhysicsObject(this);
    }

    internal virtual void Update()
    {

    }

    internal virtual void LateUpdate()
    {
        List<CollisionTracker> expiredTrackers = new List<CollisionTracker>();
        foreach (CollisionTracker tracker in _currentCollisions)
        {
            if (tracker.CollisionEntry)
            {
                OnCollisionEnter?.Invoke(tracker.Manifold);
                tracker.CollisionEntry = false;
            }
            
            if (tracker.SimStep < PhysiX.Time.SimStep)
            {
                OnCollisionExit?.Invoke(tracker.Manifold);
                expiredTrackers.Add(tracker);
            }
        }
        _currentCollisions.RemoveRange(expiredTrackers);
    }
    
    public void SetActive(bool b)
    {
        IsActive = b;
    }

    public void AddCollider(Collider collider)
    {
        Collider = collider;
        Collider.SetPhysicsObject(this);
        if (Rotation != 0)
            Collider.SetRotation(Rotation);
    }

    public void RemoveCollider()
    {
        Collider = null;
    }

    public int GetCurrentCollisionCount()
    {
        return _currentCollisions.Count;
    }

    public void OnCollisionEnterSubscribe(CollisionDelegate handler)
    {
        OnCollisionEnter += handler;
    } 
    public void OnCollisionEnterUnsubscribe(CollisionDelegate handler)
    {
        OnCollisionEnter -= handler;
    } 
    public void OnCollisionExitSubscribe(CollisionDelegate handler)
    {
        OnCollisionExit += handler;
    } 
    public void OnCollisionExitUnsubscribe(CollisionDelegate handler)
    {
        OnCollisionExit -= handler;
    } 
    
    public virtual void Destroy()
    {
        PhysicsManager.Instance.RemovePhysicsObject(this);
    }

    internal void CollisionTrack(CollisionManifold manifold)
    {
        foreach (CollisionTracker tracker in _currentCollisions)
        {
            //Check if a collision between 2 objects is already ongoing
            if ((tracker.Manifold.PhysicsObject1.Id == manifold.PhysicsObject1.Id && tracker.Manifold.PhysicsObject2.Id == manifold.PhysicsObject2.Id) 
                || (tracker.Manifold.PhysicsObject1.Id == manifold.PhysicsObject2.Id && tracker.Manifold.PhysicsObject2.Id == manifold.PhysicsObject1.Id))
            {
                //Update tracker data
                tracker.Manifold = manifold;
                tracker.SimStep = manifold.SimStepStamp;
                return;
            }
        }
        
        //If no tracker already existed, track a new collision
        _currentCollisions.Add(new CollisionTracker(manifold, manifold.SimStepStamp, true));
    }

    private class CollisionTracker(CollisionManifold manifold, int simStep, bool collisionEntry)
    {
        public CollisionManifold Manifold = manifold;
        public int SimStep = simStep;
        public bool CollisionEntry = collisionEntry;
    }
    
}