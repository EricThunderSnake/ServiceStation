using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float vertical;
    public float horizontal;
    public bool handbrake;
    public bool boost;

    void FixedUpdate()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        handbrake = (Input.GetAxis("Jump") != 0) ? true : false;
        boost = Input.GetKey(KeyCode.LeftShift) ? true : false;
    }
}
