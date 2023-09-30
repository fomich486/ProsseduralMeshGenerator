using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPrisms : AbstractMeshGenerator {
  [Range(3, 50)] public int numSides = 3;
  public float frontRadius;
  public float backRadius;
  public float length;

  public Gradient gradient;

  private Vector3[] vs;

  protected override void SetMeshNums() {
    _numVertices = 6 * numSides;
    _numTriangles = 12 * (numSides - 1);
  }

  protected override void SetVertices() {
    vs = new Vector3[2 * numSides];

    for (int i = 0; i < numSides; i++) {
      var angle = 2 * Mathf.PI * i / numSides;
      vs[i] = new Vector3(frontRadius * Mathf.Cos(angle), frontRadius * Mathf.Sin(angle), 0);
      vs[i + numSides] = new Vector3(backRadius * Mathf.Cos(angle), backRadius * Mathf.Sin(angle), length);
    }

    //first end
    for (int i = 0; i < numSides; i++) {
      _vertices.Add(vs[i]);
    }

    //middle
    for (int i = 0; i < numSides; i++) {
      _vertices.Add(vs[i]);
      var secondIndex = i == 0 ? 2 * numSides - 1 : numSides + i - 1;
      _vertices.Add(vs[secondIndex]);
      var thirdIndex = i == 0 ? numSides - 1 : i - 1;
      _vertices.Add(vs[thirdIndex]);
      _vertices.Add(vs[numSides + i]);
    }

    //second end
    for (int i = 0; i < numSides; i++) {
      _vertices.Add(vs[numSides + i]);
    }
  }

  protected override void SetTriangles() {
    //first end
    for (int i = 1; i < numSides - 1; i++) {
      _triangles.Add(0);
      _triangles.Add(i + 1);
      _triangles.Add(i);
    }

    //middle
    for (int i = 1; i <= numSides; i++) {
      var val = numSides + 4 * (i - 1);

      _triangles.Add(val);
      _triangles.Add(val + 1);
      _triangles.Add(val + 2);

      _triangles.Add(val);
      _triangles.Add(val + 3);
      _triangles.Add(val + 1);
    }

    //second end
    for (int i = 1; i < numSides - 1; i++) {
      _triangles.Add(5 * numSides);
      _triangles.Add(5 * numSides + i);
      _triangles.Add(5 * numSides + i + 1);
    }
  }

  protected override void SetNormals() {
    SetGeneralNormals();
    // Vector3 frontNormal = new Vector3(0, 0, -1);
    // for (int i = 0; i < numSides; i++) {
    //   _normals.Add(frontNormal);
    // }
    //
    // for (int i = 0; i < numSides; i++) {
    //   int index = numSides + 4 * i;
    //   Vector3 dirOne = _vertices[index + 2] - _vertices[index];
    //   Vector3 dirTwo = _vertices[index + 3] - _vertices[index];
    //
    //   Vector3 normal = Vector3.Cross(dirTwo, dirOne).normalized;
    //   for (int j = 0; j < 4; j++) {
    //     _normals.Add(normal);
    //   }
    // }
    //
    // Vector3 backNormal = new Vector3(0, 0, 1);
    // for (int i = 0; i < numSides; i++) {
    //   _normals.Add(backNormal);
    // }
  }

  protected override void SetTangents() {
    SetGeneralTangents();
    // var frontTangents = new Vector4(1, 0, 0, -1);
    // for (int i = 0; i < numSides; i++) {
    //   _tangets.Add(frontTangents);
    // }
    //
    // for (int i = 0; i < numSides; i++) {
    //   int index = numSides + 4 * i;
    //   var uDir = _vertices[index] - _vertices[index + 2];
    //   Vector4 sideTangent = uDir.normalized;
    //   sideTangent.w = 1;
    //   for (int j = 0; j < 4; j++) {
    //     _tangets.Add(sideTangent);
    //   }
    // }
    //
    // var backTangents = new Vector4(1, 0, 0, 1);
    // for (int i = 0; i < numSides; i++) {
    //   _tangets.Add(backTangents);
    //
    // }
  }

  protected override void SetUVs() {
    for (int i = 0; i < numSides; i++) {
      _uvs.Add(vs[i]);
    }

    for (int i = 0; i < numSides; i++) {
      _uvs.Add(new Vector2(frontRadius, 0));
      _uvs.Add(new Vector2(0, length));
      _uvs.Add(new Vector2(0, 0));
      _uvs.Add(new Vector2(backRadius, length));
    }

    for (int i = 0; i < numSides; i++) {
      _uvs.Add(vs[i + numSides]);
    }
  }

  protected override void SetVertexColor() {
    for (int i = 0; i < _numVertices; i++) {
      _vertexColors.Add(gradient.Evaluate((float) i / _numTriangles));
    }
  }
}