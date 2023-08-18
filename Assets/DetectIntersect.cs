using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectIntersect : MonoBehaviour
{
    BoxCollider collider;
    Cuboid box, otherBox;
    Vector3[] intersectPoints;
    bool[] vertexIsIntersecting;
    GameObject[] spheres = new GameObject[16];
    // Start is called before the first frame update
    void Start()
    {
        collider = gameObject.GetComponent<BoxCollider>();
        box = gameObject.GetComponent<Cuboid>();
        for (int i = 0; i < 16; i++)
        {
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spheres[i].transform.localScale = 0.1f * Vector3.one;
            spheres[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("test");
        otherBox = other.gameObject.GetComponent<Cuboid>();   
    }

    void OnTriggerStay()
    {
        ExcludeVertices();
    }

    void ExcludeVertices()
    {
        intersectPoints = otherBox.GetVertices();
        vertexIsIntersecting = new bool[intersectPoints.Length];
        for (int i = 0; i < intersectPoints.Length; i++)
        {
            vertexIsIntersecting[i] = false;
        }
        int count = 0;
        foreach (Vector3 point in intersectPoints)
        {
            if (IsInBox(point)){
                spheres[count].SetActive(true);
                spheres[count].transform.position = point;
                vertexIsIntersecting[count] = true;
            }
            else { spheres[count].SetActive(false); }

            count++;
            
        }

        count = 8;

        foreach(Cuboid.Face face in box.GetFaces())
        {
           
        }
    }

    int Mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }

    bool IsInBox(Vector3 point)
    {
        bool xCheck = Vector3.Dot(box.Front().normal, point) - box.Front().displacement > 0 && Vector3.Dot(box.Back().normal, point) - box.Back().displacement > 0;
        bool yCheck = Vector3.Dot(box.Top().normal, point) - box.Top().displacement > 0 && Vector3.Dot(box.Bottom().normal, point) - box.Bottom().displacement > 0;
        bool zCheck = Vector3.Dot(box.Right().normal, point) - box.Right().displacement > 0 && Vector3.Dot(box.Left().normal, point) - box.Left().displacement > 0;
        return xCheck && yCheck && zCheck;
    }
}
