using System.Diagnostics;
using MotusPhysics.Core.Physics;
using MotusPhysics.Core.Physics.Colliders;
using MotusPhysics.Core.Utility;

namespace MotusPhysics.Core.Testing;

public class UpdateRateTest
{

    [Test]
    public void Test_EmptySceneUpdateRate()
    {
        Stopwatch watch = new Stopwatch();
     
        Motus.DisableModuleLoading();
        if (!Motus.IsInitialized)
            Motus.Initialize();
        
        watch.Start();
        while (watch.Elapsed.TotalSeconds < 5)
        {
            if (Motus.Time.SimStep > 1 && Motus.Time.LastStepMilliseconds / 1000d > Motus.Time.FixedTimeStep)
            {
                Assert.Fail();
            }
        }
        
        Assert.Pass();
    }

    [Test]
    public void Test_TripleCollisionUpdateRate()
    {
        Stopwatch watch = new Stopwatch();
        
        Motus.DisableModuleLoading();
        if (!Motus.IsInitialized)
            Motus.Initialize();

        //Setup 3 objects to collide at the same time
        Rigidbody.CreateRigidbody(Collider.CreateCircleCollider(1d), position: new Vector(-3, 0), initialVelocity: new Vector(1, 0)).OnCollisionEnterSubscribe(manifold => {Console.WriteLine("Collision!");});
        Rigidbody.CreateRigidbody(Collider.CreateCircleCollider(1d), position: new Vector(-3, 2), initialVelocity: new Vector(1, 0));
        Rigidbody.CreateRigidbody(Collider.CreateCircleCollider(1d), position: new Vector(-3, 4), initialVelocity: new Vector(1, 0));
        
        Rigidbody.CreateRigidbody(Collider.CreateCircleCollider(1d), position: new Vector(3, 0), initialVelocity: new Vector(-1, 0));
        Rigidbody.CreateRigidbody(Collider.CreateCircleCollider(1d), position: new Vector(3, 2), initialVelocity: new Vector(-1, 0));
        Rigidbody.CreateRigidbody(Collider.CreateCircleCollider(1d), position: new Vector(3, 4), initialVelocity: new Vector(-1, 0));
        
        watch.Start();
        while (watch.Elapsed.TotalSeconds < 5)
        {
            if (Motus.Time.SimStep > 1 && Motus.Time.LastStepMilliseconds / 1000d > Motus.Time.FixedTimeStep)
            {
                Assert.Fail();
            }
        }
        
        Assert.Pass();
    }

    [TearDown]
    public void Cleanup()
    {
        if (Motus.IsInitialized)
            Motus.Shutdown();
    }

}