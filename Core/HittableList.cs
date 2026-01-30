using System.Collections.Generic;

namespace RayTracer.Core;

public class HittableList : IHittable
{
    private readonly List<IHittable> _objects = new();

    public void Add(IHittable obj) => _objects.Add(obj);

    public void Clear() => _objects.Clear();

    public bool Hit(Ray r, Interval rayT, ref HitRecord rec)
    {
        HitRecord tempRec = default;
        bool hitAnything = false;
        var closestSoFar = rayT.Max;

        foreach (var obj in _objects)
        {
            if (obj.Hit(r, new Interval(rayT.Min, closestSoFar), ref tempRec))
            {
                hitAnything = true;
                closestSoFar = tempRec.T;
                rec = tempRec;
            }
        }

        return hitAnything;
    }
}
