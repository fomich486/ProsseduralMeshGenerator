using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractLandscapeMeshGenerator : AbstractMeshGenerator {
  [Header("Geometry")] public int xResolution = 20;
  public int zResolution = 20;
  public float meshScale = 1;
  public float yScale = 1;

  [Header("Noise")] [Range(1, 9)] public int octaves;
  public float lacunarity = 2f;
  [Range(0f, 1f)] public float gain = 0.5f;
  public float perlinScale = 1;
  public FallOffType type;
  public float fallOffSize = 1f;
  public float seaLevel;

  protected override void SetVertices() { }
  protected override void SetTriangles() { }
  protected override void SetNormals() { }
  protected override void SetTangents() { }
  protected override void SetUVs() { }
  protected override void SetVertexColor() { }
  
  protected float FallOff(float x, float height, float z) {
    x = x - xResolution / 2f;
    z = z - zResolution / 2f;

    float fallOff = 0;
    switch (type) {
      case FallOffType.None:
        return height;
      case FallOffType.Circular:
        fallOff = Mathf.Sqrt(x * x + z * z) / fallOffSize;
        return GetHeight(fallOff, height);
      case FallOffType.RoundSquare:
        fallOff = Mathf.Sqrt(x * x * x * x + z * z * z * z) / fallOffSize;
        return GetHeight(fallOff, height);

      default:
        Debug.LogError("Unrecognized fall type");
        return height;
    }
  }

  protected float GetHeight(float fallOff, float height) {
    if (fallOff < 1) {
      fallOff = fallOff * fallOff * (3 - 2 * fallOff);
      height = height - fallOff * (height - seaLevel);
    }
    else {
      height = seaLevel;
    }

    return height;
  }
}