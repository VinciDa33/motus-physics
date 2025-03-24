using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Data;

public sealed class CollisionResolution
{
    public readonly Rigidbody RigidbodyA;
    public readonly Rigidbody RigidbodyB;
    public readonly Vector VelocityChangeA;
    public readonly Vector VelocityChangeB;
    public readonly double AngularVelocityChangeA;
    public readonly double AngularVelocityChangeB;

    public CollisionResolution(Rigidbody rigidbodyA, Rigidbody rigidbodyB, Vector velocityA, Vector velocityB, double angularA, double angularB)
    {
        RigidbodyA = rigidbodyA;
        RigidbodyB = rigidbodyB;
        VelocityChangeA = velocityA;
        VelocityChangeB = velocityB;
        AngularVelocityChangeA = angularA;
        AngularVelocityChangeB = angularB;
    }

}