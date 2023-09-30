using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshTangentDisplayer : MonoBehaviour {
  public bool drawTangent;
  public float tangentLength = 0.5f;

  void OnDrawGizmosSelected() {
    if (drawTangent) {
      Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

      if (mesh != null) {
        for (int i = 0; i < mesh.vertexCount; i++) {
          //change these to world space so they display tangent when move transform
          Vector3 vertex = transform.TransformPoint(mesh.vertices[i]);
          Vector3 tangent = transform.TransformDirection(mesh.tangents[i]);

          Gizmos.color = Color.red;
          Gizmos.DrawLine(vertex, vertex + tangentLength * tangent);
        }
      }
    }
  }
}