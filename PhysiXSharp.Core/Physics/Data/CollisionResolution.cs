using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Data;

public sealed class CollisionResolution
{
    public readonly Rigidbody RigidbodyA;
    public readonly Rigidbody RigidbodyB;
    public readonly Vector VelocityChangeA;
    public readonly Vector VelocityChangeB;
    public readonly float AngularVelocityChangeA;
    public readonly float AngularVelocityChangeB;

    public CollisionResolution(Rigidbody rigidbodyA, Rigidbody rigidbodyB, Vector velocityA, Vector velocityB, float angularA, float angularB)
    {
        RigidbodyA = rigidbodyA;
        RigidbodyB = rigidbodyB;
        VelocityChangeA = velocityA;
        VelocityChangeB = velocityB;
        AngularVelocityChangeA = angularA;
        AngularVelocityChangeB = angularB;
    }

}