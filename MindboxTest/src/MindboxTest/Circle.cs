namespace MindboxTest;

public class Circle : Shape
{
    public Circle(double radius)
    {
        Radius = radius switch
        {
            < 0 => throw new ArgumentOutOfRangeException(nameof(radius), "Radius must be positive"),
            > 7.5645455722826181E+153 => throw new ArgumentOutOfRangeException(nameof(radius),
                "Radius must be less then 7.5645455722826181E+153"),
            _ => radius
        };
    }

    public double Radius { get; }

    public override double GetArea()
    {
        return Math.PI * (Radius * Radius);
    }
}