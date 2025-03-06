using System.Diagnostics;

namespace PhysiXSharp.Core.Testing;

public class DeltaTimeTests
{
    /*
    [Test]
    public void TestDeltaOver10Seconds()
    {
        Stopwatch stopwatch = new Stopwatch();
        double deltaSum = 0d;
        
        PhysiX.Initialize();
        stopwatch.Start();
        
        while (stopwatch.Elapsed.TotalSeconds < 10d)
        {
            PhysiX.Update();
            deltaSum += PhysiX.DeltaTime;
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
        
        PhysiX.Initialize();
        stopwatch.Start();
        
        while (stopwatch.Elapsed.TotalSeconds < 10d)
        {
            PhysiX.Update();
            totalSteps++;
        }

        double meanStepsPerSecond = totalSteps / 10d;
        Console.WriteLine("Mean steps per seconds: " + meanStepsPerSecond);
        Assert.That(meanStepsPerSecond > 100);
    }
    */
}