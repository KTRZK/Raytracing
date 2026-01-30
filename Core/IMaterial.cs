using System;

namespace RayTracer.Core;

public interface IMaterial
{
    bool Scatter(Ray rIn, HitRecord rec, out Vec3 attenuation, out Ray scattered, Random rng);
}
