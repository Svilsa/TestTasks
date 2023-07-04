namespace MindboxTest.Tests;

public class CircleTest
{
    [Theory]
    [InlineData(1, 3.141592653589793)]
    [InlineData(5, 78.53981633974483)]
    [InlineData(23.456, 1728.453811460717)]
    [InlineData(7.5645455722826181E+153, double.MaxValue)]
    public void AreaCalculatingTest(double radius, double expectedArea)
    {
        var circle = new Circle(radius);
        Assert.Equal(expectedArea, circle.GetArea());
    }
    
    [Fact]
    public void AreaOverflowTest()
    {
        Assert.Throws<ArgumentException>(() => new Circle(8.5645455722826181E+153));
        Assert.Throws<ArgumentException>(() => new Circle(double.MaxValue));
    }
}