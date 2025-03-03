using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics;

public abstract class Rigidbody : PhysicsObject
{
    private Vector Velocity { get; set; } = new Vector(0d, 0d);
    private float AngularVelocity { get; set; } = 0f;
    private double _mass = 1d;
    private Vector _gravity = new Vector(0d, 0d);
    public Rigidbody()
    {
        IsStatic = false;
        PhysicsManager.Instance.AddRigidbody(this);
    }
    
    public Rigidbody(double mass)
    {
        IsStatic = false;
        PhysicsManager.Instance.AddRigidbody(this);
        _mass = mass;
    }
    
    public Rigidbody(double mass, Vector gravity)
    {
        IsStatic = false;
        PhysicsManager.Instance.AddRigidbody(this);
        _mass = mass;
        _gravity = gravity;
    }
    
    public Rigidbody(Vector velocity, float angularVelocity, double mass, Vector gravity)
    {
        IsStatic = false;
        PhysicsManager.Instance.AddRigidbody(this);
        Velocity = velocity;
        AngularVelocity = angularVelocity;
        _mass = mass;
        _gravity = gravity;
    }

    public void Update()
    {
        TranslatePosition(Velocity * PhysiX.FixedDeltaTime);
        Rotate((float) (AngularVelocity * PhysiX.FixedDeltaTime));
        Velocity += _gravity * PhysiX.FixedDeltaTime;
        
        Collider?.SetRotation(Rotation);
    }

    public void SetPosition(Vector position)
    {
        Position = position;
        Collider?.CalculateAABB();
    }

    public void TranslatePosition(Vector translation)
    {
        Position += translation;
        Collider?.CalculateAABB();
    }
    
    public void SetRotation(float degrees)
    {
        Rotation = degrees;
        Collider?.SetRotation(degrees);
    }

    public void Rotate(float degrees)
    {
        Rotation += degrees;
        Collider?.Rotate(degrees);
    }
    
    public void SetVelocity(Vector velocity)
    {
        Velocity = velocity;
    }
    
    public void SetAngularVelocity(float angularVelocity)
    {
        AngularVelocity = angularVelocity;
    }
}