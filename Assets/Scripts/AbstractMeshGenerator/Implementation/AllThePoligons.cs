using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AllThePoligons : AbstractMeshGenerator {
  [Range(3, 50)] public int numSides = 3;
  public float radius;
  public float xTiling = 1;
  public float yTiling = 1;

  public float xScroll = 1;
  public float yScroll = 1;
  public float angle = 0;

  protected override void SetMeshNums() {
    _numVertices = numSides;
    _numTriangles = 3 * (numSides - 2);
  }

  protected override void SetNormals() {
    Vector3 normal = new Vector3(0, 0, -1);
    for (int i = 0; i < _numVertices; i++) {
      _normals.Add(normal);
    }
  }

  protected override void SetTangents() {  
    Vector3 tangent3 = new Vector3(1,0, 0);
    Vector3 rotatedTangent = Quaternion.AngleAxis(angle, -Vector3.forward) * tangent3;
    Vector4 tangent = rotatedTangent;
    tangent.w = -1;
    for (int i = 0; i < _numVertices; i++) {
      _tangets.Add(tangent);
    }
  }

  protected override void SetUVs() {
    Debug.Log(_vertices.Count);
    for (int i = 0; i < _numVertices; i++) {
      float uvX = xTiling * _vertices[i].x + xScroll;
      float uvY = yTiling * _vertices[i].y + yScroll;
      var uv = new Vector2(uvX, uvY);

      _uvs.Add(Quaternion.AngleAxis(angle, Vector3.forward) * uv);
    }
  }

  protected override void SetVertexColor() { }

  protected override void SetTriangles() {
    for (int i = 1; i < numSides - 1; i++) {
      _triangles.Add(0);
      _triangles.Add(i + 1);
      _triangles.Add(i);
    }
  }

  protected override void SetVertices() {
    for (int i = 0; i < numSides; i++) {
      var angle = 2 * Mathf.PI * i / numSides;
      _vertices.Add(new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0));
    }
  }
}