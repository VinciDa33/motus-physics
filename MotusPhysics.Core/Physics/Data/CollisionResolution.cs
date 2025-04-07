using MotusPhysics.Core.Utility;

namespace MotusPhysics.Core.Physics.Data;

public sealed class CollisionResolution
{
    public readonly RigidBody RigidBodyA;
    public readonly RigidBody RigidBodyB;
    public readonly Vector VelocityChangeA;
    public readonly Vector VelocityChangeB;
    public readonly double AngularVelocityChangeA;
    public readonly double AngularVelocityChangeB;

    public CollisionResolution(RigidBody rigidBodyA, RigidBody rigidBodyB, Vector velocityA, Vector velocityB, double angularA, double angularB)
    {
        RigidBodyA = rigidBodyA;
        RigidBodyB = rigidBodyB;
        VelocityChangeA = velocityA;
        VelocityChangeB = velocityB;
        AngularVelocityChangeA = angularA;
        AngularVelocityChangeB = angularB;
    }

}