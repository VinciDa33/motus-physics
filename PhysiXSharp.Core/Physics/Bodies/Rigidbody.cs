using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Physics.Data;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Bodies;

public sealed class Rigidbody
{
    
    public readonly int Id;
    public bool IsActive { get; private set; } = true;
    public bool IsStatic { get; private set; }
    public Vector Position { get; private set; }
    public Vector Velocity { get; private set; }
    public double Rotation { get; private set; } = 0f;
    public double AngularVelocity { get; private set; } = 0f;
    public double Mass { get; private set; } = 1d;
    public double InverseMass { get; private set; } = 1d;
    public double Inertia { get; private set; } = 1d;
    public double InverseInertia { get; private set; } = 1d;
    public Vector Gravity { get; private set; }
    
    /// <summary>
    /// The bounciness of the object, upon collision.
    /// 0 -> Perfect inelastic (objects will stick)
    /// 1 -> Perfect elastic (full bounce, no energy loss)
    /// </summary>
    public double Restitution { get; private set; } = 0.5d;
    public Collider Collider { get; private set; }
    
    public delegate void CollisionDelegate(CollisionManifold manifold);
    private event CollisionDelegate OnCollisionEnter;
    private event CollisionDelegate OnCollisionExit;

    private Dictionary<(int, int), CollisionManifold> currentCollisions =
        new Dictionary<(int, int), CollisionManifold>();
    
    
    private Rigidbody(Vector position, double rotation, Vector initialVelocity, double initialAngularVelocity, double mass, double inertia, Vector gravity, double restitution, Collider collider, bool staticBody)
    {
        Id = PhysicsManager.Instance.GetUniqueRigidbodyId();
        PhysicsManager.Instance.AddRigidbody(this);
        
        IsStatic = staticBody;
        Position = position;
        Velocity = initialVelocity;
        Rotation = rotation;
        AngularVelocity = initialAngularVelocity;
        Mass = mass;
        InverseMass = mass <= 0 ? 0d : 1d / mass;
        Inertia = inertia;
        InverseInertia = inertia <= 0 ? 0d : 1d / inertia;
        Gravity = gravity;
        Restitution = restitution;
        Collider = collider;
        collider.SetRigidbody(this);
    }

    public void Destroy()
    {
        PhysicsManager.Instance.RemoveRigidbody(this);
    }
    
    internal void Update()
    {
        if (!IsActive)
            return;
        
        TranslatePosition(Velocity * PhysiX.Time.FixedTimeStep * PhysiX.Time.TimeScale);
        Rotate((float) (AngularVelocity * PhysiX.Time.FixedTimeStep * PhysiX.Time.TimeScale));
        Velocity += Gravity * PhysiX.Time.FixedTimeStep * PhysiX.Time.TimeScale;
        
        Collider.UpdateRotation();
    }

    internal void LateUpdate()
    {
        foreach(KeyValuePair<(int, int), CollisionManifold> entry in currentCollisions)
        {
            //Discard old collisions that have not been refreshed
            if (entry.Value.SimStep < PhysiX.Time.SimStep)
            {
                OnCollisionExit?.Invoke(entry.Value);
                currentCollisions.Remove(entry.Key);
            }
        }
    }
    
    internal void OnCollision(CollisionManifold manifold)
    {
        //Create a key using the 2 id's in sorted order
        int idA = manifold.RigidbodyA.Id;
        int idB = manifold.RigidbodyB.Id;
        (int, int) key = (Math.Min(idA, idB), Math.Max(idA, idB));
        
        //If the key does not already exist, it must be a new collision
        if (!currentCollisions.ContainsKey(key))
            OnCollisionEnter?.Invoke(manifold);

        currentCollisions[key] = manifold;
    }

    #region Mutators
    public void SetActive(bool value)
    {
        IsActive = value;
    }
    
    /// <summary>
    /// Set the absolute position of the rigidbody to a new position.
    /// </summary>
    /// <param name="position"></param>
    public void SetPosition(Vector position)
    {
        Position = position;
        Collider.CalculateAABB();
    }

    /// <summary>
    /// Moves the rigidbody by the specified translation value.
    /// </summary>
    /// <param name="translation"></param>
    public void TranslatePosition(Vector translation)
    {
        Position += translation;
        Collider.CalculateAABB();
    }
    
