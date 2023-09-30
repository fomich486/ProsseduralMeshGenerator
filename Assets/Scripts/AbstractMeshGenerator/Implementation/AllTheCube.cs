using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTheCube : AbstractMeshGenerator {

  public Vector3[] vs = new Vector3[8];

  protected override void SetMeshNums() {
    _numVertices = 36;
    _numTriangles = 36;
  }

  protected override void SetVertices() {
    //front
    _vertices.Add(vs[2]);
    _vertices.Add(vs[3]);
    _vertices.Add(vs[0]);
    
    _vertices.Add(vs[0]);
    _vertices.Add(vs[1]);
    _vertices.Add(vs[2]);
    
    //right
    _vertices.Add(vs[3]);
    _vertices.Add(vs[2]);
    _vertices.Add(vs[6]);
    
    _vertices.Add(vs[7]);
    _vertices.Add(vs[3]);
    _vertices.Add(vs[6]);
    
    //left
    _vertices.Add(vs[0]);
    _vertices.Add(vs[4]);
    _vertices.Add(vs[1]);
    
    _vertices.Add(vs[4]);
    _vertices.Add(vs[5]);
    _vertices.Add(vs[1]);
    
    //top
    _vertices.Add(vs[6]);
    _vertices.Add(vs[2]);
    _vertices.Add(vs[5]);
    
    _vertices.Add(vs[5]);
    _vertices.Add(vs[2]);
    _vertices.Add(vs[1]);
    
    
    //bottom
    _vertices.Add(vs[7]);
    _vertices.Add(vs[4]);
    _vertices.Add(vs[0]);
    
    _vertices.Add(vs[0]);
    _vertices.Add(vs[3]);
    _vertices.Add(vs[7]);
    
    //back
    _vertices.Add(vs[7]);
    _vertices.Add(vs[6]);
    _vertices.Add(vs[5]);
    
    _vertices.Add(vs[5]);
    _vertices.Add(vs[4]);
    _vertices.Add(vs[7]);
    
    
    
    
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