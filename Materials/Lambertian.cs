using System;
using RayTracer.Core;

namespace RayTracer.Materials;

public class Lambertian : IMaterial
{
    private readonly Vec3 _albedo;

    public Lambertian(Vec3 albedo)
    {
        _albedo = albedo;
    }

    public bool Scatter(Ray rIn, HitRecord rec, out Vec3 attenuation, out Ray scattered, Random rng)
    {
        var scatterDirection = rec.Normal + Vec3.RandomUnitVector(rng);

        if (scatterDirection.NearZero())
            scatterDirection = rec.Normal;

        scattered = new Ray(rec.Point, scatterDirection);
        attenuation = _albedo;
        return true;
    }
}
