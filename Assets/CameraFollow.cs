﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 normalPos = target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, normalPos, smoothSpeed);
        transform.position = smoothPos;
    }
}
