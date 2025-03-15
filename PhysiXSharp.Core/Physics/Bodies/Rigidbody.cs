using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Bodies;

public sealed class Rigidbody : PhysicsObject
{
    private Vector Velocity { get; set; } = new Vector(0d, 0d);
    private float AngularVelocity { get; set; } = 0f;
    private double _mass = 1d;
    private Vector _gravity = new Vector(0d, 0d);
    public Rigidbody(Vector position)
    {
        IsStatic = false;
        Position = position;
    }
    
    public Rigidbody(Vector position, float rotation)
    {
        IsStatic = false;
        Position = position;
        Rotation = rotation;
    }
    
    public Rigidbody(Vector position, double mass)
    {
        IsStatic = false;
        Position = position;
        _mass = mass;
    }
    
    public Rigidbody(Vector position, float rotation, double mass)
    {
        IsStatic = false;
        Position = position;
        Rotation = rotation;
        _mass = mass;
    }

    internal override void Update()
    {
        base.Update();
        TranslatePosition(Velocity * PhysiX.Time.FixedTimeStep * PhysiX.Time.TimeScale);
        Rotate((float) (AngularVelocity * PhysiX.Time.FixedTimeStep * PhysiX.Time.TimeScale));
        Velocity += _gravity * PhysiX.Time.FixedTimeStep * PhysiX.Time.TimeScale;
        
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