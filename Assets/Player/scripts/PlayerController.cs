using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;

    public float gravity = -9.8f;

    Vector3 velocity;

    #region Camera and Movement

    public Camera playerCamera;

    public float fov = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;

    public bool lockCursor = true;
    public bool crosshair = false;
    public Sprite crosshairImage;
    public Color crosshairColor = Color.white;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Image crosshairObject;

    #region Camera Zoom Variables

    public bool enableZoom = true;
    public bool holdToZoom = false;
    public KeyCode zoomKey = KeyCode.Mouse1;
    public float zoomFOV = 30f;
    public float zoomStepTime = 5f;

    private bool isZoomed = false;

    #endregion
    #endregion

    #region Movement Variables

    public bool playerCanMove = true;
    public float walkSpeed = 2.5f;
    public float maxVelocityChange = 10f;

    private bool isWalking = false;

    #region Sprint

    public bool enableSprint = true;
    public bool unlimitedSprint = false;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public float sprintSpeed = 5f;
    public float sprintDuration = 5f;
    public float sprintCooldown = .5f;
    public float sprintFOV = 80f;
    public float sprintFOVStepTime = 10f;

    public bool useSprintBar = true;
    public bool hideBarWhenFull = true;
    public Image sprintBarBG;
    public Image sprintBar;
    public float sprintBarWidthPercent = .3f;
    public float sprintBarHeightPercent = .015f;

    private CanvasGroup sprintBarCG;
    public bool isSprinting = false;
    private float sprintRemaining;
    private float sprintBarWidth;
    private float sprintBarHeight;
    private bool isSprintCooldown = false;
    private float sprintCooldownReset;


    #endregion
    #endregion

    #region Crouch

    public bool enableCrouch = true;
    public bool holdToCrouch = true;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public float crouchHeight = .75f;
    public float speedReduction = 0.25f;
    
    public bool isCrouched = false;
    private Vector3 originalScale;
   
    #endregion

    bool isGrounded;
    public float groundDistance = 0.1f;

    #region Jump

    public bool enableJump = true;
    public float jumpHeight = 1f;
    public KeyCode jumpKey = KeyCode.Space;

    #endregion

    #region Head Bob

    public bool enableHeadBob = true;
    public Transform headBob;
    public float bobspeed = 10f;
    public Vector3 bobAmount = new Vector3(0f, .05f, .05f);

    private Vector3 headOriginalPos;
    private float timer = 0;

    #endregion

    private Animator _anim;

    private void Awake()
    {
        crosshairObject = GetComponentInChildren<Image>();
        
        playerCamera.fieldOfView = fov;
        originalScale = transform.localScale;
        headOriginalPos = headBob.localPosition;

        if(!unlimitedSprint)
        {
            sprintRemaining = sprintDuration;
            sprintCooldownReset = sprintCooldown;
        }
    }

    void Start()
    {
        if(lockCursor)
        {
            Cursor.lockState= CursorLockMode.Locked;
        }

        if(crosshair)
        {
            crosshairObject.sprite = crosshairImage;
            crosshairObject.color = crosshairColor;
        }
        else
        {
            crosshairObject.gameObject.SetActive(false);
        }

        #region Sprint Bar

        sprintBarCG = GetComponentInChildren<CanvasGroup>();

        if(useSprintBar)
        {
            sprintBarBG.gameObject.SetActive(true);
            sprintBar.gameObject.SetActive(true);

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            sprintBarWidth = screenWidth * sprintBarWidthPercent;
            sprintBarHeight= screenHeight * sprintBarHeightPercent;

            sprintBarBG.rectTransform.sizeDelta = new Vector3(sprintBarWidth, sprintBarHeight, 0f);
            sprintBar.rectTransform.sizeDelta = new Vector3(sprintBarWidth - 2, sprintBarHeight - 2, 0f);
        
            if(hideBarWhenFull)
            {
                sprintBarCG.alpha= 0;
            }
        }
        else
        {
            sprintBarBG.gameObject.SetActive(false);
            sprintBar.gameObject.SetActive(false);
        }

        #endregion
    }

    void Update()
    {
        #region Camera

        if(cameraCanMove)
        {
            yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;

            if (!invertCamera)
            {
                pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
            }
            else
            {
                // Inverted Y
                pitch += mouseSensitivity * Input.GetAxis("Mouse Y");
            }

            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        }

        #region Camera Zoom

        if(enableZoom)
        {


            if(Input.GetKeyDown(zoomKey) && !holdToZoom && !isSprinting)
            {
                if(!isZoomed)
                {
                    isZoomed= true;
                }
                else
                {
                    isZoomed= false;
                }
            }



            if(holdToZoom && !isSprinting)
            {
                if (Input.GetKeyDown(zoomKey))
                {
                    isZoomed = true;
                }
                else if (Input.GetKeyUp(zoomKey))
                {
                    isZoomed = false;
                }
            }


            if(isZoomed)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomFOV, zoomStepTime * Time.deltaTime);
            }
            else if(!isZoomed && !isSprinting)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, zoomStepTime * Time.deltaTime);
            }
        }

        #endregion
        #endregion

        

        #region Sprint
        
        if (enableSprint)
        {
            if (isSprinting)
            {
                isZoomed = false;
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, sprintFOV, sprintFOVStepTime * Time.deltaTime);
            

                if(!unlimitedSprint) 
                { 
                    sprintRemaining -= 1 * Time.deltaTime;
                    if(sprintRemaining <= 0)
                    {
                        isSprinting= false;
                        isSprintCooldown = true;
                    }
                }
            }
            else
            {
                sprintRemaining = Mathf.Clamp(sprintRemaining += 1 * Time.deltaTime, 0, sprintDuration);
            }



            if(isSprintCooldown)
            {
                sprintCooldown -= 1 * Time.deltaTime;
                if(sprintCooldown <= 0)
                {
                    isSprintCooldown= false;
                }
            }
            else
            {
                sprintCooldown = sprintCooldownReset;
            }


            if(useSprintBar && !unlimitedSprint)
            {
                float sprintRemainingPercent = sprintRemaining / sprintDuration;
                sprintBar.transform.localScale = new Vector3(sprintRemainingPercent, 1f, 1f);
            }
        }

        #endregion

        #region Jump

        if(enableJump && Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        #endregion

        #region Crouch

        if (enableCrouch)
        {
            if(Input.GetKeyDown(crouchKey) && !holdToCrouch)
            {
                Crouch();
            }

            if(Input.GetKeyDown(crouchKey) && holdToCrouch)
            {
                isCrouched = false;
                Crouch();
            }
            else if (Input.GetKeyUp(crouchKey) && holdToCrouch)
            {
                isCrouched = true;
                Crouch();
            }
        }

        #endregion

        if (enableHeadBob)
        {
            HeadBob();
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 move = transform.right* x + transform.forward * y;


        if((targetVelocity.x != 0 || targetVelocity.z != 0) && isGrounded)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        if (enableSprint && Input.GetKey(sprintKey) && sprintRemaining > 0f && !isSprintCooldown) 
        {
            targetVelocity = transform.TransformDirection(targetVelocity) * sprintSpeed;
            
            //Проверяем изменилась ли скорость
            if(targetVelocity !=Vector3.zero)
            {
                isSprinting = true;

                sprintRemaining -= Time.deltaTime;

                if(isCrouched)
                {
                    Crouch() ;
                }

                if(hideBarWhenFull && !unlimitedSprint)
                {
                    sprintBarCG.alpha += 5 * Time.deltaTime;
                }
            }
            else
            {
                isSprinting = false;
            }
        }
        else
        {
            isSprinting = false;
        }

        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        move *= currentSpeed;

        controller.Move(move  * Time.fixedDeltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    private void Crouch()
    {


        if(isCrouched)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            walkSpeed /= speedReduction;

            isCrouched= false;
        }
        else
        {
            transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
            walkSpeed *= speedReduction;

            isCrouched = true;
        }
    }

    private void Jump ()
    {
        if(isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if(isCrouched && !holdToCrouch)
        {
            Crouch();
        }
    }
    
    private void HeadBob()
    {
        if(isWalking)
        {
            if(isSprinting)
            {

            }
            else if (isCrouched)
            {

            }
            else
            {
                timer += Time.deltaTime * bobspeed;
            }

            headBob.localPosition = new Vector3(headOriginalPos.x + Mathf.Sin(timer) * bobAmount.x, headOriginalPos.y + Mathf.Sin(timer) * bobAmount.y, headOriginalPos.z + Mathf.Sin(timer) * bobAmount.z);
        }
        else
        {
            timer = 0;
            headBob.localPosition = new Vector3(Mathf.Lerp(headBob.localPosition.x, headOriginalPos.x, Time.deltaTime * bobspeed), Mathf.Lerp(headBob.localPosition.y,headOriginalPos.y, Time.deltaTime * bobspeed),Mathf.Lerp(headBob.localPosition.z, headOriginalPos.z, Time.deltaTime * bobspeed));
        }
    }

}
