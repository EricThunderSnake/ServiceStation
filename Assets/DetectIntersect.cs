using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectIntersect : MonoBehaviour
{
    [SerializeField] Cuboid box;
    [SerializeField] DynamicCuboid otherBox;
    Vector3[] intersectPoints;
    bool[] vertexIsIntersecting;
    [SerializeField] bool playerParked;
    GameObject[] spheres = new GameObject[8];
    // Start is called before the first frame update
    void Start()
    {
        box = gameObject.GetComponent<Cuboid>();
        for (int i = 0; i < 8; i++)
        {
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spheres[i].transform.localScale = 0.1f * Vector3.one;
            spheres[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ParkCheck())
            Debug.Log("parked");
    }

    void OnTriggerStay()
    {
        ExcludeVertices();
        ParkCheck();
    }

    void OnTriggerExit()
    {
        for(int i = 0; i < spheres.Length; i++)
            spheres[i].SetActive(false);
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
            if (IsInBox(point))
            {
                spheres[count].SetActive(true);
                spheres[count].transform.position = point;
                vertexIsIntersecting[count] = true;
            } else 
            { 
                spheres[count].SetActive(false);
            }

            count++;
            
        }
    }

    bool ParkCheck()
    {
        bool check = true;
        foreach (GameObject ball in spheres)
            check = check && ball.activeSelf;
        return check;
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
