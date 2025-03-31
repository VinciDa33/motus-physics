using MotusPhysics.Core.Utility;

namespace MotusPhysics.Core.Testing;

public class VectorTests
{
    
    [Test]
    public void Test_Equality()
    {
        var v1 = new Vector(10, 5);
        var v2 = new Vector(10, 5);
        
        Assert.That(v1.Equals(v2));

        v1 = new Vector(5, 5);
        Assert.That(!v1.Equals(v2));
    }
    
    [Test]
    public void Test_Addition()
    {
        var v1 = new Vector(10, 5);
        var v2 = new Vector(5, 2);
        var addition = v1 + v2;
        
        Assert.Multiple(() =>
        {
            Assert.That(addition.x, Is.EqualTo(15).Within(0.0001));
            Assert.That(addition.y, Is.EqualTo(7).Within(0.0001));
        });

        v1 = new Vector(-10, 5);
        v2 = new Vector(5, -5);
        addition = v1 + v2;
        
        Assert.Multiple(() =>
        {
            Assert.That(addition.x, Is.EqualTo(-5).Within(0.0001));
            Assert.That(addition.y, Is.EqualTo(0).Within(0.0001));
        });
    }
    
    [Test]
    public void Test_Subtraction()
    {
        var v1 = new Vector(10, 5);
        var v2 = new Vector(5, 2);
        var addition = v1 - v2;
        
        Assert.Multiple(() =>
        {
            Assert.That(addition.x, Is.EqualTo(5).Within(0.0001));
            Assert.That(addition.y, Is.EqualTo(3).Within(0.0001));
        });

        v1 = new Vector(-10, 5);
        v2 = new Vector(5, -5);
        addition = v1 - v2;
        
        Assert.Multiple(() =>
        {
            Assert.That(addition.x, Is.EqualTo(-15).Within(0.0001));
            Assert.That(addition.y, Is.EqualTo(10).Within(0.0001));
        });
    }
    
    [Test]
    public void Test_Multiplication()
    {
        var v1 = new Vector(10, 5);
        var mult = v1 * 5;
        
        Assert.Multiple(() =>
        {
            Assert.That(mult.x, Is.EqualTo(50).Within(0.0001));
            Assert.That(mult.y, Is.EqualTo(25).Within(0.0001));
        });

        v1 = new Vector(-2, 6);
        mult = v1 * -2;
        
        Assert.Multiple(() =>
        {
            Assert.That(mult.x, Is.EqualTo(4).Within(0.0001));
            Assert.That(mult.y, Is.EqualTo(-12).Within(0.0001));
        });
    }
    
    [Test]
    public void Test_Division()
    {
        var v1 = new Vector(10, 5);
        var mult = v1 / 5;
        
        Assert.Multiple(() =>
        {
            Assert.That(mult.x, Is.EqualTo(2).Within(0.0001));
            Assert.That(mult.y, Is.EqualTo(1).Within(0.0001));
        });

        v1 = new Vector(-2, 6);
        mult = v1 / -2;
        
        Assert.Multiple(() =>
        {
            Assert.That(mult.x, Is.EqualTo(1).Within(0.0001));
            Assert.That(mult.y, Is.EqualTo(-3).Within(0.0001));
        });
    }
    
    [Test]
    public void Test_Magnitude()
    {
        var vector = new Vector(3, 4);
        var magnitude = vector.Magnitude();

        Assert.That(magnitude, Is.EqualTo(5).Within(0.0001)); //sqrt(3^2 + 4^2) = 5

        vector = new Vector(-5, 5);
        magnitude = vector.Magnitude();
        
        Assert.That(magnitude, Is.EqualTo(7.071).Within(0.0001));
    }
    
    [Test]
    public void Test_Normalize()
    {
        var vector = new Vector(3, 4);
        vector.Normalize();

        Assert.That(vector.Magnitude(), Is.EqualTo(1).Within(0.0001));
        Assert.That(vector.x, Is.EqualTo(3.0 / 5.0).Within(0.0001));
        Assert.That(vector.y, Is.EqualTo(4.0 / 5.0).Within(0.0001));
    }
    
    [Test]
    public void Test_Normal()
    {
        var vector = new Vector(3, 4);
        var normal = vector.Normal();

        Assert.That(normal.x, Is.EqualTo(-4.0 / 5.0).Within(0.0001));
        Assert.That(normal.y, Is.EqualTo(3.0 / 5.0).Within(0.0001));
    }
    
    [Test]
    public void Test_Rotate()
    {
        var vector = new Vector(1, 0);
        vector.Rotate(90);

        Assert.That(vector.x, Is.EqualTo(0).Within(0.0001));
        Assert.That(vector.y, Is.EqualTo(1).Within(0.0001));
    }
    
    [Test]
    public void Test_Distance()
    {
        var vector1 = new Vector(1, 1);
        var vector2 = new Vector(4, 5);

        var distance = Vector.Distance(vector1, vector2);

        Assert.That(distance, Is.EqualTo(5).Within(0.0001));  //sqrt(3^2 + 4^2) = 5
    }
    
    [Test]
    public void Test_DotProduct()
    {
        var vector1 = new Vector(1, 2);
        var vector2 = new Vector(3, 4);

        var dotProduct = Vector.Dot(vector1, vector2);

        Assert.That(dotProduct, Is.EqualTo(11));  // (1*3 + 2*4) = 11
    }
    
    [Test]
    public void Test_CrossProduct()
    {
        var vector1 = new Vector(1, 2);
        var vector2 = new Vector(3, 4);

        var crossProduct = Vector.Cross(vector1, vector2);

        Assert.That(crossProduct, Is.EqualTo(-2));  // (1*4 - 2*3) = -2
    }

}