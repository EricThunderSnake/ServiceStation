using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingManager : MonoBehaviour
{
    public BoxCollider car;
    Renderer renderer;
    Color standardColor;
    Bounds spaceBounds;
    Bounds carBounds;
    float[] x = new float[2];
    float[] y = new float[2];
    float[] z = new float[2];
    float distance;
    bool spaceOccupied;
    Vector3 direction;
    GameObject[] tests = new GameObject[8];
    // Start is called before the first frame update

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        standardColor = renderer.material.color;
        spaceBounds = GetComponent<BoxCollider>().bounds;

        GameObject basic = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        for (int i = 0; i < 8; i++)
        {
            tests[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            tests[i].transform.localScale = 0.1f * Vector3.one;
            tests[i].GetComponent<SphereCollider>().isTrigger = true;
            tests[i].SetActive(false);
        }
        tests[0].transform.position = new Vector3(spaceBounds.max.x, spaceBounds.max.y, spaceBounds.max.z);
        tests[1].transform.position = new Vector3(spaceBounds.max.x, spaceBounds.max.y, spaceBounds.min.z);
        tests[2].transform.position = new Vector3(spaceBounds.max.x, spaceBounds.min.y, spaceBounds.max.z);
        tests[3].transform.position = new Vector3(spaceBounds.max.x, spaceBounds.min.y, spaceBounds.min.z);
        tests[4].transform.position = new Vector3(spaceBounds.min.x, spaceBounds.max.y, spaceBounds.max.z);
        tests[5].transform.position = new Vector3(spaceBounds.min.x, spaceBounds.max.y, spaceBounds.min.z);
        tests[6].transform.position = new Vector3(spaceBounds.min.x, spaceBounds.min.y, spaceBounds.max.z);
        tests[7].transform.position = new Vector3(spaceBounds.min.x, spaceBounds.min.y, spaceBounds.min.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        renderer.material.color = new Color(1,0,0,0.5f);
        spaceOccupied = true;
        foreach (GameObject test in tests)
            test.SetActive(true);
    }

    void OnTriggerStay(Collider other)
    {
        
        if (spaceOccupied)
        {
            carBounds = car.bounds;
            x[0] = Mathf.Max(spaceBounds.min.x, carBounds.min.x);
            x[1] = Mathf.Min(spaceBounds.max.x, carBounds.max.x);
            y[0] = Mathf.Max(spaceBounds.min.y, carBounds.min.y);
            y[1] = Mathf.Min(spaceBounds.max.y, carBounds.max.y);
            z[0] = Mathf.Max(spaceBounds.min.z, carBounds.min.z);
            z[1] = Mathf.Min(spaceBounds.max.z, carBounds.max.z);
            Debug.Log(new Vector3(x[0], y[0], z[0]));
            Debug.Log(new Vector3(x[1], y[1], z[1]));
            tests[0].transform.position = new Vector3(x[0], y[0], z[0]);
            tests[1].transform.position = new Vector3(x[0], y[0], z[1]);
            tests[2].transform.position = new Vector3(x[0], y[1], z[0]);
            tests[3].transform.position = new Vector3(x[0], y[1], z[1]);
            tests[4].transform.position = new Vector3(x[1], y[0], z[0]);
            tests[5].transform.position = new Vector3(x[1], y[0], z[1]);
            tests[6].transform.position = new Vector3(x[1], y[1], z[0]);
            tests[7].transform.position = new Vector3(x[1], y[1], z[1]);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        renderer.material.color = standardColor;
        spaceOccupied = false;
        foreach (GameObject test in tests)
            test.SetActive(false);
    }
}
