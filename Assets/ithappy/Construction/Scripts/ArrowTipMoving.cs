using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArrowTipMoving : MonoBehaviour
{
    float Y = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Y += 1.0f;
        this.transform.eulerAngles = new Vector3(0, Y, 0);
        this.transform.position = new Vector3(this.transform.position.x, (float)(4.0f + 0.5 * Math.Sin(0.1 * Y)), this.transform.position.z);
    }
}
