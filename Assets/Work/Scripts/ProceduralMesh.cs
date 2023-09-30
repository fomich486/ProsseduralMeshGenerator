using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Seasons.Utility {
  [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
  public class ProceduralMesh : MonoBehaviour {

    public void Init(List<RaycastHit> hits, List<Material> materials) {
      if (materials.Count == 0) return;

      Debug.LogError("Init procedural mesh");
      var mesh = new Mesh {name = "Procedural Mesh"};
      mesh.vertices = new Vector3[] {
        hits[0].point, hits[1].point, hits[2].point, hits[0].point, hits[1].point, hits[2].point
      };
      mesh.triangles = new int[] {
        0, 1, 2, 3, 5, 4
      };
      mesh.normals = new Vector3[] {
        Vector3.back, Vector3.back, Vector3.back, Vector3.forward, Vector3.forward, Vector3.forward
      };
      mesh.uv = new Vector2[] {
        Vector2.down + Vector2.left, Vector2.up + Vector2.left, Vector2.down + Vector2.right,
        Vector2.up + Vector2.right, Vector2.up + Vector2.left, Vector2.down + Vector2.right
      };
      GetComponent<MeshFilter>().mesh = mesh;
      GetComponent<MeshRenderer>().material = materials.FirstOrDefault();
    }
  }
}