using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class FPS_Movement : MonoBehaviour
{
    public float M_Sensitivity = 1f;
    public Transform plaerBody;
    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        M_Sensitivity *= 100;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * M_Sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * M_Sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        plaerBody.Rotate(Vector3.up * mouseX);


    }
}
