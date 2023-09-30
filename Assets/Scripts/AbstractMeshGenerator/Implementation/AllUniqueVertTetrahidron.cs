using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllUniqueVertTetrahidron : AbstractMeshGenerator {

  public Vector3[] vs = new Vector3[4];

  protected override void SetMeshNums() {
    _numVertices = 12; //one per vertex per side. 
    _numTriangles = 12; //tetrahedron has 4 sides, all triangular. 4 physical triangles * 3 ints for each = 12
  }

  protected override void SetVertices() {
    _vertices.Add(vs[0]);
    _vertices.Add(vs[2]);
    _vertices.Add(vs[1]);

    _vertices.Add(vs[0]);
    _vertices.Add(vs[3]);
    _vertices.Add(vs[2]);

    _vertices.Add(vs[2]);
    _vertices.Add(vs[3]);
    _vertices.Add(vs[1]);

    _vertices.Add(vs[1]);
    _vertices.Add(vs[3]);
    _vertices.Add(vs[0]);
    
  }

  protected override void SetTriangles() {
    for (int i = 0; i < _numTriangles; i++) {
      _triangles.Add(i);
    }
  }

  protected override void SetNormals() { }
  protected override void SetTangents() { }
  protected override void SetUVs() { }
  protected override void SetVertexColor() { }

}