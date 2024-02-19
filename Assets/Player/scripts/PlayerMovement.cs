using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float M_speed = 0.0f;
    public float M_default_speed = 2.5f;
    public float M_sprint_speed = 5f;
    public float M_crouc_speed = 1.25f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    Vector3 velocity;
    public float groundDistance = 0.1f;

    bool isGrounded;
    bool isCrouch = false;
    bool isSprint = false;

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        
        
        
        if (isGrounded && velocity.y < 0 ) 
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        
        }

        

        if (isCrouch)
        {
            M_speed = M_crouc_speed; 
        }
        else if (isSprint)
        {
            M_speed = M_sprint_speed;
        }
        else
        {
            M_speed = M_default_speed;
        }


        Vector3 move = transform.right * x + transform.forward * y;
        controller.Move(move * M_speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        }

        if (Input.GetKey("left ctrl"))
        {
            controller.height = 0.75f;
            isCrouch = true; 
        }
        else 
        { 
            controller.height = 1.75f;
            isCrouch = false;
        }

        if (Input.GetKey("left shift"))
        {
            isSprint = true;
        }
        else
        {
            isSprint = false;
        }

    }
}
