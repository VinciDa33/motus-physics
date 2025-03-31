using MotusPhysics.Core.Utility;

namespace MotusPhysics.Core.Testing;

public class VectorTests
{
    
    [Test]
    public void TestEquality()
    {
        Vector v1 = new Vector(10, 5);
        Vector v2 = new Vector(10, 5);
        
        Assert.That(v1 == v2);

        v1 = new Vector(5, 5);
        Assert.That(v1 != v2);
    }
    
    [Test]
    public void TestAddition()
    {
        Vector v1 = new Vector(10, 5);
        Vector v2 = new Vector(5, 2);
        Vector addition = v1 + v2;
        
        Assert.That(addition.x == 15 && addition.y == 7);

        v1 = new Vector(-10, 5);
        v2 = new Vector(5, -5);
        addition = v1 + v2;
        
        Assert.That(addition.x == -5 && addition.y == 0);
    }
    
    [Test]
    public void TestMultiplication()
    {
        Vector v1 = new Vector(10, 5);
        Vector mult = v1 * 5;
        
        Assert.That(mult.x == 50 && mult.y == 25);

        v1 = new Vector(-2, 6);
        mult = v1 * -2;
        
        Assert.That(mult.x == 4 && mult.y == -12);
    }
}