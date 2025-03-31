using System.Diagnostics;

namespace MotusPhysics.Core.Testing;

public class DeltaTimeTests
{
    /*
    [Test]
    public void TestDeltaOver10Seconds()
    {
        Stopwatch stopwatch = new Stopwatch();
        double deltaSum = 0d;
        
        Motus.Initialize();
        stopwatch.Start();
        
        while (stopwatch.Elapsed.TotalSeconds < 10d)
        {
            Motus.Update();
            deltaSum += Motus.DeltaTime;
        }
        stopwatch.Stop();
        
        Console.WriteLine("Stopwatch:" + stopwatch.Elapsed.TotalSeconds + " | Sum of deltas: " + deltaSum);

        Assert.That(Math.Abs(deltaSum - stopwatch.Elapsed.TotalSeconds) < 0.00005d);
    }

    [Test]
    public void TestStepsPerSecondOver10Seconds()
    {
        Stopwatch stopwatch = new Stopwatch();
        
        int totalSteps = 0;
        
        Motus.Initialize();
        stopwatch.Start();
        
        while (stopwatch.Elapsed.TotalSeconds < 10d)
        {
            Motus.Update();
            totalSteps++;
        }

        double meanStepsPerSecond = totalSteps / 10d;
        Console.WriteLine("Mean steps per seconds: " + meanStepsPerSecond);
        Assert.That(meanStepsPerSecond > 100);
    }
    */
}