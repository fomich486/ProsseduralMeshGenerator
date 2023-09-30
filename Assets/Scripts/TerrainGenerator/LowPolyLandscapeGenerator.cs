using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPolyLandscapeGenerator : AbstractLandscapeMeshGenerator {

  protected override void SetMeshNums() {
    _numTriangles = 6 * xResolution * zResolution;
    _numVertices = _numTriangles;
  }

  protected override void SetVertices() {
    var noiseGenerator = new NoiseGenerator(octaves, lacunarity, gain, perlinScale);
    int xx = 0;
    int zz = 0;

    bool isBottomTriangle = false;

    for (int vertexIndex = 0; vertexIndex < _numVertices; vertexIndex++) {
      if (IsNewRaw(vertexIndex)) {
        isBottomTriangle = !isBottomTriangle;
      }

      if (!IsNewRaw(vertexIndex)) {
        if (isBottomTriangle) {
          if (vertexIndex % 3 == 1) {
            xx++;
          }
        }
        else {
          if (vertexIndex % 3 == 2) {
            xx++;
          }
        }
      }

      if (IsNewRaw(vertexIndex)) {
        xx = 0;
        if (!isBottomTriangle) {
          zz++;
        }
      }


      float xVal = ((float) xx / xResolution) * meshScale;
      float zVal = ((float) zz / zResolution) * meshScale;
      float y = yScale * noiseGenerator.GetFractalNoise(xVal, zVal);
      y = FallOff(xx, y, zz);

      _vertices.Add(new Vector3(xVal, y, zVal));
    }
  }

  private bool IsNewRaw(int vertexIndex) {
    return vertexIndex % (3 * xResolution) == 0;
  }

  protected override void SetTriangles() {
    int triCount = 0;
    for (int z = 0; z < zResolution; z++) {
      for (int x = 0; x < xResolution; x++) {
        _triangles.Add(triCount);
        _triangles.Add(triCount + 3 * xResolution);
        _triangles.Add(triCount + 1);

        _triangles.Add(triCount + 2);
        _triangles.Add(triCount + 3 * xResolution + 1);
        _triangles.Add(triCount + 3 * xResolution + 2);

        triCount += 3;
      }

      triCount += 3 * xResolution;
    }
  }

  protected override void SetNormals() { }
  protected override void SetTangents() { }
  protected override void SetUVs() { }
  protected override void SetVertexColor() { }
}