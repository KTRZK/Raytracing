namespace RayTracer.Core;

public readonly struct Ray
{
    public readonly Vec3 Origin;
    public readonly Vec3 Direction;

    public Ray(Vec3 origin, Vec3 direction)
    {
        Origin = origin;
        Direction = direction;
    }

    public Vec3 At(double t) => Origin + t * Direction;
}
