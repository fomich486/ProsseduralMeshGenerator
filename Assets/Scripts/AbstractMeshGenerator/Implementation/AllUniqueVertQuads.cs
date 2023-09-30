using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllUniqueVertQuads : AbstractMeshGenerator {
    public Vector3[] vs = new Vector3[4];
    public Vector2[] flexibleUVs = new Vector2[4];
    public Vector2[] flexibleUniqueUvs = new Vector2[6];
    
    protected override void SetMeshNums() {
        _numVertices = 6;
        _numTriangles = 6;
    }

    protected override void SetNormals() { }

    protected override void SetTangents() { }

    protected override void SetUVs() {
        // _uvs.Add(flexibleUVs[0]);
        // _uvs.Add(flexibleUVs[1]);
        // _uvs.Add(flexibleUVs[3]);
        //
        // _uvs.Add(flexibleUVs[0]);
        // _uvs.Add(flexibleUVs[3]);
        // _uvs.Add(flexibleUVs[2]);
        _uvs.AddRange(flexibleUniqueUvs);
    }

    protected override void SetVertexColor() { }

    protected override void SetTriangles() {
        _triangles.Add(0);
        _triangles.Add(1);
        _triangles.Add(2);

        _triangles.Add(3);
        _triangles.Add(4);
        _triangles.Add(5);
    }

    protected override void SetVertices() {
        _vertices.Add(vs[0]);
        _vertices.Add(vs[1]);
        _vertices.Add(vs[3]);
        
        _vertices.Add(vs[0]);
        _vertices.Add(vs[3]);
        _vertices.Add(vs[2]);
    }
}
