using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class PGC_Mesh : MonoBehaviour
{

    public int xSize;
    public int ySize;

    public float amplitude;
    public float speed;

    private Mesh mesh;
    private Vector3[] verts;
    private Vector3[] tris;
    private Rigidbody cubeRB;
    private MeshRenderer mr;
    private MeshFilter mf;
    private MeshCollider mc;
    void Start()
    {

        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        //create our verts
        verts = new Vector3[ (xSize + 1) * (ySize+1)];
        //create tris
        int[] tris = new int[xSize * 6 * ySize];
        //create UV
        Vector2[] uv = new Vector2[verts.Length];

        //position verts in grid
        for (int i = 0, y = 0; y <= ySize; y+=1)
        {
            for (int x = 0; x <= xSize; x+=1, i+=1)
            {
                verts[i] = new Vector3(x,0, y);
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
            }
        }
        
        mesh.vertices = verts;
        mesh.uv = uv;
        mesh.name = "Procedural mesh";
        for (int triangleI = 0, vertI = 0, y = 0; y < ySize; y++, vertI++)
        {
            for(int x = 0; x < xSize; x++, triangleI += 6 , vertI++)
            {
                tris[triangleI] = vertI;
                tris[triangleI + 3] = tris[triangleI + 2] = vertI + 1;
                tris[triangleI + 4] = tris[triangleI + 1] = vertI + xSize + 1;
                tris[triangleI + 5] = vertI + xSize + 2;
                
                
            }
           
        }
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mc.sharedMesh = mesh;


    }


    // Update is called once per frame
    void Update()
    {
        Mesh newMesh = new Mesh();
        newMesh = mf.mesh;

        Vector3[] verts = newMesh.vertices;
        Vector3[] normals = newMesh.normals;

        
        for (var i = 0; i < verts.Length; i++)
        {
            verts[i] = new Vector3(verts[i].x, (verts[i].y+1) * (amplitude * Mathf.Sin(Time.time * i * speed)), (verts[i].z));
        }
        
        newMesh.vertices = verts;
        mc.sharedMesh = newMesh;
        

    }
    private void Awake()
    {
        mf = gameObject.AddComponent<MeshFilter>();
        mr = gameObject.GetComponent<MeshRenderer>();
        mc = gameObject.AddComponent<MeshCollider>();

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(20, 20, 20);
        cubeRB = cube.AddComponent<Rigidbody>();
        cubeRB.useGravity = true;
    }

}
