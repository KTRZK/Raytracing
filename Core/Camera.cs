using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace RayTracer.Core;

public class Camera
{
    public double AspectRatio { get; set; } = 16.0 / 9.0;
    public int ImageWidth { get; set; } = 400;
    public int SamplesPerPixel { get; set; } = 100;
    public int MaxDepth { get; set; } = 50;
    public double Vfov { get; set; } = 90;
    public Vec3 LookFrom { get; set; } = new(0, 0, 0);
    public Vec3 LookAt { get; set; } = new(0, 0, -1);
    public Vec3 Vup { get; set; } = new(0, 1, 0);
    public double DefocusAngle { get; set; } = 0;
    public double FocusDist { get; set; } = 10;

    private int _imageHeight;
    private double _pixelSamplesScale;
    private Vec3 _center;
    private Vec3 _pixel00Loc;
    private Vec3 _pixelDeltaU;
    private Vec3 _pixelDeltaV;
    private Vec3 _u, _v, _w;
    private Vec3 _defocusDiskU;
    private Vec3 _defocusDiskV;

    public void Render(IHittable world, string outputPath)
    {
        var startTime = DateTime.Now;
        
        Initialize();

        var pixels = new Vec3[_imageHeight * ImageWidth];

        var rng = new Random(42);
        for (int j = 0; j < _imageHeight; j++)
        {
            for (int i = 0; i < ImageWidth; i++)
            {
                Vec3 pixelColor = new(0, 0, 0);
                for (int sample = 0; sample < SamplesPerPixel; sample++)
                {
                    var r = GetRay(i, j, rng);
                    pixelColor += RayColor(r, MaxDepth, world, rng);
                }
                pixels[j * ImageWidth + i] = _pixelSamplesScale * pixelColor;
            }

            if (j % 10 == 0)
                Console.WriteLine("Scanlines remaining: " + (_imageHeight - j));
        }

        using var image = new Image<Rgb24>(ImageWidth, _imageHeight);
        for (int j = 0; j < _imageHeight; j++)
        {
            for (int i = 0; i < ImageWidth; i++)
            {
                var color = pixels[j * ImageWidth + i];
                var (r, g, b) = ColorUtils.ToRgb24(color);
                image[i, j] = new Rgb24(r, g, b);
            }
        }
        image.SaveAsPng(outputPath);

        var elapsed = DateTime.Now - startTime;
        Console.WriteLine("Done in " + elapsed.TotalSeconds.ToString("F2") + " seconds");
    }

    private void Initialize()
    {
        _imageHeight = (int)(ImageWidth / AspectRatio);
        _imageHeight = _imageHeight < 1 ? 1 : _imageHeight;

        _pixelSamplesScale = 1.0 / SamplesPerPixel;

        _center = LookFrom;

        var theta = Vfov * Math.PI / 180.0;
        var halfTheta = theta / 2;
        var h = Math.Sin(halfTheta) / Math.Cos(halfTheta);
        var viewportHeight = 2 * h * FocusDist;
        var viewportWidth = viewportHeight * ((double)ImageWidth / _imageHeight);

        _w = Vec3.UnitVector(LookFrom - LookAt);
        _u = Vec3.UnitVector(Vec3.Cross(Vup, _w));
        _v = Vec3.Cross(_w, _u);

        var viewportU = viewportWidth * _u;
        var viewportV = viewportHeight * -_v;

        _pixelDeltaU = viewportU / ImageWidth;
        _pixelDeltaV = viewportV / _imageHeight;

        var viewportUpperLeft = _center - FocusDist * _w - viewportU / 2 - viewportV / 2;
        _pixel00Loc = viewportUpperLeft + 0.5 * (_pixelDeltaU + _pixelDeltaV);

        var defocusAngleRad = (DefocusAngle / 2) * Math.PI / 180.0;
        var defocusRadius = FocusDist * Math.Sin(defocusAngleRad) / Math.Cos(defocusAngleRad);
        _defocusDiskU = _u * defocusRadius;
        _defocusDiskV = _v * defocusRadius;
    }

    private Ray GetRay(int i, int j, Random rng)
    {
        var offset = SampleSquare(rng);
        var pixelSample = _pixel00Loc + ((i + offset.X) * _pixelDeltaU) + ((j + offset.Y) * _pixelDeltaV);

        var rayOrigin = DefocusAngle <= 0 ? _center : DefocusDiskSample(rng);
        var rayDirection = pixelSample - rayOrigin;

        return new Ray(rayOrigin, rayDirection);
    }

    private static Vec3 SampleSquare(Random rng) => new(rng.NextDouble() - 0.5, rng.NextDouble() - 0.5, 0);

    private Vec3 DefocusDiskSample(Random rng)
    {
        var p = Vec3.RandomInUnitDisk(rng);
        return _center + p.X * _defocusDiskU + p.Y * _defocusDiskV;
    }

    private Vec3 RayColor(Ray r, int depth, IHittable world, Random rng)
    {
        if (depth <= 0)
            return new Vec3(0, 0, 0);

        HitRecord rec = default;

        if (world.Hit(r, new Interval(0.001, double.PositiveInfinity), ref rec))
        {
            if (rec.Material!.Scatter(r, rec, out var attenuation, out var scattered, rng))
                return attenuation * RayColor(scattered, depth - 1, world, rng);
            return new Vec3(0, 0, 0);
        }

        var unitDirection = Vec3.UnitVector(r.Direction);
        var a = 0.5 * (unitDirection.Y + 1.0);
        return (1.0 - a) * new Vec3(1.0, 1.0, 1.0) + a * new Vec3(0.5, 0.7, 1.0);
    }
}
