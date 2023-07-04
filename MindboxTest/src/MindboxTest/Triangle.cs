namespace MindboxTest;

public class Triangle : Shape
{
    public Triangle(double a, double b, double c)
    {
        if (a <= 0 || b <= 0 || c <= 0)
            throw new ArgumentException("Sides must be positive");
        if (a >= b + c || b >= a + c || c >= a + b)
            throw new ArgumentException("This triangle doesn't exist");
        if (a + b + c > 5.2789776911199922E+77)
            throw new ArgumentException("The perimeter must be less then 5.2789776911199922E+77");

        ASide = a;
        BSide = b;
        CSide = c;
    }

    public double ASide { get; }
    public double BSide { get; }
    public double CSide { get; }

    public bool IsRightTriangle => ASide * ASide + BSide * BSide == CSide * CSide ||
                                   ASide * ASide + CSide * CSide == BSide * BSide ||
                                   CSide * CSide + BSide * BSide == ASide * ASide;


    //Heron's formula
    public override double GetArea()
    {
        var s = (ASide + BSide + CSide) / 2; // semiperimeter
        return Math.Sqrt(s * (s - ASide) * (s - BSide) * (s - CSide));
    }
}