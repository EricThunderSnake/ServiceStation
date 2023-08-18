using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CarController car;
    public GameObject needle;
    public Text kph;
    public Text gear;
    float startPosition = 200.762f, endPosition = 360f - 22.511f;
    float desiredPosition;

    public float vehicleSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        kph.text = car.KPH.ToString("0");
        UpdateNeedle();
    }

    public void UpdateNeedle()
    {
        desiredPosition = endPosition - startPosition;
        float temp = car.engineRPM * 1.6f/ 10000f;
        needle.transform.eulerAngles = new Vector3(0, 0, startPosition - temp * desiredPosition);
    }

    public void ChangeGear()
    {
        gear.text = (!car.reverse) ? (car.gearNum + 1).ToString() : "R";
    }
}
