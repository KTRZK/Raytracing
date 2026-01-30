namespace RayTracer.Core;

public interface IHittable
{
    bool Hit(Ray r, Interval rayT, ref HitRecord rec);
}
