using System.Collections.Generic;
using UnityEngine;


public class AllTheTriangles : AbstractMeshGenerator {
  public bool reverceTrangel;
  public Vector3[] vs = new Vector3[3];


  protected override void SetMeshNums() {
    _numVertices = 3;
    _numTriangles = 3;
  }

  protected override void SetNormals() { }

  protected override void SetTangents() { }

  protected override void SetUVs() { }

  protected override void SetVertexColor() { }

  protected override void SetTriangles() {
    _vertices.AddRange(vs);
  }

  protected override void SetVertices() {
    if (!reverceTrangel) {
      _triangles.Add(0);
      _triangles.Add(1);
      _triangles.Add(2);
    }
    else {
      _triangles.Add(0);
      _triangles.Add(2);
      _triangles.Add(1);
    }
  }
}