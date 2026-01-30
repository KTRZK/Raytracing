namespace RayTracer.Core;

public struct HitRecord
{
    public Vec3 Point;
    public Vec3 Normal;
    public IMaterial? Material;
    public double T;
    public bool FrontFace;

    public void SetFaceNormal(Ray r, Vec3 outwardNormal)
    {
        FrontFace = Vec3.Dot(r.Direction, outwardNormal) < 0;
        Normal = FrontFace ? outwardNormal : -outwardNormal;
    }
}
