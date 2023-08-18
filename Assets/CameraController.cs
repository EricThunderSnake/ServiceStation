using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject Player;
    CarController car;
    [SerializeField] GameObject cameraFollow;
    [SerializeField] float speed;
    public float defaultFOV, desiredFOV;
    [Range(0, 5)] public float smoothTime;

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        cameraFollow = Player.transform.Find("CameraConstraint").gameObject;
        car = Player.GetComponent<CarController>();
        defaultFOV = Camera.main.fieldOfView;
    }

    void FixedUpdate()
    {
        Follow();
        BoostFOV();        
    }

    void Follow()
    {
        speed = Mathf.Lerp(speed, car.KPH, Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, cameraFollow.transform.position, Time.deltaTime*speed);
        transform.LookAt(Player.transform.position);
    }

    void BoostFOV()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, desiredFOV, Time.deltaTime * smoothTime);
        else
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, defaultFOV, Time.deltaTime * smoothTime);
    }
}
