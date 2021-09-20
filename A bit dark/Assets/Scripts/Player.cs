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
    [SerializeField] private LightOrb lightOrb;
    [SerializeField] private GameObject lightAttachPoint;
    public GameObject LightAttachPoint { get => lightAttachPoint; }


    [Header("Camera")]
    [SerializeField] private float mouseSensitivity;
    private float mouseX, mouseY, xRotation;

    #endregion


    void Start()
    {

    }

    void Update()
    {
        if(!(Input.GetMouseButton(0) && lightOrb.MovableSphereRenderer.isVisible))
        {
            CameraMovements();
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
            velocity.y = -2f;
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
