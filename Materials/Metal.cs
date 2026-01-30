using System;
using RayTracer.Core;

namespace RayTracer.Materials;

public class Metal : IMaterial
{
    private readonly Vec3 _albedo;
    private readonly double _fuzz;

    public Metal(Vec3 albedo, double fuzz)
    {
        _albedo = albedo;
        _fuzz = fuzz < 1 ? fuzz : 1;
    }

    public bool Scatter(Ray rIn, HitRecord rec, out Vec3 attenuation, out Ray scattered, Random rng)
    {
        var reflected = Vec3.Reflect(rIn.Direction, rec.Normal);
        reflected = Vec3.UnitVector(reflected) + _fuzz * Vec3.RandomUnitVector(rng);
        scattered = new Ray(rec.Point, reflected);
        attenuation = _albedo;
        return Vec3.Dot(scattered.Direction, rec.Normal) > 0;
    }
}
