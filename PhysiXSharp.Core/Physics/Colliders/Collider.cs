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
        Vector origin1 = collider1.AxisAlignedBoundingBox.Origin;
        Vector size1 = collider1.AxisAlignedBoundingBox.Size;
        
        Vector origin2 = collider2.AxisAlignedBoundingBox.Origin;
        Vector size2 = collider2.AxisAlignedBoundingBox.Size;
        
        
        if (origin1.x > origin2.x + size2.x || origin1.x + size1.x < origin2.x)
            return false;

        if (origin1.y > origin2.y + size2.y || origin1.y + size1.y < origin2.y)
            return false;        

        return true;
    }
}