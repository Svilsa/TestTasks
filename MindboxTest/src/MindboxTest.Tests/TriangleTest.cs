namespace MindboxTest.Tests;

public class TriangleTest
{
    [Theory]
    [InlineData(3, 4, 5, 6)]
    [InlineData(5.67, 8.87, 10.56, 25.145862100950136)]
    [InlineData(156778.6743, 176778.2547, 193534.8545, 13074022520.755596)]
    [InlineData(1.75965923037333E+77, 1.75965923037333E+77, 1.75965923037333E+77, 1.3407807929942584E+154)]
    public void AreaCalculatingTest(double a, double b, double c, double expectedArea)
    {
        var circle = new Triangle(a, b, c);
        Assert.Equal(expectedArea, circle.GetArea());
    }
    
    [Theory]
    [InlineData(3, 4, -5)]
    [InlineData(5.67, -8.87, 10.56)]
    [InlineData(-156778.6743, 176778.2547, 193534.8545)]
    [InlineData(1.75965923037333E+77, -1.75965923037333E+77, 1.75965923037333E+77)]
    public void NegativeSideTest(double a, double b, double c)
    {
        Assert.Throws<ArgumentException>(() => new Triangle(a, b, c));
    }
    
    [Theory]
    [InlineData(1.75965923037333E+77, 1.75965923037333E+78, 1.75965923037333E+78)]
    [InlineData(double.MaxValue, double.MaxValue, double.MaxValue)]
    public void OverflowAreaTest(double a, double b, double c)
    {
        Assert.Throws<ArgumentException>(() => new Triangle(a, b, c));
    }
    
    [Theory]
    [InlineData(3, 4, 5)]
    [InlineData(5, 12, 13)]
    [InlineData(28, 45, 53)]
    [InlineData(120, 209, 241)]
    [InlineData(160, 231, 281)]
    [InlineData(68, 285, 293)]
    public void RightTriangleTest(double a, double b, double c)
    {
        Assert.True(new Triangle(a, b, c).IsRightTriangle);
    }
}