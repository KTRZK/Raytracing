# RayTracer

Implementacja ray tracera w C# na podstawie "Ray Tracing in One Weekend" Petera Shirleya.

## Kompilacja

```
dotnet build -c Release
```

## Uruchomienie

```
dotnet run -c Release -- tutorial
```

Dostępne sceny:
- `step1` - gradient tła
- `step2` - pierwsza sfera  
- `step3` - normalne powierzchni
- `step4` - podłoże
- `step5` - materiały (diffuse, metal, szkło)
- `step6` - pozycjonowanie kamery
- `tutorial` - finalna scena z tutoriala (1200x675, 500 samples)
- `advanced` - zaawansowana scena własna
- `all` - wszystkie etapy

## Wygenerowane obrazki

- `step1_gradient.png` - gradient nieba
- `step2_sphere.png` - pierwsza sfera
- `step3_normals.png` - kolory wg normalnych
- `step4_ground.png` - sfera na podłożu
- `step5_materials.png` - różne materiały
- `step6_camera.png` - widok z kamery
- `tutorial_final.png` - finalna scena
- `advanced.png` - scena własna

## Czas renderowania

| Scena | Czas |
|-------|------|
| step1_gradient | 0.12 s |
| step2_sphere | 0.15 s |
| step3_normals | 0.2 s |
| step4_ground | 2 s |
| step5_materials | 8 s |
| step6_camera | 15 s |
| tutorial_final | ~40 min |
| advanced | ~3 h |
