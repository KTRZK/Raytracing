namespace RayTracer.Core;

public readonly struct Interval
{
    public readonly double Min;
    public readonly double Max;

    public Interval(double min, double max)
    {
        Min = min;
        Max = max;
    }

    public bool Contains(double x) => Min <= x && x <= Max;

    public bool Surrounds(double x) => Min < x && x < Max;

    public static readonly Interval Empty = new(double.PositiveInfinity, double.NegativeInfinity);
    public static readonly Interval Universe = new(double.NegativeInfinity, double.PositiveInfinity);
}
