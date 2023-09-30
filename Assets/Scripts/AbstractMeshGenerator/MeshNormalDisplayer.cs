using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshNormalDisplayer : MonoBehaviour {
  public bool drawNormals;
  public float normalLength = 0.5f;

  void OnDrawGizmosSelected() {
    if (drawNormals) {
      Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

      if (mesh != null) {
        for (int i = 0; i < mesh.vertexCount; i++) {
          //change these to world space so they display normals when move transform
          Vector3 vertex = transform.TransformPoint(mesh.vertices[i]);
          Vector3 normal = transform.TransformDirection(mesh.normals[i]);

          Gizmos.color = Color.blue;
          Gizmos.DrawLine(vertex, vertex + normalLength * normal);
        }
      }
    }
  }

}