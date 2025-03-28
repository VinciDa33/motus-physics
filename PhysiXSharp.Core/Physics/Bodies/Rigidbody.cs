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
    
    /// <summary>
    /// Velocity in meters per second
    /// </summary>
    public Vector Velocity { get; private set; }
    
    /// <summary>
    /// Current rotation in degrees
    /// </summary>
    public double Rotation { get; private set; }
    
    /// <summary>
    /// Rotational velocity in radians per second
    /// </summary>
    public double AngularVelocity { get; private set; }
    
    /// <summary>
    /// Rotational velocity in degrees per second
    /// </summary>
    public double AngularVelocityDegrees { get; private set; } 
    
    /// <summary>
    /// Arbitrary value for the mass of the rigidbody.
    /// Represents the resistance to change in velocity upon collision.
    /// </summary>
    public double Mass { get; private set; } = 1d;
    public double InverseMass { get; private set; } = 1d;
    
    /// <summary>
    /// Arbitrary value for the moment of inertia of the rigidbody.
    /// Represents the resistance to change in angular velocity upon collision.
    /// </summary>
    public double Inertia { get; private set; } = 1d;
    public double InverseInertia { get; private set; } = 1d;
    
    /// <summary>
    /// Gravitational force and direction.
    /// The magnitude of the gravity vector represents the velocity change in meters/second per second.
    /// </summary>
    public Vector Gravity { get; private set; }

    public double DragCoefficient { get; private set; } = 0d;
    public double AngularDragCoefficient { get; private set; } = 0d;

    
    /// <summary>
    /// The bounciness of the object, upon collision.
    /// 0 -> Perfect inelastic (objects will stick)
    /// 1 -> Perfect elastic (full bounce, no energy loss)
    /// </summary>
    public double Restitution { get; private set; }
    public Collider Collider { get; private set; }
    
    public delegate void CollisionDelegate(CollisionManifold manifold);
    private event CollisionDelegate OnCollisionEnter;
    private event CollisionDelegate OnCollisionExit;

    private readonly Dictionary<(int, int), CollisionManifold> _currentCollisions =
        new Dictionary<(int, int), CollisionManifold>();
    
    
    private Rigidbody(Vector position, double rotation, Vector initialVelocity, double initialAngularVelocity, double mass, double inertia, 
        Vector gravity, double dragCoefficient, double angularDragCoefficient, double restitution, Collider collider, bool staticBody)
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
        DragCoefficient = dragCoefficient;
        AngularDragCoefficient = angularDragCoefficient;
        Collider = collider;
        collider.SetRigidbody(this);
    }

    public void Destroy()
    {
        PhysicsManager.Instance.RemoveRigidbody(this);
    }
    
    internal void Update()
    {
        if (!IsActive || IsStatic)
            return;
        
        //Update position based on velocity
        TranslatePosition(Velocity * PhysiX.Time.FixedTimeStep * PhysiX.Time.TimeScale);
        
        //Update rotation based on angular velocity
        double rotaryChange = AngularVelocity * 180d / Math.PI * PhysiX.Time.FixedTimeStep * PhysiX.Time.TimeScale;
        Rotation += rotaryChange;
        
        //Update velocity based on gravity
        Velocity += Gravity * PhysiX.Time.FixedTimeStep * PhysiX.Time.TimeScale;

        //Update velocity based on drag
        Velocity *= (1 - DragCoefficient * PhysiX.Time.FixedTimeStep * PhysiX.Time.TimeScale);
        
        //Update angular velocity based on angular drag
        AngularVelocity *= (1 - AngularDragCoefficient * PhysiX.Time.FixedTimeStep * PhysiX.Time.TimeScale);
        
        //Update the colliders rotation, only if a change in rotation occured (This also updates it AABB and its normals)
        if (rotaryChange != 0)
            Collider.UpdateRotation();
        
        //Handle old collision evetns
        foreach(KeyValuePair<(int, int), CollisionManifold> entry in _currentCollisions)
        {
            //Discard old collisions that have not been refreshed after a few sim steps
            if (entry.Value.SimStep < PhysiX.Time.SimStep - 4)
            {
                OnCollisionExit?.Invoke(entry.Value);
                _currentCollisions.Remove(entry.Key);
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
        if (!_currentCollisions.ContainsKey(key))
            OnCollisionEnter?.Invoke(manifold);

        _currentCollisions[key] = manifold;
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
    }

    /// <summary>
    /// Moves the rigidbody by the specified translation value.
    /// </summary>
    /// <param name="translation"></param>
    public void TranslatePosition(Vector translation)
    {
        Position += translation;
    }
    
    /// <summary>
    /// Sets the absolute rotation of the rigidbody in degrees.
    /// </summary>
    /// <param name="degrees"></param>
    public void SetRotation(double degrees)
    {
        Rotation = degrees;
        Collider.UpdateRotation();
    }

    /// <summary>
    /// Rotates the rigidbody by a specified amount of degrees.
    /// </summary>
    /// <param name="degrees"></param>
    public void Rotate(double degrees)
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
    
    public void SetAngularVelocity(double angularVelocity)
    {
        AngularVelocity = angularVelocity;
        AngularVelocityDegrees = AngularVelocity * (180d / Math.PI);
    }

    public void AddAngularVelocity(double addition)
    {
        AngularVelocity += addition;
        AngularVelocityDegrees = AngularVelocity * (180d / Math.PI);
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
        return new Rigidbody(position, 0d, Vector.Zero, 0d, 1d, 1d, Vector.Zero, 0d, 0d, 0.4d, collider, false);
    }

    public static Rigidbody CreateRigidbody(Vector position, double rotation, Collider collider)
    {
        return new Rigidbody(position, rotation, Vector.Zero, 0d, 1d, 1d, Vector.Zero, 0d, 0d, 0.4d, collider, false);
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
        
        return new Rigidbody(position, rotation, Vector.Zero, 0d, mass, inertia, Vector.Zero, 0d, 0d, restitution, collider, false);
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
        
        if (restitution > 1d || restitution < 0d)
        {
            restitution = restitution > 1d ? 1d : 0d;
            PhysiX.Logger.LogWarning("Rigidbody restitution must be between 0 and 1.\nValue has been clamped.");
        }
        
        return new Rigidbody(position, rotation, initialVelocity, initialAngularVelocity, mass, inertia, Vector.Zero, 0d, 0d, restitution, collider, false);
    }
    
    public static Rigidbody CreateRigidbody(Vector position, double rotation, Collider collider, Vector initialVelocity,
        double initialAngularVelocity, double mass, double inertia, double drag, double angularDrag,
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
        
        if (restitution > 1d || restitution < 0d)
        {
            restitution = restitution > 1d ? 1d : 0d;
            PhysiX.Logger.LogWarning("Rigidbody restitution must be between 0 and 1.\nValue has been clamped.");
        }

        if (drag > 1d || drag < 0d)
        {
            drag = drag > 1d ? 1d : 0d;
            PhysiX.Logger.LogWarning("Rigidbody drag coefficient must be between 0 and 1.\nValue has been clamped.");
        }
        
        if (angularDrag > 1d || angularDrag < 0d)
        {
            angularDrag = angularDrag > 1d ? 1d : 0d;
            PhysiX.Logger.LogWarning("Rigidbody angular drag coefficient must be between 0 and 1.\nValue has been clamped.");
        }
        
        return new Rigidbody(position, rotation, initialVelocity, initialAngularVelocity, mass, inertia, Vector.Zero, drag, angularDrag, restitution, collider, false);
    }
    
    //Optional parameter factory
    public static Rigidbody CreateRigidbody(Collider collider, Vector? position = null, double rotation = 0d, Vector? initialVelocity = null,
        double initialAngularVelocity = 0d, double mass = 1d, double inertia = 1d, double drag = 0d, double angularDrag = 0d,
        double restitution = 0.4d)
    {
        position ??= Vector.Zero;
        initialVelocity ??= Vector.Zero;
        
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
        
        if (restitution > 1d || restitution < 0d)
        {
            restitution = restitution > 1d ? 1d : 0d;
            PhysiX.Logger.LogWarning("Rigidbody restitution must be between 0 and 1.\nValue has been clamped.");
        }

        if (drag > 1d || drag < 0d)
        {
            drag = drag > 1d ? 1d : 0d;
            PhysiX.Logger.LogWarning("Rigidbody drag coefficient must be between 0 and 1.\nValue has been clamped.");
        }
        
        if (angularDrag > 1d || angularDrag < 0d)
        {
            angularDrag = angularDrag > 1d ? 1d : 0d;
            PhysiX.Logger.LogWarning("Rigidbody angular drag coefficient must be between 0 and 1.\nValue has been clamped.");
        }
        
        return new Rigidbody(position, rotation, initialVelocity, initialAngularVelocity, mass, inertia, Vector.Zero, drag, angularDrag, restitution, collider, false);
    }
    
    public static Rigidbody CreateStaticbody(Vector position, Collider collider)
    {
        return new Rigidbody(position, 0d, Vector.Zero, 0d, 1d, 1d, Vector.Zero, 0d, 0d, 0.4d, collider, true);
    }

    public static Rigidbody CreateStaticbody(Vector position, double rotation, Collider collider)
    {
        return new Rigidbody(position, rotation, Vector.Zero, 0d, 1d, 1d, Vector.Zero, 0d, 0d, 0.4d, collider, true);
    }

    public static Rigidbody CreateStaticbody(Collider collider, Vector? position = null, double rotation = 0d, double restitution = 0.4d)
    {
        position ??= Vector.Zero;
        return new Rigidbody(position, rotation, Vector.Zero, 0d, 1d, 1d, Vector.Zero, 0d, 0d, restitution, collider, true);
    }
    #endregion
}