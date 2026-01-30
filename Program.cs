using System;
using RayTracer.Core;
using RayTracer.Materials;

namespace RayTracer;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Ray Tracer - C# Implementation");
            Console.WriteLine("Based on 'Ray Tracing in One Weekend' by Peter Shirley");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  dotnet run -- all                Generate all tutorial stages");
            Console.WriteLine("  dotnet run -- step1              Step 1: Blue-white gradient");
            Console.WriteLine("  dotnet run -- step2              Step 2: First sphere");
            Console.WriteLine("  dotnet run -- step3              Step 3: Surface normals");
            Console.WriteLine("  dotnet run -- step4              Step 4: Ground plane");
            Console.WriteLine("  dotnet run -- step5              Step 5: Materials (diffuse, metal, glass)");
            Console.WriteLine("  dotnet run -- step6              Step 6: Camera positioning");
            Console.WriteLine("  dotnet run -- tutorial           Final: Complete tutorial scene");
            Console.WriteLine("  dotnet run -- advanced           Advanced: Custom complex scene");
            return;
        }

        var scene = args[0].ToLower();

        switch (scene)
        {
            case "all":
                RenderAllSteps();
                break;
            case "step1":
                RenderStep1_Gradient();
                break;
            case "step2":
                RenderStep2_Sphere();
                break;
            case "step3":
                RenderStep3_Normals();
                break;
            case "step4":
                RenderStep4_Ground();
                break;
            case "step5":
                RenderStep5_Materials();
                break;
            case "step6":
                RenderStep6_Camera();
                break;
            case "tutorial":
                RenderTutorialFinal();
                break;
            case "advanced":
                RenderAdvanced();
                break;
            default:
                Console.WriteLine($"Unknown scene: {scene}");
                break;
        }
    }

    static void RenderAllSteps()
    {
        RenderStep1_Gradient();
        RenderStep2_Sphere();
        RenderStep3_Normals();
        RenderStep4_Ground();
        RenderStep5_Materials();
        RenderStep6_Camera();
        RenderTutorialFinal();
    }

    // Step 1: Simple gradient background (Chapter 4)
    static void RenderStep1_Gradient()
    {
        Console.WriteLine("=== Step 1: Gradient Background ===");
        var world = new HittableList(); // Empty world - just gradient

        var cam = new Camera
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 400,
            SamplesPerPixel = 10,
            MaxDepth = 5,
            Vfov = 90,
            LookFrom = new Vec3(0, 0, 0),
            LookAt = new Vec3(0, 0, -1),
            Vup = new Vec3(0, 1, 0),
            DefocusAngle = 0
        };

        cam.Render(world, "step1_gradient.png");
    }

    // Step 2: First red sphere (Chapter 5)
    static void RenderStep2_Sphere()
    {
        Console.WriteLine("=== Step 2: First Sphere ===");
        var world = new HittableList();
        
        // Simple red sphere
        var red = new Lambertian(new Vec3(1.0, 0.0, 0.0));
        world.Add(new Sphere(new Vec3(0, 0, -1), 0.5, red));

        var cam = new Camera
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 400,
            SamplesPerPixel = 10,
            MaxDepth = 5,
            Vfov = 90,
            LookFrom = new Vec3(0, 0, 0),
            LookAt = new Vec3(0, 0, -1),
            Vup = new Vec3(0, 1, 0),
            DefocusAngle = 0
        };

        cam.Render(world, "step2_sphere.png");
    }

    // Step 3: Surface normals as colors (Chapter 6)
    static void RenderStep3_Normals()
    {
        Console.WriteLine("=== Step 3: Surface Normals ===");
        var world = new HittableList();
        
        var normalMat = new Lambertian(new Vec3(0.5, 0.5, 1.0));
        world.Add(new Sphere(new Vec3(0, 0, -1), 0.5, normalMat));

        var cam = new Camera
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 400,
            SamplesPerPixel = 20,
            MaxDepth = 10,
            Vfov = 90,
            LookFrom = new Vec3(0, 0, 0),
            LookAt = new Vec3(0, 0, -1),
            Vup = new Vec3(0, 1, 0),
            DefocusAngle = 0
        };

        cam.Render(world, "step3_normals.png");
    }

    // Step 4: Sphere on ground (Chapter 7-8)
    static void RenderStep4_Ground()
    {
        Console.WriteLine("=== Step 4: Sphere on Ground ===");
        var world = new HittableList();
        
        var groundMat = new Lambertian(new Vec3(0.8, 0.8, 0.0));
        var sphereMat = new Lambertian(new Vec3(0.7, 0.3, 0.3));
        
        world.Add(new Sphere(new Vec3(0, -100.5, -1), 100, groundMat));
        world.Add(new Sphere(new Vec3(0, 0, -1), 0.5, sphereMat));

        var cam = new Camera
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 400,
            SamplesPerPixel = 50,
            MaxDepth = 20,
            Vfov = 90,
            LookFrom = new Vec3(0, 0, 0),
            LookAt = new Vec3(0, 0, -1),
            Vup = new Vec3(0, 1, 0),
            DefocusAngle = 0
        };

        cam.Render(world, "step4_ground.png");
    }

    // Step 5: Different materials (Chapter 9-11)
    static void RenderStep5_Materials()
    {
        Console.WriteLine("=== Step 5: Materials (Diffuse, Metal, Glass) ===");
        var world = new HittableList();

        var groundMat = new Lambertian(new Vec3(0.8, 0.8, 0.0));
        var centerMat = new Lambertian(new Vec3(0.1, 0.2, 0.5));
        var leftMat = new Dielectric(1.50);
        var bubbleMat = new Dielectric(1.00 / 1.50);
        var rightMat = new Metal(new Vec3(0.8, 0.6, 0.2), 0.0);

        world.Add(new Sphere(new Vec3(0.0, -100.5, -1.0), 100.0, groundMat));
        world.Add(new Sphere(new Vec3(0.0, 0.0, -1.2), 0.5, centerMat));
        world.Add(new Sphere(new Vec3(-1.0, 0.0, -1.0), 0.5, leftMat));
        world.Add(new Sphere(new Vec3(-1.0, 0.0, -1.0), 0.4, bubbleMat));
        world.Add(new Sphere(new Vec3(1.0, 0.0, -1.0), 0.5, rightMat));

        var cam = new Camera
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 600,
            SamplesPerPixel = 100,
            MaxDepth = 50,
            Vfov = 90,
            LookFrom = new Vec3(0, 0, 0),
            LookAt = new Vec3(0, 0, -1),
            Vup = new Vec3(0, 1, 0),
            DefocusAngle = 0
        };

        cam.Render(world, "step5_materials.png");
    }

    // Step 6: Positionable camera (Chapter 12)
    static void RenderStep6_Camera()
    {
        Console.WriteLine("=== Step 6: Camera Positioning ===");
        var world = new HittableList();

        var groundMat = new Lambertian(new Vec3(0.8, 0.8, 0.0));
        var centerMat = new Lambertian(new Vec3(0.1, 0.2, 0.5));
        var leftMat = new Dielectric(1.50);
        var bubbleMat = new Dielectric(1.00 / 1.50);
        var rightMat = new Metal(new Vec3(0.8, 0.6, 0.2), 0.1);

        world.Add(new Sphere(new Vec3(0.0, -100.5, -1.0), 100.0, groundMat));
        world.Add(new Sphere(new Vec3(0.0, 0.0, -1.2), 0.5, centerMat));
        world.Add(new Sphere(new Vec3(-1.0, 0.0, -1.0), 0.5, leftMat));
        world.Add(new Sphere(new Vec3(-1.0, 0.0, -1.0), 0.4, bubbleMat));
        world.Add(new Sphere(new Vec3(1.0, 0.0, -1.0), 0.5, rightMat));

        var cam = new Camera
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 800,
            SamplesPerPixel = 100,
            MaxDepth = 50,
            Vfov = 20,
            LookFrom = new Vec3(-2, 2, 1),
            LookAt = new Vec3(0, 0, -1),
            Vup = new Vec3(0, 1, 0),
            DefocusAngle = 0
        };

        cam.Render(world, "step6_camera.png");
    }

    static void RenderTutorialFinal()
    {
        Console.WriteLine("=== Rendering Tutorial Final Scene ===");
        var world = CreateTutorialWorld();

        var cam = new Camera
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 1200,
            SamplesPerPixel = 500,
            MaxDepth = 50,
            Vfov = 20,
            LookFrom = new Vec3(13, 2, 3),
            LookAt = new Vec3(0, 0, 0),
            Vup = new Vec3(0, 1, 0),
            DefocusAngle = 0.6,
            FocusDist = 10.0
        };

        cam.Render(world, "tutorial_final.png");
    }

    static void RenderAdvanced()
    {
        Console.WriteLine("=== Rendering Advanced Scene (More Complex) ===");
        var world = new HittableList();
        var rng = new Random(123);

        // Ground with pattern
        var ground = new Lambertian(new Vec3(0.4, 0.5, 0.4));
        world.Add(new Sphere(new Vec3(0, -1000, 0), 1000, ground));

        // More random spheres (larger grid)
        for (int a = -15; a < 15; a++)
        {
            for (int b = -15; b < 15; b++)
            {
                var choose = rng.NextDouble();
                var center = new Vec3(a + 0.9 * rng.NextDouble(), 0.2, b + 0.9 * rng.NextDouble());

                if ((center - new Vec3(4, 0.2, 0)).Length() > 0.9)
                {
                    IMaterial mat;
                    if (choose < 0.8)
                        mat = new Lambertian(Vec3.Random(rng) * Vec3.Random(rng));
                    else if (choose < 0.95)
                        mat = new Metal(Vec3.Random(rng, 0.5, 1), rng.NextDouble() * 0.5);
                    else
                        mat = new Dielectric(1.5);

                    world.Add(new Sphere(center, 0.2, mat));
                }
            }
        }

        // Large feature spheres
        world.Add(new Sphere(new Vec3(0, 1, 0), 1.0, new Dielectric(1.5)));
        world.Add(new Sphere(new Vec3(-4, 1, 0), 1.0, new Lambertian(new Vec3(0.4, 0.2, 0.1))));
        world.Add(new Sphere(new Vec3(4, 1, 0), 1.0, new Metal(new Vec3(0.7, 0.6, 0.5), 0.0)));

        world.Add(new Sphere(new Vec3(0, 2.5, -6), 1.5, new Metal(new Vec3(0.9, 0.8, 0.7), 0.1)));
        world.Add(new Sphere(new Vec3(-6, 1.2, -3), 1.2, new Dielectric(1.8)));

        var cam = new Camera
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 1400,
            SamplesPerPixel = 600,
            MaxDepth = 60,
            Vfov = 20,
            LookFrom = new Vec3(15, 2.5, 4),
            LookAt = new Vec3(0, 0.5, 0),
            Vup = new Vec3(0, 1, 0),
            DefocusAngle = 0.7,
            FocusDist = 12.0
        };

        cam.Render(world, "advanced.png");
    }

    static HittableList CreateTutorialWorld(bool smallerGrid = false)
    {
        var world = new HittableList();
        var rng = new Random(42);

        var ground = new Lambertian(new Vec3(0.5, 0.5, 0.5));
        world.Add(new Sphere(new Vec3(0, -1000, 0), 1000, ground));

        int gridSize = smallerGrid ? 5 : 11;
        for (int a = -gridSize; a < gridSize; a++)
        {
            for (int b = -gridSize; b < gridSize; b++)
            {
                var choose = rng.NextDouble();
                var center = new Vec3(a + 0.9 * rng.NextDouble(), 0.2, b + 0.9 * rng.NextDouble());

                if ((center - new Vec3(4, 0.2, 0)).Length() > 0.9)
                {
                    IMaterial mat;
                    if (choose < 0.8)
                        mat = new Lambertian(Vec3.Random(rng) * Vec3.Random(rng));
                    else if (choose < 0.95)
                        mat = new Metal(Vec3.Random(rng, 0.5, 1), rng.NextDouble() * 0.5);
                    else
                        mat = new Dielectric(1.5);

                    world.Add(new Sphere(center, 0.2, mat));
                }
            }
        }

        world.Add(new Sphere(new Vec3(0, 1, 0), 1.0, new Dielectric(1.5)));
        world.Add(new Sphere(new Vec3(-4, 1, 0), 1.0, new Lambertian(new Vec3(0.4, 0.2, 0.1))));
        world.Add(new Sphere(new Vec3(4, 1, 0), 1.0, new Metal(new Vec3(0.7, 0.6, 0.5), 0.0)));

        return world;
    }
}
