using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Colliders;

public abstract class Collider
{
    public bool IsTrigger { get; private set; } = false;
    public PhysicsObject? PhysicsObject { get; private set; }
    public Vector Position => PhysicsObject == null ? Vector.Zero : PhysicsObject.Position;
    public AABB AxisAlignedBoundingBox { get; protected set; }
    public List<Vector> Normals { get; protected set; }
    
    public void SetPhysicsObject(PhysicsObject physicsObject)
    {
        PhysicsObject = physicsObject;
        CalculateAABB();
        CalculateNormals();
    }
    
    internal abstract void CalculateAABB();
    internal abstract void CalculateNormals();
    
    internal abstract void Rotate(float degrees);

    internal abstract void SetRotation(float degrees);
    
    /// <summary>
    /// Returns true if the axis aligned bounding boxes of the two colliders are overlapping
    /// </summary>
    /// <param name="collider1"></param>
    /// <param name="collider2 "></param>
    /// <returns></returns>
    public static bool OverlapAABB(Collider collider1, Collider collider2)
    {
        Vector minA = collider1.Position + collider1.AxisAlignedBoundingBox.Min;
        Vector maxA = collider1.Position + collider1.AxisAlignedBoundingBox.Max;
        
        Vector minB = collider2.Position + collider2.AxisAlignedBoundingBox.Min;
        Vector maxB = collider2.Position + collider2.AxisAlignedBoundingBox.Max;
        
        
        if (minA.x > maxB.x ||  minB.x > maxA.x)
            return false;

        if (minA.y > maxB.y ||  minB.y > maxA.y)
            return false;        

        return true;
    }
}