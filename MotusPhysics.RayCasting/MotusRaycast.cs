using MotusPhysics.Core.Physics;
using MotusPhysics.Core.Physics.Colliders;
using MotusPhysics.Core.Utility;

namespace MotusPhysics.RayCasting;

public static class MotusRaycast
{
    
    /// <summary>
    /// Casts a ray, checking for interceptions along its path.
    /// Returns only the first object hit.
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="hit">Output of the ray cast hit</param>
    /// <returns></returns>
    public static bool Cast(Ray ray, out RayCastHit hit)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Casts a ray, checking for interceptions along its path.
    /// Returns true if at least one intersection was found.
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="hits">Output array of all the ray cast hits</param>
    /// <returns></returns>
    public static bool CastAll(Ray ray, out RayCastHit[] hits)
    {
        hits = [];
        List<RayCastHit> hitsList = new List<RayCastHit>();
        
        RigidBody[] bodies = PhysicsManager.Instance.GetRigidbodies().ToArray();
        
        foreach (RigidBody body in bodies)
        {
            if (!body.Collider.IsRayCastTarget)
                continue;
            
            if (body.Collider is PolygonCollider pc)
            {
                CastOntoPolygon(ray, pc, out RayCastHit[] castHits);
                hitsList.AddRange(castHits);
            }
            else if (body.Collider is CircleCollider cc)
            {
                CastOntoCircel(ray, cc, out RayCastHit[] castHits);
                hitsList.AddRange(castHits);
            }
        }

        hits = hitsList.ToArray();
        return hits.Length > 0;
    }

    private static void CastOntoPolygon(Ray ray, PolygonCollider pc, out RayCastHit[] hits)
    {
        hits = [];
        List<RayCastHit> hitsList = new List<RayCastHit>();
        Vector rayOriginDirection = ray.Origin + ray.Direction;
        
        for (int i = 0; i < pc.Vertices.Length; i++)
        {
            Vector edgePointA = pc.Position + pc.Vertices[i];
            Vector edgePointB = pc.Position + pc.Vertices[(i + 1) % pc.Vertices.Length];
                    
            // denominator = (A.x - B.x) * (C.y - D.y) - (A.y - B.y) * (C.x - D.x)
            double denominator = (edgePointA.x - edgePointB.x) * (ray.Origin.y - rayOriginDirection.y) -
                                 (edgePointA.y - edgePointB.y) * (ray.Origin.x - rayOriginDirection.x);
            
            if (denominator == 0) //Parallel lines
                continue;
                    
            // t = (A.x - C.x) * (C.y - D.y) - (A.y - C.y) * (C.x - D.x) / denominator
            double t = ((edgePointA.x - ray.Origin.x) * (ray.Origin.y - rayOriginDirection.y) -
                        (edgePointA.y - ray.Origin.y) * (ray.Origin.x - rayOriginDirection.x)) / denominator;
            // u = -(A.x - B.x) * (A.y - C.y) - (A.y - B.y) * (A.x - C.x) / denominator
            double u = -(edgePointA.x - edgePointB.x) * (edgePointA.y - ray.Origin.y) -
                       (edgePointA.y - edgePointB.y) * (edgePointA.x - ray.Origin.x) / denominator;

            if (t is > 0 and < 1 && u > 0)
            {
                // (P.x, P.y) = (C.x + u * (D.x - C.x), C.y + u * (D.y - C.y))
                Vector point = new Vector(ray.Origin.x + u * (rayOriginDirection.x - ray.Origin.x),
                    ray.Origin.y + u * (rayOriginDirection.y - ray.Origin.y));

                Vector edge = edgePointB - edgePointA;
                
                hitsList.Add(new RayCastHit(pc.Rigidbody, point, edge.Normal()));
            }
        }

        hits = hitsList.ToArray();
    }

    private static void CastOntoCircel(Ray ray, CircleCollider cc, out RayCastHit[] hits)
    {
        hits = [];
        
        Vector u = cc.Position - ray.Origin;
        Vector u1 = Vector.Dot(u, ray.Direction) * ray.Direction;
        Vector u2 = u - u1;
        double d = u2.Magnitude();
        
        if (d > cc.Radius)
            return;

        double m = Math.Sqrt(cc.Radius * cc.Radius - d * d);

        Vector point1 = ray.Origin + u1 + m * ray.Direction;
        Vector normal1 = (point1 - cc.Position).Normalized();

        
        if (Math.Abs(d - cc.Radius) < 0.0001d)
        {
            hits = new RayCastHit[1];
            hits[0] = new RayCastHit(cc.Rigidbody, point1, normal1);
            return;
        }

        hits = new RayCastHit[2];
        
        Vector point2 = ray.Origin + u1 - m * ray.Direction;
        Vector normal2 = (point2 - cc.Position).Normalized();
        
        hits[0] = new RayCastHit(cc.Rigidbody, point1, normal1);
        hits[1] = new RayCastHit(cc.Rigidbody, point2, normal2);
    }
}
