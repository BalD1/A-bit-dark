using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region variables

    [Header("Player")]
    [SerializeField] private GameObject player;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform groundCheck;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private CharacterController controller;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private float groundDistance;
    [SerializeField] private float gravity;
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;

    private float xMovement, zMovement;
    private Vector3 move, velocity;
    private bool isGrounded;


    [Header("Light")]
    [SerializeField] private GameObject lightObject;

    [SerializeField] private Rigidbody lightRB;

    [SerializeField] private Renderer lightRenderer;    

    [SerializeField] private float lightSpeed;
    [SerializeField] private float maxLightDistance;
    [SerializeField] private float baseDistanceToCam;
    [SerializeField] private float toCenterSpeed;

    private Vector3 lightMovement;


    [Header("Camera")]

    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float lightSensitivity;
    private float mouseX, mouseY, mouseZ, xRotation;

    private Vector3 center, screenCenter;

    #endregion


    void Start()
    {
        center = new Vector3(0.5f, 0.5f, baseDistanceToCam);
        screenCenter = playerCamera.ViewportToWorldPoint(center);
        lightObject.transform.position = playerCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, baseDistanceToCam));
    }

    void Update()
    {
        lightObject.transform.eulerAngles = playerCamera.transform.eulerAngles;
        Debug.Log(lightRenderer.isVisible);

        if(Input.GetMouseButton(0) && lightRenderer.isVisible)
        {
            LightControl();
        }
        else
        {
            CameraMovements();
            if(Input.GetMouseButton(1))
            {
                LightToCenter(toCenterSpeed);
            }
        }

        if (!lightRenderer.isVisible)
        {
            LightToCenter(toCenterSpeed * 2);
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if(Input.GetKeyDown(KeyCode.R))
            lightObject.transform.position = playerCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, baseDistanceToCam));
    }

    private void FixedUpdate()
    {
        if(GameManager.Instance.gameState == GameManager.GameState.InGame)
        {
            PlayerMovements();
        }
    }

    #region camera + movements

    private void CameraMovements()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerTransform.Rotate(Vector3.up * mouseX);
    }

    private void PlayerMovements()
    {

        xMovement = Input.GetAxis("Horizontal");
        zMovement = Input.GetAxis("Vertical");

        move = transform.right * xMovement + transform.forward * zMovement;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    #endregion

    private void LightControl()
    {
        mouseX = Input.GetAxis("Mouse X") * lightSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * lightSensitivity * Time.deltaTime;
        mouseZ = Input.mouseScrollDelta.y * mouseSensitivity * Time.deltaTime;

        lightMovement.x = mouseX;
        lightMovement.y = mouseY;
        lightMovement.z = mouseZ;

        lightObject.transform.Translate(lightMovement, relativeTo: Space.Self);
    }

    private void LightToCenter(float speed)
    {
        screenCenter = playerCamera.ViewportToWorldPoint(center);
        lightObject.transform.position = Vector3.MoveTowards(lightObject.transform.position, screenCenter, speed * Time.deltaTime);
    }

    public void Death()
    {
        controller.enabled = false;
        this.transform.position = GameManager.Instance.respawnTransform.pos;
        this.transform.eulerAngles = GameManager.Instance.respawnTransform.rot;
        controller.enabled = true;

        xRotation = 0;
        playerCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
