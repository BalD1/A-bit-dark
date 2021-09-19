using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region variables

    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private GameObject lightObject;
    [SerializeField] private Rigidbody lightRB;
    [SerializeField] private float lightSpeed;
    [SerializeField] private GameObject baseLightPos;
    [SerializeField] private float maxLightDistance;
    private Vector3 lightDirection;

    [SerializeField] private LayerMask mask;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 center = new Vector3(0.5f, 0.5f, 0);


    #region camera variables

    [SerializeField] private float mouseSensitivity;
    private float mouseX;
    private float mouseY;
    private float xRotation = 0f;

    #endregion

    #region character movements variables

    [SerializeField] private float gravity;
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    private float xMovement;
    private float zMovement;
    private Vector3 move, velocity;
    private bool isGrounded;

    #endregion

    #endregion


    void Start()
    {

    }

    void Update()
    {

        if(Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if(Input.GetKeyDown(KeyCode.R))
            lightObject.transform.position = baseLightPos.transform.position;
    }

    private void FixedUpdate()
    {
        if(GameManager.Instance.gameState == GameManager.GameState.InGame)
        {
            CameraMovements();
            PlayerMovements();
            LightControl();
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
        lightRB.velocity = Vector3.zero;

        if(Input.GetKey(KeyCode.Mouse0) && Vector3.Distance(this.transform.position, lightObject.transform.position) < maxLightDistance)
        {
            ray = Camera.main.ViewportPointToRay(center);

            if(Physics.Raycast(ray, out hit, 10000, ~mask))
                MoveLightToPoint(hit.point);
        }

        if(Input.GetKey(KeyCode.Mouse1) && lightObject.transform.position != baseLightPos.transform.position)
        {
            MoveLightToPoint(baseLightPos.transform.position);
        }

    }

    private void MoveLightToPoint(Vector3 direction)
    {

        lightDirection = (direction - lightObject.transform.position).normalized * lightSpeed;
        lightRB.velocity = lightDirection;
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
