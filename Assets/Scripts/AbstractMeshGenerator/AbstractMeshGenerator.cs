using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public abstract class AbstractMeshGenerator : MonoBehaviour {
  public Material material;

  protected List<Vector3> _vertices;
  protected List<int> _triangles;
  protected List<Vector3> _normals;
  protected List<Vector4> _tangets;
  protected List<Vector2> _uvs;
  protected List<Color32> _vertexColors;

  protected int _numVertices;
  protected int _numTriangles;

  protected MeshFilter _meshFilter;
  protected MeshRenderer _meshRenderer;
  protected MeshCollider _meshCollider;
  public Mesh mesh;


  public void InitMesh() {
    _vertices = new List<Vector3>();
    _triangles = new List<int>();
    _normals = new List<Vector3>();
    _tangets = new List<Vector4>();
    _uvs = new List<Vector2>();
    _vertexColors = new List<Color32>();
  }

  protected abstract void SetMeshNums();

  private bool ValidateMesh() {
    string errorStr = "";
    errorStr += _vertices.Count == _numVertices
      ? ""
      : $"Vertices should have {_numVertices}, but there are {_vertices.Count}";
    errorStr += _triangles.Count == _numTriangles
      ? ""
      : $"Triangles should have {_numTriangles},  but there are {_triangles.Count}";
    errorStr += _normals.Count == _numVertices || _normals.Count == 0
      ? ""
      : $"Normals should have {_numVertices}, but there are {_normals.Count}";
    errorStr += _tangets.Count == _numVertices || _tangets.Count == 0
      ? ""
      : $"Tangents should have {_numVertices}, but there are {_tangets.Count}";
    errorStr += _uvs.Count == _numVertices || _uvs.Count == 0
      ? ""
      : $"UVs should have {_numVertices}, but there are {_uvs.Count}";
    errorStr += _vertexColors.Count == _numVertices || _vertexColors.Count == 0
      ? ""
      : $"VertexColors should have {_numVertices}, but there are {_vertexColors.Count}";

    var result = string.IsNullOrEmpty(errorStr);
    if (!result) Debug.LogError(errorStr);
    return result;
  }

  //private void Update() {
  private void Start() {
    _meshFilter = GetComponent<MeshFilter>();
    _meshRenderer = GetComponent<MeshRenderer>();
    _meshCollider = GetComponent<MeshCollider>();

    _meshRenderer.sharedMaterial = material;
    InitMesh();
    SetMeshNums();
    SetVertices();
    SetTriangles();
    SetNormals();
    SetUVs();
    SetTangents();
    SetVertexColor();

    CreateMesh();
  }

  protected abstract void SetVertices();
  protected abstract void SetTriangles();
  protected abstract void SetNormals();
  protected abstract void SetTangents();
  protected abstract void SetUVs();
  protected abstract void SetVertexColor();

  protected void SetGeneralNormals() {
    int numGeometricTriangles = _numTriangles / 3;
    var norms = new Vector3[_numVertices];
    int index = 0;
    for (int i = 0; i < numGeometricTriangles; i++) {
      int triA = _triangles[index];
      int triB = _triangles[index + 1];
      int triC = _triangles[index + 2];

      var dirA = _vertices[triB] - _vertices[triC];
      var dirB = _vertices[triC] - _vertices[triA];

      var normal = Vector3.Cross(dirA, dirB);
      norms[triA] += normal;
      norms[triB] += normal;
      norms[triC] += normal;

      index += 3;
    }

    for (int i = 0; i < _numVertices; i++) {
      _normals.Add(norms[i].normalized);
    }
  }

  protected void SetGeneralTangents() {
    if (_uvs.Count == 0 || _normals.Count == 0) {
      Debug.Log("Set uvs and normals");
      return;
    }

    int numGeometricTriangles = _numTriangles / 3;
    var tans = new Vector3[_numVertices];
    var bitans = new Vector3[_numVertices];
    var index = 0;
    for (int i = 0; i < numGeometricTriangles; i++) {
      int triA = _triangles[index];
      int triB = _triangles[index + 1];
      int triC = _triangles[index + 2];

      //find UVs
      Vector2 uvA = _uvs[triA];
      Vector2 uvB = _uvs[triB];
      Vector2 uvC = _uvs[triC];


      var dirA = _vertices[triB] - _vertices[triA];
      var dirB = _vertices[triC] - _vertices[triA];

      var uvDiffA = new Vector2(uvB.x - uvA.x, uvC.x - uvA.x);
      var uvDiffB = new Vector2(uvB.y - uvA.y, uvC.y - uvA.y);

      var determinant = 1 / (uvDiffA.x * uvDiffB.y - uvDiffA.y * uvDiffB.x);
      var sDir = determinant *
                 new Vector3(uvDiffB.y * dirA.x - uvDiffB.x * dirB.x,
                   uvDiffB.y * dirA.y - uvDiffB.x * dirB.y,
                   uvDiffB.y * dirA.z - uvDiffB.x * dirB.z);
      var tDir = determinant *
                 new Vector3(uvDiffA.x * dirB.x - uvDiffA.y * dirA.x,
                   uvDiffA.x * dirB.y - uvDiffA.y * dirA.y,
                   uvDiffA.x * dirB.z - uvDiffA.y * dirA.z);

      tans[triA] += sDir;
      tans[triB] += sDir;
      tans[triC] += sDir;
      
      bitans[triA] += tDir;
      bitans[triB] += tDir;
      bitans[triC] += tDir;
      index += 3;
    }
    
    for (int i = 0; i < _numVertices; i++) {
      var normal = _normals[i];
      var tan = tans[i];

      var tangent3 = (tan - Vector3.Dot(normal, tan) * normal).normalized;
      Vector4 tangent = tangent3;
      tangent.w = Vector3.Dot(Vector3.Cross(normal, tan),bitans[i]) < 0f ? -1 : 1f;
      _tangets.Add(tangent);
    }
  }

  private void CreateMesh() {
    mesh = new Mesh();
    if (ValidateMesh()) {
      //Order v -> t requier
      mesh.SetVertices(_vertices);
      mesh.SetTriangles(_triangles, 0);
      if (_normals.Count == 0) {
        mesh.RecalculateNormals();
        _normals.AddRange(mesh.normals);
      }

      mesh.SetNormals(_normals);
      mesh.SetTangents(_tangets);
      mesh.SetUVs(0, _uvs);
      mesh.SetColors(_vertexColors);

      _meshFilter.mesh = mesh;
      _meshCollider.sharedMesh = mesh;
    }
  }

}