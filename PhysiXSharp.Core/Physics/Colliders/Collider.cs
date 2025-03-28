using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Utility;

namespace PhysiXSharp.Core.Physics.Colliders;

public abstract class Collider
{
    public bool IsEnabled { get; private set; } = true;
    public bool IsTrigger { get; private set; } = false;
    public Rigidbody? Rigidbody { get; private set; }
    public Vector Position => Rigidbody == null ? Vector.Zero : Rigidbody.Position;
    public AABB AxisAlignedBoundingBox { get; protected set; }
    public Vector[] Normals { get; protected set; } = [];
    
    public void SetRigidbody(Rigidbody rigidbody)
    {
        Rigidbody = rigidbody;
        CalculateAABB();
        CalculateNormals();
    }

    public void SetEnabled(bool enable)
    {
        IsEnabled = enable;
    }

    public void SetTrigger(bool setTrigger)
    {
        IsTrigger = setTrigger;
    }
    
    internal abstract void CalculateAABB();
    internal abstract void CalculateNormals();
    internal abstract void UpdateRotation();

    /// <summary>
    /// Returns true if the axis aligned bounding boxes of the two colliders are overlapping
    /// </summary>
    /// <param name="colliderA"></param>
    /// <param name="colliderB"></param>
    /// <returns></returns>
    public static bool OverlapAABB(Collider colliderA, Collider colliderB)
    {
        Vector minA = colliderA.Position + colliderA.AxisAlignedBoundingBox.Min;
        Vector maxA = colliderA.Position + colliderA.AxisAlignedBoundingBox.Max;
        
        Vector minB = colliderB.Position + colliderB.AxisAlignedBoundingBox.Min;
        Vector maxB = colliderB.Position + colliderB.AxisAlignedBoundingBox.Max;
        
        
        if (minA.x > maxB.x ||  minB.x > maxA.x)
            return false;

        if (minA.y > maxB.y ||  minB.y > maxA.y)
            return false;        

        return true;
    }


    #region Collider Factories

    public static CircleCollider CreateCircleCollider(double radius)
    {
        if (radius <= 0d)
        {
            PhysiX.Logger.LogWarning("Circle collider radius must be positive!\nRadius has been set to 0.01.");
            radius = 0.01d;
        }
        
        return new CircleCollider(radius);
    }

    public static PolygonCollider CreateRectangleCollider(Vector size)
    {
        if (size.x <= 0d)
        {
            PhysiX.Logger.LogWarning("Rectangle collider width must be positive!\nWidth has been set to 0.01.");
            size.x = 0.01d;
        }
        
        if (size.y <= 0d)
        {
            PhysiX.Logger.LogWarning("Rectangle collider height must be positive!\nHeight has been set to 0.01.");
            size.y = 0.01d;
        }

        return new RectangleCollider(size);
    }

    public static PolygonCollider CreatePolygonCollider(params Vector[] points)
    {
        if (points.Length < 3)
        {
            PhysiX.Logger.LogError("Polygon collider must have 3 or more vertices!\nA default rectangle shape has been created instead!");
            return new PolygonCollider(new Vector(0.5d, 0.5d), new Vector(0.5d, -0.5d), new Vector(-0.5d, -0.5d), new Vector(-0.5d, 0.5d));
        }

        return new PolygonCollider(points);
    }
    #endregion
    
}