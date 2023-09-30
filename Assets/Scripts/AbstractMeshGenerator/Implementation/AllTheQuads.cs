using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTheQuads : AbstractMeshGenerator {
  public Vector3[] vs = new Vector3[4];
  public Vector2[] flexibleUVs = new Vector2[4];

  protected override void SetMeshNums() {
    _numVertices = 4;
    _numTriangles = 6;
  }

  protected override void SetNormals() { }

  protected override void SetTangents() { }

  protected override void SetUVs() {
    _uvs.AddRange(flexibleUVs);
  }

  protected override void SetVertexColor() { }

  protected override void SetTriangles() {
    _triangles.Add(0);
    _triangles.Add(3);
    _triangles.Add(2);

    _triangles.Add(0);
    _triangles.Add(1);
    _triangles.Add(3);
  }

  protected override void SetVertices() {
    _vertices.AddRange(vs);
  }
}