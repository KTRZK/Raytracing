using System;

namespace RayTracer.Core;

public class Sphere : IHittable
{
    private readonly Vec3 _center;
    private readonly double _radius;
    private readonly IMaterial _material;

    public Sphere(Vec3 center, double radius, IMaterial material)
    {
        _center = center;
        _radius = Math.Max(0, radius);
        _material = material;
    }

    public bool Hit(Ray r, Interval rayT, ref HitRecord rec)
    {
        var oc = _center - r.Origin;
        var a = r.Direction.LengthSquared();
        var h = Vec3.Dot(r.Direction, oc);
        var c = oc.LengthSquared() - _radius * _radius;

        var discriminant = h * h - a * c;
        if (discriminant < 0) return false;

        var sqrtd = Math.Sqrt(discriminant);

        var root = (h - sqrtd) / a;
        if (!rayT.Surrounds(root))
        {
            root = (h + sqrtd) / a;
            if (!rayT.Surrounds(root))
                return false;
        }

        rec.T = root;
        rec.Point = r.At(rec.T);
        var outwardNormal = (rec.Point - _center) / _radius;
        rec.SetFaceNormal(r, outwardNormal);
        rec.Material = _material;

        return true;
    }
}
