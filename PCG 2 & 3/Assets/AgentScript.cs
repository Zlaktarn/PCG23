using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentScript : MonoBehaviour
{
    public int mDivisions;
    public float mSize;
    public float mHeight;

    Vector3[] mVerts;
    int mVertCount;
    public GameObject treePrefab;
    public GameObject treePrefab1;
    public GameObject housePrefab;


    public int tDensity;
    public int treeRarity;
    public int hDensity;
    public int houseRarity;

    // Start is called before the first frame update
    void Start()
    {
        CreateTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateTerrain()
    {
        mVertCount = (mDivisions + 1) * (mDivisions + 1);
        mVerts = new Vector3[mVertCount];
        Vector2[] uvs = new Vector2[mVertCount];
        int[] tris = new int[mDivisions * mDivisions * 6];

        float halfSize = mSize * 0.5f;
        float divisionSize = mSize / mDivisions;

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int triOffset = 0;

        for (int i = 0; i <= mDivisions; i++)
        {
            for (int j = 0; j <= mDivisions; j++)
            {
                mVerts[i * (mDivisions + 1) + j] = new Vector3(-halfSize + j * divisionSize, 0.0f, halfSize - i * divisionSize);
                uvs[i * (mDivisions + 1) + j] = new Vector2((float)i / mDivisions, (float)j / mDivisions);

                if (i < mDivisions && j < mDivisions)
                {
                    int topLeft = i * (mDivisions + 1) + j;
                    int botLeft = (i + 1) * (mDivisions + 1) + j;

                    tris[triOffset] = topLeft;
                    tris[triOffset + 1] = topLeft + 1;
                    tris[triOffset + 2] = botLeft + 1;

                    tris[triOffset + 3] = topLeft;
                    tris[triOffset + 4] = botLeft + 1;
                    tris[triOffset + 5] = botLeft;

                    triOffset += 6;
                }
            }
        }

        //mVerts[0].y = Random.Range(-mHeight, mHeight);
        //mVerts[mDivisions].y = Random.Range(-mHeight, mHeight);
        //mVerts[mVerts.Length - 1].y = Random.Range(-mHeight, mHeight);
        //mVerts[mVerts.Length - 1 - mDivisions].y = Random.Range(-mHeight, mHeight);

        int iterations = (int)Mathf.Log(mDivisions, 2);
        int numSquares = 1;
        int squareSize = mDivisions;
        for (int i = 0; i < iterations; i++)
        {
            int row = 0;
            for (int j = 0; j < numSquares; j++)
            {
                int col = 0;
                for (int k = 0; k < numSquares; k++)
                {
                    int r = Random.Range(1, 100);

                    if(r == 1)
                        DiamondSquare(row, col, squareSize, mHeight);

                    col += squareSize;
                }
                row += squareSize;
            }
            numSquares *= 2;
            squareSize /= 2;
            mHeight *= 0.5f;
        }


        

        mesh.vertices = mVerts;
        mesh.uv = uvs;
        mesh.triangles = tris;


        for (int i = 0; i < mVerts.Length - 1; i++)
        {
            if (mVerts[i].y == 0)
            {
                if (mVerts[i].x > (-mSize * 0.5f) + 30 && mVerts[i].x < mSize * 0.5f - 30 && mVerts[i].z > -mSize * 0.5f + 30 && mVerts[i].z < mSize * 0.5f - 30)
                {
                    int random = Random.Range(1, treeRarity);

                    if (random == 1)
                    {
                        float distance = Random.Range(-tDensity, tDensity);
                        GameObject go = Instantiate(treePrefab, new Vector3(mVerts[i].x - distance, mVerts[i].y - 0.2f, mVerts[i].z - distance), Quaternion.identity);
                    }
                    if (random == 2)
                    {
                        float distance = Random.Range(-tDensity, tDensity);
                        GameObject go1 = Instantiate(treePrefab1, new Vector3(mVerts[i].x + distance, mVerts[i].y - 0.2f, mVerts[i].z + distance), Quaternion.identity);
                    }

                }
            }
        }

        for (int i = 0; i < mVerts.Length - 1; i++)
        {
            if (mVerts[i].y == 0)
            {
                if (mVerts[i].x > (-mSize * 0.5f) + 30 && mVerts[i].x < mSize * 0.5f -30 && mVerts[i].z > -mSize * 0.5f  + 30 && mVerts[i].z < mSize * 0.5f - 30)
                {
                    int random = Random.Range(1, houseRarity);

                    if (random == 1)
                    {
                        float distance = Random.Range(-hDensity, hDensity);
                        GameObject go = Instantiate(housePrefab, new Vector3(mVerts[i].x - distance, mVerts[i].y - 0.2f, mVerts[i].z - distance), Quaternion.identity);
                    }
                }
            }
        }

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    void DiamondSquare(int row, int col, int size, float offset)
    {
        int halfSize = (int)(size * 0.5f);
        int topLeft = row * (mDivisions + 1) + col;
        int botLeft = (row + size) * (mDivisions + 1) + col;

        int mid = (int)(row + halfSize) * (mDivisions + 1) + (int)(col + halfSize);
        mVerts[mid].y = (mVerts[topLeft].y + mVerts[topLeft + size].y + mVerts[botLeft].y + mVerts[botLeft + size].y) * 0.25f + Random.Range(-offset, offset);

        mVerts[topLeft + halfSize].y = (mVerts[topLeft].y + mVerts[topLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
        mVerts[mid - halfSize].y = (mVerts[topLeft].y + mVerts[botLeft].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
        mVerts[mid + halfSize].y = (mVerts[topLeft + size].y + mVerts[botLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
        mVerts[botLeft + halfSize].y = (mVerts[botLeft].y + mVerts[botLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
    }
}
