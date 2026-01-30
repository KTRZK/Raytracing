using System;

namespace RayTracer.Core;

public struct Vec3
{
    public double X, Y, Z;

    public Vec3(double x, double y, double z)
    {
        X = x; Y = y; Z = z;
    }

    public static Vec3 operator +(Vec3 a, Vec3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Vec3 operator -(Vec3 a, Vec3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static Vec3 operator *(Vec3 a, Vec3 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

    public static Vec3 operator *(Vec3 v, double t) => new(v.X * t, v.Y * t, v.Z * t);

    public static Vec3 operator *(double t, Vec3 v) => new(v.X * t, v.Y * t, v.Z * t);

    public static Vec3 operator /(Vec3 v, double t) => new(v.X / t, v.Y / t, v.Z / t);

    public static Vec3 operator -(Vec3 v) => new(-v.X, -v.Y, -v.Z);

    public double LengthSquared() => X * X + Y * Y + Z * Z;

    public double Length() => Math.Sqrt(LengthSquared());

    public bool NearZero()
    {
        const double s = 1e-8;
        return Math.Abs(X) < s && Math.Abs(Y) < s && Math.Abs(Z) < s;
    }

    public static double Dot(Vec3 a, Vec3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

    public static Vec3 Cross(Vec3 a, Vec3 b) => new(
        a.Y * b.Z - a.Z * b.Y,
        a.Z * b.X - a.X * b.Z,
        a.X * b.Y - a.Y * b.X
    );

    public static Vec3 UnitVector(Vec3 v) => v / v.Length();

    public static Vec3 Random(Random rng) => new(rng.NextDouble(), rng.NextDouble(), rng.NextDouble());

    public static Vec3 Random(Random rng, double min, double max)
    {
        var range = max - min;
        return new(
            min + rng.NextDouble() * range,
            min + rng.NextDouble() * range,
            min + rng.NextDouble() * range
        );
    }

    public static Vec3 RandomInUnitSphere(Random rng)
    {
        while (true)
        {
            var p = Random(rng, -1, 1);
            if (p.LengthSquared() < 1) return p;
        }
    }

    public static Vec3 RandomUnitVector(Random rng) => UnitVector(RandomInUnitSphere(rng));

    public static Vec3 RandomInUnitDisk(Random rng)
    {
        while (true)
        {
            var p = new Vec3(rng.NextDouble() * 2 - 1, rng.NextDouble() * 2 - 1, 0);
            if (p.LengthSquared() < 1) return p;
        }
    }

    public static Vec3 Reflect(Vec3 v, Vec3 n) => v - 2 * Dot(v, n) * n;

    public static Vec3 Refract(Vec3 uv, Vec3 n, double etaiOverEtat)
    {
        var cosTheta = Math.Min(Dot(-uv, n), 1.0);
        var rOutPerp = etaiOverEtat * (uv + cosTheta * n);
        var rOutParallel = -Math.Sqrt(Math.Abs(1.0 - rOutPerp.LengthSquared())) * n;
        return rOutPerp + rOutParallel;
    }
}

public static class ColorUtils
{
    public static double LinearToGamma(double linear) => linear > 0 ? Math.Sqrt(linear) : 0;

    public static (byte r, byte g, byte b) ToRgb24(Vec3 color)
    {
        var r = LinearToGamma(color.X);
        var g = LinearToGamma(color.Y);
        var b = LinearToGamma(color.Z);

        byte rb = (byte)(256 * Math.Max(0.0, Math.Min(0.999, r)));
        byte gb = (byte)(256 * Math.Max(0.0, Math.Min(0.999, g)));
        byte bb = (byte)(256 * Math.Max(0.0, Math.Min(0.999, b)));

        return (rb, gb, bb);
    }
}