    /// <summary>
    /// Sets the absolute rotation of the rigidbody in degrees.
    /// </summary>
    /// <param name="degrees"></param>
    public void SetRotation(float degrees)
    {
        Rotation = degrees;
        Collider.UpdateRotation();
    }

    /// <summary>
    /// Rotates the rigidbody by a specified amount of degrees.
    /// </summary>
    /// <param name="degrees"></param>
    public void Rotate(float degrees)
    {
        Rotation += degrees;
        Collider.UpdateRotation();
    }
    
    public void SetVelocity(Vector velocity)
    {
        Velocity = velocity;
    }

    public void AddVelocity(Vector addition)
    {
        Velocity += addition;
    }
    
    public void SetAngularVelocity(float angularVelocity)
    {
        AngularVelocity = angularVelocity;
    }

    public void AddAngularVelocity(float addition)
    {
        AngularVelocity += addition;
    }

    public void SetGravity(Vector gravity)
    {
        Gravity = gravity;
    }
    
    #endregion


    
    
    
    #region Event Subscription Methods
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
    #endregion
    
    
    
    
    
    #region Factories

    public static Rigidbody CreateRigidbody(Vector position, Collider collider)
    {
        return new Rigidbody(position, 0d, Vector.Zero, 0d, 1d, 1d, Vector.Zero, 0.4d, collider, false);
    }

    public static Rigidbody CreateRigidbody(Vector position, double rotation, Collider collider)
    {
        return new Rigidbody(position, rotation, Vector.Zero, 0d, 1d, 1d, Vector.Zero, 0.4d, collider, false);
    }

    public static Rigidbody CreateRigidbody(Vector position, double rotation, Collider collider, double mass, double inertia,
        double restitution)
    {
        if (mass <= 0d)
        {
            //TODO: Specify grams or kilograms!
            PhysiX.Logger.LogError("Rigidbody mass can not be 0 or less!\nThe mass has been set to 0.05");
            mass = 0.05d;
        }

        if (inertia < 0)
        {
            PhysiX.Logger.LogError("Rigidbody moment of inertia can not be less than 0!\nThe inertia has been set to 0.");
            inertia = 0d;
        }
        
        if (restitution > 1d)
        {
            restitution = 1d;
            PhysiX.Logger.LogWarning("Rigidbody restitution must be between 0 and 1.\nValue has been clamped to 1.");
        }
        
        if (restitution < 0d)
        {
            restitution = 0d;
            PhysiX.Logger.LogWarning("Rigidbody restitution must be between 0 and 1.\nValue has been clamped to 0.");
        }
        
        return new Rigidbody(position, rotation, Vector.Zero, 0d, mass, inertia, Vector.Zero, restitution, collider, false);
    }

    public static Rigidbody CreateRigidbody(Vector position, double rotation, Collider collider, Vector initialVelocity,
        double initialAngularVelocity, double mass, double inertia,
        double restitution)
    {
        if (mass <= 0d)
        {
            //TODO: Specify grams or kilograms!
            PhysiX.Logger.LogError("Rigidbody mass can not be 0 or less!\nThe mass has been set to 0.05");
            mass = 0.05d;
        }

        if (inertia < 0)
        {
            PhysiX.Logger.LogError("Rigidbody moment of inertia can not be less than 0!\nThe inertia has been set to 0.");
            inertia = 0d;
        }
        
        if (restitution > 1d)
        {
            restitution = 1d;
            PhysiX.Logger.LogWarning("Rigidbody restitution must be between 0 and 1.\nValue has been clamped to 1.");
        }
        
        if (restitution < 0d)
        {
            restitution = 0d;
            PhysiX.Logger.LogWarning("Rigidbody restitution must be between 0 and 1.\nValue has been clamped to 0.");
        }
        
        return new Rigidbody(position, rotation, initialVelocity, initialAngularVelocity, mass, inertia, Vector.Zero, restitution, collider, false);
    }
    
    public static Rigidbody CreateStaticbody(Vector position, Collider collider)
    {
        return new Rigidbody(position, 0d, Vector.Zero, 0d, 1d, 1d, Vector.Zero, 0.4d, collider, true);
    }

    public static Rigidbody CreateStaticbody(Vector position, double rotation, Collider collider)
    {
        return new Rigidbody(position, rotation, Vector.Zero, 0d, 1d, 1d, Vector.Zero, 0.4d, collider, true);
    }
    
    

    #endregion
}