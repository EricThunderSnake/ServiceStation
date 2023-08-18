using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    internal enum DriveType
    {
        FWD,
        RWD,
        AWD
    }

    internal enum GearBox
    {
        Manual,
        Automatic
    }

    public GameManager GM;

    [SerializeField] DriveType drive;
    [SerializeField] GearBox gearbox;
    [Header("Variables")]
    public float KPH;
    public AnimationCurve enginePower;
    public float engineRPM;
    public float smoothTime = 0.01f;
    public float wheelsRPM;
    public float totalPower;
    public float minRPM, maxRPM;
    public float[] gears = new float[5];
    public int gearNum = 0;
    public float brakePower;
    InputManager IM;
    GameObject wheelMeshes, colliders;
    WheelCollider[] wheelCollider = new WheelCollider[4];
    [HideInInspector] public GameObject parkingDetection;
    GameObject[] wheelMesh = new GameObject[4];
    [SerializeField] GameObject centerOfMass;
    Rigidbody rigidbody;
    [SerializeField] float steeringRadius = 100;
    [SerializeField] float downForceVal = 50f;
    [SerializeField] float motorTorque = 200f;
    [SerializeField] float steeringMax = 4f;
    [SerializeField] float thrust = 200;
    public bool reverse = false;

    [Header("DEBUG")]
    public float[] slip = new float[4];
    
    // Start is called before the first frame update
    void Start()
    {
        GetObjects();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AddDownForce();
        AnimateWheels();
        
        
        GetFriction();
        CalculateEnginePower();
        
    }
    private void Update()
    {
        MoveVehicle();
        SteerVehicle();
        Shifter();
    }

    void Shifter()
    {
        if (!IsGrounded()) return;

        if (gearbox == GearBox.Automatic)
        {
            if (engineRPM > maxRPM && gearNum < gears.Length - 1)
            {
                gearNum++;
                GM.ChangeGear();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E) && gearNum < gears.Length - 1)
            {
                gearNum++;
                GM.ChangeGear();
            }
        }

        if (engineRPM < minRPM && gearNum > 0)
        {
            gearNum--;
            GM.ChangeGear();
        }

    }

    bool IsGrounded()
    {
        if (wheelCollider[0].isGrounded && wheelCollider[1].isGrounded &&
            wheelCollider[2].isGrounded && wheelCollider[3].isGrounded)
            return true;
        else return false;
    }

    void CalculateEnginePower()
    {
        WheelRPM();

        totalPower = enginePower.Evaluate(engineRPM) * 3.6f * gears[gearNum] * IM.vertical;
        float velocity = 0;
        engineRPM = Mathf.SmoothDamp(engineRPM, 1000 + Mathf.Abs(wheelsRPM) * gears[gearNum], ref velocity, smoothTime);
    }

    void WheelRPM()
    {
        float sum = 0;
        float R = 0;
        for (int i = 0; i < wheelCollider.Length; i++)
        {
            sum += wheelCollider[i].rpm;
            R++;
        }
        wheelsRPM = (R != 0) ? sum/R : 0;

        if(wheelsRPM < 0 && !reverse)
        {
            reverse = true;
            GM.ChangeGear();
        } else if (wheelsRPM > 0 && reverse)
        {
            reverse = false;
            GM.ChangeGear();
        }
    }

    void MoveVehicle(){        
        if(drive == DriveType.AWD){
            foreach (WheelCollider wheel in wheelCollider)
                wheel.motorTorque = totalPower/4;
        } else if (drive == DriveType.RWD) {
            for (int i = 2; i < wheelCollider.Length; i++)
                wheelCollider[i].motorTorque = totalPower/2;
        } else if (drive == DriveType.FWD){
            for (int i = 0; i < wheelCollider.Length - 2; i++)
                wheelCollider[i].motorTorque = totalPower / 2;
        }

        wheelCollider[2].brakeTorque = wheelCollider[3].brakeTorque = IM.handbrake ? brakePower : 0;

        if (IM.boost)
            rigidbody.AddForce(Vector3.forward * thrust);


        KPH = rigidbody.velocity.magnitude * 3.6f;
    }

    void SteerVehicle(){
        if (IM.horizontal != 0)
        {
            wheelCollider[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (steeringRadius - Mathf.Sign(IM.horizontal) * (1.5f / 2f))) * IM.horizontal;
            wheelCollider[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (steeringRadius + Mathf.Sign(IM.horizontal) * (1.5f / 2f))) * IM.horizontal;
        }
        else
        {
            wheelCollider[0].steerAngle = 0;
            wheelCollider[1].steerAngle = 0;
        }
    }

    void AnimateWheels() {
        for (int i = 0; i < 4; i++)
        {
            wheelCollider[i].GetWorldPose(out Vector3 wheelPosition, out Quaternion wheelRotation);
            wheelMesh[i].transform.SetPositionAndRotation(wheelPosition, wheelRotation);
        }
    }

    void AddDownForce()
    {
        rigidbody.AddForce(-downForceVal * rigidbody.velocity.magnitude * transform.up);
    }

    void GetFriction()
    {
        for (int i = 0; i < wheelCollider.Length; i++)
        {
            wheelCollider[i].GetGroundHit(out WheelHit wheelHit);
            slip[i] = wheelHit.forwardSlip;
        }
    }

    void GetObjects()
    {
        IM = GetComponent<InputManager>();
        rigidbody = GetComponent<Rigidbody>();
        colliders = GameObject.Find("Wheel Colliders");
        wheelMeshes = GameObject.Find("Wheel Meshes");
        parkingDetection = GameObject.Find("Parking Detection");
        wheelMesh[0] = wheelMeshes.transform.Find("0").gameObject;
        wheelMesh[1] = wheelMeshes.transform.Find("1").gameObject;
        wheelMesh[2] = wheelMeshes.transform.Find("2").gameObject;
        wheelMesh[3] = wheelMeshes.transform.Find("3").gameObject;
        wheelCollider[0] = colliders.transform.Find("0").gameObject.GetComponent<WheelCollider>();
        wheelCollider[1] = colliders.transform.Find("1").gameObject.GetComponent<WheelCollider>();
        wheelCollider[2] = colliders.transform.Find("2").gameObject.GetComponent<WheelCollider>();
        wheelCollider[3] = colliders.transform.Find("3").gameObject.GetComponent<WheelCollider>();

        centerOfMass = GameObject.Find("Mass");
        rigidbody.centerOfMass = centerOfMass.transform.localPosition;
    }
}
