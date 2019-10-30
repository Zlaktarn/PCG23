using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Vector3[] mVerts;
    Vector2[] mUVs;
    int[] mtris;

    // Start is called before the first frame update
    void Start()
    {
        mVerts = new Vector3[4];
        mUVs = new Vector2[4];
        mtris = new int[6];

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mVerts[0] = new Vector3(-1.0f, 0.0f, 1.0f);
        mVerts[1] = new Vector3(1.0f, 0.0f, 1.0f);
        mVerts[2] = new Vector3(-1.0f, 0.0f, -1.0f);
        mVerts[3] = new Vector3(1.0f, 0.0f, -1.0f);

        mUVs[0] = new Vector2(0.0f, 0.0f);
        mUVs[1] = new Vector2(1.0f, 0.0f);
        mUVs[2] = new Vector2(0.0f, 1.0f);
        mUVs[3] = new Vector2(1.0f, 1.0f);

        mtris[0] = 0;
        mtris[1] = 1;
        mtris[2] = 3;

        mtris[3] = 0;
        mtris[4] = 3;
        mtris[5] = 2;

        mesh.vertices = mVerts;
        mesh.uv = mUVs;
        mesh.triangles = mtris;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
