using PhysiXSharp.Core.Physics.Bodies;
using PhysiXSharp.Core.Physics.Colliders;
using PhysiXSharp.Core.Utility;

namespace TestRunner;

public class StressSetup
{
    public static void Setup()
    {
        int rigidbodyCount = 100;
        
        Rigidbody floor = Rigidbody.CreateStaticbody(Collider.CreateRectangleCollider(new Vector(28, 1)), position: new Vector(16, 22), restitution: 1d);
        Rigidbody wallLeft = Rigidbody.CreateStaticbody(Collider.CreateRectangleCollider(new Vector(1, 20)), position: new Vector(2, 12), restitution: 1d);
        Rigidbody wallRight = Rigidbody.CreateStaticbody(Collider.CreateRectangleCollider(new Vector(1, 20)), position: new Vector(30, 12), restitution: 1d);
        Rigidbody roof = Rigidbody.CreateStaticbody(Collider.CreateRectangleCollider(new Vector(28, 1)), position: new Vector(16, 2), restitution: 1d);

        Random rand = new Random();
        
        for (int i = 0; i < rigidbodyCount; i++)
        {
            int r = rand.Next(0, 2);
            Vector pos = new Vector(rand.Next(4, 29), rand.Next(4, 21));
            Vector initialVel = new Vector(rand.Next(-2, 3), rand.Next(-2, 3));
            if (r == 0)
            {
                double radius = 0.4d + rand.NextDouble() * 0.6d;
                Rigidbody.CreateRigidbody(Collider.CreateCircleCollider(radius), position: pos, initialVelocity: initialVel, restitution: 1d);
            }
            else if (r == 1)
            {
                Vector size = new Vector(0.5d + rand.NextDouble(), 0.5d + rand.NextDouble());
                Rigidbody.CreateRigidbody(Collider.CreateRectangleCollider(size), position: pos, initialVelocity: initialVel, restitution: 1d);
            }
        }
    }
}