using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProsseduralLandscapeGenerator : AbstractLandscapeMeshGenerator {
  // [Header("Geometry")] public int xResolution = 20;
  // public int zResolution = 20;
  // public float meshScale = 1;
  // public float yScale = 1;
  //
  // [Header("Noise")] [Range(1, 9)] public int octaves;
  // public float lacunarity = 2f;
  // [Range(0f, 1f)] public float gain = 0.5f;
  // public float perlinScale = 1;
  // public FallOffType type;
  // public float fallOffSize = 1f;
  // public float seaLevel;

  [Header("UV settings")] public float uvScale = 1;
  public Gradient gradient;
  public float gradMin;
  public float gradMax;




  protected override void SetMeshNums() {
    _numVertices = (xResolution + 1) * (zResolution + 1);
    _numTriangles = 6 * xResolution * zResolution;
  }

  protected override void SetVertices() {
    float xx, y, zz = 0;
    var noiseGenerator = new NoiseGenerator(octaves, lacunarity, gain, perlinScale);

    for (int z = 0; z <= zResolution; z++) {
      for (int x = 0; x <= xResolution; x++) {
        xx = ((float) x / xResolution) * meshScale;
        zz = ((float) z / zResolution) * meshScale;
        y = yScale * noiseGenerator.GetFractalNoise(xx, zz);
        y = FallOff(x, y, z);

        _vertices.Add(new Vector3(xx, y, zz));
      }
    }
  }

  protected override void SetTriangles() {
    int triCount = 0;
    for (int z = 0; z < zResolution; z++) {
      for (int x = 0; x < xResolution; x++) {
        _triangles.Add(triCount);
        _triangles.Add(triCount + xResolution + 1);
        _triangles.Add(triCount + 1);

        _triangles.Add(triCount + 1);
        _triangles.Add(triCount + xResolution + 1);
        _triangles.Add(triCount + xResolution + 2);

        triCount++;
      }

      triCount++;
    }
  }

  protected override void SetNormals() {
    //SetGeneralNormals();
  }

  protected override void SetTangents() {
    //SetGeneralTangents();
  }

  protected override void SetUVs() {
    for (int z = 0; z <= zResolution; z++) {
      for (int x = 0; x <= xResolution; x++) {
        _uvs.Add(new Vector2(x / (uvScale * xResolution), z / (uvScale * zResolution)));
      }
    }
  }

  protected override void SetVertexColor() {
    var diff = gradMax - gradMin;
    for (int i = 0; i < _numVertices; i++) {
      _vertexColors.Add(gradient.Evaluate(_vertices[i].y - gradMin) / diff);
    }
  }
}