using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TerrainGenerator.InfiniteTerrain {
  public class ProcGenChunk : AbstractLandscapeMeshGenerator {

    public int xStartPos;
    public int zStartPos;
    public int xEndPos;
    public int zEndPos;

    private int zOuterStartPos;
    private int zOuterEndPos;
    private int xOuterStartPos;
    private int xOuterEndPos;

    private float uvScale;

    private List<Vector3> outerVertices = new List<Vector3>();
    private List<int> outerTriangle = new();
    private List<Vector2> outerUvs = new();

    public void InitInfiniteLandscapeGenerator(Material mat, int xRes, int zRes, float meshScale, float yScale,
      int octaves, float lacunarity, float gain, float perlinScale, float uvScale, Vector2 startPosition) {
      this.material = mat;
      this.xResolution = xRes;
      this.zResolution = zRes;
      this.meshScale = meshScale;
      this.yScale = yScale;
      this.octaves = octaves;
      this.lacunarity = lacunarity;
      this.gain = gain;
      this.perlinScale = perlinScale;
      this.uvScale = uvScale;

      xStartPos = (int) startPosition.x;
      zStartPos = (int) startPosition.y;
      xEndPos = xStartPos + xRes;
      zEndPos = zStartPos + zRes;

      zOuterStartPos = zStartPos - 1;
      zOuterEndPos = zEndPos + 1;
      xOuterStartPos = xStartPos - 1;
      xOuterEndPos = xEndPos + 1;

      type = FallOffType.None;
    }

    protected override void SetMeshNums() {
      _numVertices = (xResolution + 1) * (zResolution + 1);
      _numTriangles = 6 * xResolution * zResolution;
    }

    protected override void SetVertices() {
      float xx, y, zz = 0;
      var noiseGenerator = new NoiseGenerator(octaves, lacunarity, gain, perlinScale);

      for (int z = zOuterStartPos; z <= zOuterEndPos; z++) {
        for (int x = xOuterStartPos; x <= xOuterEndPos; x++) {
          xx = ((float) x / xResolution) * meshScale;
          zz = ((float) z / zResolution) * meshScale;
          y = yScale * noiseGenerator.GetFractalNoise(xx, zz);
          y = FallOff(x, y, z);

          var vertex = new Vector3(xx, y, zz);

          outerVertices.Add(vertex);
          if (z >= zStartPos && z <= zEndPos && x >= xStartPos && x <= xEndPos) {
            _vertices.Add(vertex);
          }
        }
      }
    }

    protected override void SetTriangles() {
      int outerTriCount = 0;
      int triCount = 0;
      for (int z = zOuterStartPos; z < zOuterEndPos; z++) {
        for (int x = xOuterStartPos; x < xOuterEndPos; x++) {
          outerTriangle.Add(outerTriCount);
          outerTriangle.Add(outerTriCount + xResolution + 3);
          outerTriangle.Add(outerTriCount + 1);

          outerTriangle.Add(outerTriCount + 1);
          outerTriangle.Add(outerTriCount + xResolution + 3);
          outerTriangle.Add(outerTriCount + xResolution + 4);

          outerTriCount++;

          if (z >= zStartPos && z < zEndPos && x >= xStartPos && x < xEndPos) {
            _triangles.Add(triCount);
            _triangles.Add(triCount + xResolution + 1);
            _triangles.Add(triCount + 1);

            _triangles.Add(triCount + 1);
            _triangles.Add(triCount + xResolution + 1);
            _triangles.Add(triCount + xResolution + 2);

            triCount++;
          }
        }

        if (z >= zStartPos && z < zEndPos) {
          triCount++;
        }

        outerTriCount++;
      }
    }

    protected override void SetNormals() {
      int numGeometricTriangles = outerTriangle.Count / 3;
      Vector3[] norms = new Vector3[outerVertices.Count];
      int index = 0;
      for (int i = 0; i < numGeometricTriangles; i++) {
        int triA = outerTriangle[index];
        int triB = outerTriangle[index + 1];
        int triC = outerTriangle[index + 2];

        var dirA = outerVertices[triB] - outerVertices[triA];
        var dirB = outerVertices[triC] - outerVertices[triA];

        var normal = Vector3.Cross(dirA, dirB);

        norms[triA] += normal;
        norms[triB] += normal;
        norms[triC] += normal;

        index += 3;
      }

      int outerWidth = xResolution + 2;
      for (int i = 0; i < outerVertices.Count; i++) {
        if (i % (outerWidth + 1) == 0 || i % (outerWidth + 1) == outerWidth) {
          continue;
        }

        if (i <= outerWidth || i >= outerVertices.Count - outerWidth) {
          continue;
        }

        _normals.Add(norms[i].normalized);
      }
    }

    protected override void SetTangents() {
      if (_uvs.Count == 0 || _normals.Count == 0) {
        Debug.LogError("Set UVS and normals before adding tangents");
      }

      int numGeometricalTriangles = outerTriangle.Count / 3;
      Vector3[] tans = new Vector3[outerVertices.Count];
      Vector3[] bitans = new Vector3[outerVertices.Count];
      int index = 0;
      for (int i = 0; i < numGeometricalTriangles; i++) {
        int triA = outerTriangle[index];
        int triB = outerTriangle[index + 1];
        int triC = outerTriangle[index + 2];

        Vector2 uvA = outerUvs[triA];
        Vector2 uvB = outerUvs[triB];
        Vector2 uvC = outerUvs[triC];

        var dirA = outerVertices[triB] - outerVertices[triA];
        var dirB = outerVertices[triC] - outerVertices[triA];

        var uvDiffA = new Vector2(uvB.x - uvA.x, uvC.x - uvA.x);
        var uvDiffB = new Vector2(uvB.y - uvA.y, uvC.y - uvA.y);

        float invDet = uvDiffA.x * uvDiffB.y - uvDiffA.y * uvDiffB.x;
        if (invDet == 0) {
          Debug.LogError("Invalid determinant");
          return;
        }

        var determinant = 1f / invDet;
        var sDir = determinant *
                   (new Vector3(uvDiffB.y * dirA.x - uvDiffB.x * dirB.x,
                     uvDiffB.y * dirA.y - uvDiffB.x * dirB.y,
                     uvDiffB.y * dirA.z - uvDiffB.x * dirA.z));
        var tDir = determinant *
                   (new Vector3(uvDiffA.x * dirB.x - uvDiffA.y * dirA.x,
                     uvDiffA.x * dirB.y - uvDiffA.y * dirA.y,
                     uvDiffA.x * dirB.z - uvDiffA.y * dirA.z));

        tans[triA] += sDir;
        tans[triB] += sDir;
        tans[triC] += sDir;

        bitans[triA] += tDir;
        bitans[triB] += tDir;
        bitans[triC] += tDir;

        index += 3;
      }

      int outerWidth = xResolution + 2;
      int normalIndex = 0;
      for (int i = 0; i < outerVertices.Count; i++) {
        if (i % (outerWidth + 1) == 0 || i % (outerWidth + 1) == outerWidth) {
          continue;
        }

        if (i <= outerWidth || i >= outerVertices.Count - outerWidth) {
          continue;
        }

        var normal = _normals[normalIndex];
        normalIndex++;

        var tan = tans[i];
        var tangent3 = (tan - Vector3.Dot(normal, tan) * normal).normalized;
        Vector4 tangent = tangent3;
        tangent.w = Vector3.Dot(Vector3.Cross(normal, tan), bitans[i]) < 0f ? -1f : 1f;
        _tangets.Add(tangent);
      }
    }

    protected override void SetUVs() {
      for (int z = zOuterStartPos; z <= zOuterEndPos; z++) {
        for (int x = xOuterStartPos; x <= xOuterEndPos; x++) {
          outerUvs.Add(new Vector2(x / (uvScale * xResolution), z / (uvScale * zResolution)));
          if (z >= zStartPos && z <= zEndPos && x >= xStartPos && x <= xEndPos) {
            _uvs.Add(new Vector2(x / (uvScale * xResolution), z / (uvScale * zResolution)));
          }
        }
      }
    }

    protected override void SetVertexColor() { }
  }
}