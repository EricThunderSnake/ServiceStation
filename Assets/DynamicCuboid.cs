using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCuboid : Cuboid
{
    // Update is called once per frame
    void FixedUpdate()
    {
        SetFaces();
    }
}
