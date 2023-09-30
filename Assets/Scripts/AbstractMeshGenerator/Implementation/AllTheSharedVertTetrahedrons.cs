using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTheSharedVertTetrahedrons : AbstractMeshGenerator {

  public Vector3[] vs = new Vector3[4];

  protected override void SetMeshNums() {
    _numVertices = 4;
    _numTriangles = 12;
  }

  protected override void SetVertices() {
    _vertices.AddRange(vs);
  }

  protected override void SetTriangles() {
    //base
    _triangles.Add(0);
    _triangles.Add(2);
    _triangles.Add(1);
    
    //sides
    _triangles.Add(0);
    _triangles.Add(3);
    _triangles.Add(2);
    
    _triangles.Add(2);
    _triangles.Add(3);
    _triangles.Add(1);
    
    _triangles.Add(1);
    _triangles.Add(3);
    _triangles.Add(0);
  }
  
  protected override void SetNormals() { }

  protected override void SetTangents() { }

  protected override void SetUVs() { }

  protected override void SetVertexColor() { }

}