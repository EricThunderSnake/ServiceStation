using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuboid : MonoBehaviour
{
    protected Mesh mesh;
    protected Vector2 x, y, z;
    protected Vector3[] vertices = new Vector3[8];

    public Vector3[] GetVertices()
    {
        return vertices;
    }

    public Vector2 GetX() { return x; }
    public Vector2 GetY() { return y; }
    public Vector2 GetZ() { return z; }

    public Face Front() { return faces[0]; }
    public Face Right() { return faces[1]; }
    public Face Top() { return faces[2]; }
    public Face Back() { return faces[3]; }
    public Face Left() { return faces[4]; }
    public Face Bottom() { return faces[5]; }


    [SerializeField] protected Face[] faces = new Face[6];

    public Face[] GetFaces()
    {
        return faces;
    }

    [System.Serializable]
    public struct Face
    {
        public Vector3[] points;
        public Vector3 normal;
        public float displacement;
        public Vector3 centre;
        public Face(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4)
        {
            points = new Vector3[] { point1, point2, point3, point4 };

            normal = Vector3.Cross(point2 - point1, point3 - point1);
            displacement = Vector3.Dot(normal, point4);
            centre = (point1 + point2 + point3 + point4) / 4;
        }
    }


    // Start is called before the first frame update
    protected void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        SetFaces();
    }

    protected void SetFaces()
    {
        for (int i = 0; i < 8; i++)
        {
            vertices[i] = transform.TransformPoint(mesh.vertices[i]);
        }

        faces[0] = new Face(vertices[0], vertices[1], vertices[3], vertices[2]);
        faces[1] = new Face(vertices[1], vertices[7], vertices[5], vertices[3]);
        faces[2] = new Face(vertices[3], vertices[5], vertices[4], vertices[2]);
        faces[3] = new Face(vertices[5], vertices[7], vertices[6], vertices[4]);
        faces[4] = new Face(vertices[4], vertices[6], vertices[0], vertices[2]);
        faces[5] = new Face(vertices[6], vertices[7], vertices[1], vertices[0]);
    }
}
