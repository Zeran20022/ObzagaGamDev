using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Header("Movement")]
    public float s_speed = 0.0f;
    public float s_jumpLeght = 0.0f;
    public float s_gravity = 0.0f;
    private Vector3 p_direction = Vector3.zero;

    private CharacterController p_char = null;

    [Header("MouseMovement")]
    public float s_sensitivity = 0.0f;   

    private float p_angelX = 0.0f;
    private float p_angleY = 0.0f;

    public Transform s_camera = null;

    private Quaternion p_camQ;
    private Quaternion p_charQ;


    void Start()
    {
        s_camera = Camera.main.transform;
        p_char = GetComponent<CharacterController>();

        p_camQ = s_camera.rotation;
        p_charQ = transform.rotation;
    }

    void FixedUpdate()
    {
        p_angelX += Input.GetAxis("Mouse X") * s_sensitivity; 
        p_angleY += Input.GetAxis("Mouse Y") *s_sensitivity;
        p_angleY = Mathf.Clamp(p_angleY, -90f, 90f);

        s_camera.rotation = p_camQ * Quaternion.AngleAxis(p_angelX, Vector3.up) * Quaternion.AngleAxis(p_angleY, Vector3.left);
        transform.rotation = p_charQ = Quaternion.AngleAxis(p_angelX, Vector3.up);
    }

    void Update()
    {
        if(p_char.isGrounded)
        {
            if(Input.GetButton("Vertical"))
            {
                p_direction += Input.GetAxis("Vertical") * transform.forward;
            }
            
            if(Input.GetButton("Horizontal"))
            {
                p_direction += Input.GetAxis("Horizontal") *transform.right;
            }

            if(Input.GetButton("Jump"))
            {
                p_direction.y = s_jumpLeght;
            }
            p_direction *= s_speed * Time.deltaTime;
            p_char.Move(p_direction);
        }
        else
        {
            p_direction.y -=s_gravity * Time.deltaTime;
            p_char.Move(p_direction);
        }
    }
}
