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
        PhysicsManager.Instance.AddRigidbody(this);
    }
    
    public Rigidbody(double mass)
    {
        PhysicsManager.Instance.AddRigidbody(this);
        _mass = mass;
    }
    
    public Rigidbody(double mass, Vector gravity)
    {
        PhysicsManager.Instance.AddRigidbody(this);
        _mass = mass;
        _gravity = gravity;
    }
    
    public Rigidbody(Vector velocity, float angularVelocity, double mass, Vector gravity)
    {
        PhysicsManager.Instance.AddRigidbody(this);
        Velocity = velocity;
        AngularVelocity = angularVelocity;
        _mass = mass;
        _gravity = gravity;
    }

    public void Update()
    {
        Position += Velocity * PhysiX.FixedDeltaTime;
        Velocity += _gravity * PhysiX.FixedDeltaTime;
        
        Rotation += (float) (AngularVelocity * PhysiX.FixedDeltaTime);
        Collider?.SetRotation(Rotation);
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