using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator2D : AbstractMeshGenerator {

  [Header("Geometry")] public int resolution = 20;
  public float xScale = 1;
  public float yScale = 1;
  public float meshHeight = 1;

  [Header("Noise")] [Range(1, 9)] public int octaves;
  public float lacunarity = 2f;
  [Range(0f, 1f)] public float gain = 0.5f;
  public float perlinScale = 1;
  public int seed;

  [Header("UV settings")] public float uvScale = 1;
  public float numTexPerSquare = 1;
  public bool uvFollowSurface;
  public int sortingOrder = 0;

  protected override void SetMeshNums() {
    _numVertices = 2 * resolution;
    _numTriangles = 6 * (resolution - 1);
  }

  protected override void SetVertices() {
    float x, y = 0;
    Vector3[] vs = new Vector3[_numVertices];

    Random.InitState(seed);
    var noiseGenerator = new NoiseGenerator(octaves, lacunarity, gain, perlinScale);

    for (int i = 0; i < resolution; i++) {
      x = ((float) i / resolution) * xScale;
      y = yScale * noiseGenerator.GetFractalNoise(x, 0);

      vs[i] = new Vector3(x, y, 0);
      vs[i + resolution] = new Vector3(x, y - meshHeight, 0);
    }

    _vertices.AddRange(vs);
  }

  protected override void SetTriangles() {
    for (int i = 0; i < resolution - 1; i++) {
      _triangles.Add(i);
      _triangles.Add(i + resolution + 1);
      _triangles.Add(i + resolution);

      _triangles.Add(i);
      _triangles.Add(i + 1);
      _triangles.Add(i + resolution + 1);
    }
  }

  protected override void SetNormals() {
    SetGeneralNormals();
  }

  protected override void SetTangents() {
    SetGeneralTangents();
  }

  protected override void SetUVs() {
    _meshRenderer.sortingOrder = sortingOrder;
    var uvsArray = new Vector2[_numVertices];
    for (int i = 0; i < resolution; i++) {

      if (uvFollowSurface) {
        uvsArray[i] = new Vector2(i * numTexPerSquare / uvScale, 1);
        uvsArray[i + resolution] =
          new Vector2(i * numTexPerSquare / uvScale, 0);
      }
      else {
        uvsArray[i] = new Vector2(_vertices[i].x / uvScale, _vertices[i].y / uvScale);
        uvsArray[i + resolution] =
          new Vector2(_vertices[i + resolution].x / uvScale, _vertices[i + resolution].y / uvScale);
      }
    }

    _uvs.AddRange(uvsArray);
  }

  protected override void SetVertexColor() { }
}