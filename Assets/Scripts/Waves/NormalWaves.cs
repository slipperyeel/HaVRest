using UnityEngine;
using System.Threading;
using System.Collections;

public class NormalWaves : MonoBehaviour
{
    [SerializeField] float scale = 0.1f;
    [SerializeField] float speed = 1.0f;
    [SerializeField] float noiseStrength = 1f;
    [SerializeField] float noiseWalk = 1f;

    private Vector3[] baseHeight;

    void OnStart()
    {
        Mesh sourceMesh = GetComponent<MeshFilter>().mesh;

        int[] sourceIndices = sourceMesh.GetTriangles(0);
        Vector3[] sourceVerts = sourceMesh.vertices;
        Vector2[] sourceUVs = sourceMesh.uv;

        int[] newIndices = new int[sourceIndices.Length];
        Vector3[] newVertices = new Vector3[sourceIndices.Length];
        Vector2[] newUVs = new Vector2[sourceIndices.Length];

        // Create a unique vertex for every index in the original Mesh:
        for (int i = 0; i < sourceIndices.Length; i++)
        {
            newIndices[i] = i;
            newVertices[i] = sourceVerts[sourceIndices[i]];
            newUVs[i] = sourceUVs[sourceIndices[i]];
        }

        Mesh unsharedVertexMesh = new Mesh();
        unsharedVertexMesh.vertices = newVertices;
        unsharedVertexMesh.uv = newUVs;

        unsharedVertexMesh.SetTriangles(newIndices, 0);
    }

    private void _ThreadWork(Vector3[] vec, float time)
    {
        Vector3[] vertices = new Vector3[vec.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseHeight[i];
            vertex.y += Mathf.Sin(time * speed + baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * scale;
            vertex.y += Mathf.PerlinNoise(baseHeight[i].x + noiseWalk, baseHeight[i].y + Mathf.Sin(time * 0.1f)) * noiseStrength;
            vertices[i] = vertex;
        }

        MainThread.Call(() => _Cb(vertices));
        
    }

    private void _Cb(Vector3[] verts)
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.vertices = verts;
        mesh.RecalculateNormals();
    }

    void Update()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        if (baseHeight == null)
            baseHeight = mesh.vertices;
        float time = Time.time;
        Thread myNewThread = new Thread(() => _ThreadWork(baseHeight, time));
        myNewThread.Start();

        //Vector3[] vertices = new Vector3[baseHeight.Length];
        //for (int i = 0; i < vertices.Length; i++)
        //{
        //    Vector3 vertex = baseHeight[i];
        //    vertex.y += Mathf.Sin(Time.time * speed + baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * scale;
        //    vertex.y += Mathf.PerlinNoise(baseHeight[i].x + noiseWalk, baseHeight[i].y + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;
        //    vertices[i] = vertex;
        //}

    }
}