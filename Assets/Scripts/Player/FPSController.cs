using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://sharpcoderblog.com/blog/unity-3d-fps-controller

public class FPSController : NetworkBehaviour
{
    [SerializeField] private float m_WalkingSpeed = 7.5f;
    [SerializeField] private float m_RunningSpeed = 11.5f;
    [SerializeField] private float m_JumpSpeed = 8.0f;
    [SerializeField] private float m_Gravity = 20.0f;
    [SerializeField] private float m_LookSpeed = 2.0f;
    [SerializeField] private float m_LookXLimit = 45.0f;

    private CharacterController m_CharacterController;
    private Camera m_PlayerCamera;
    private Vector3 m_MoveDirection = Vector3.zero;
    private float m_RotationX = 0;

    private bool m_CanMove = true;

    [SerializeField] private GameObject m_PlayerCanvas;


    // Start is called before the first frame update
    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();

        var canvasManager = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<PlayersCanvasManager>();
        canvasManager.AddPlayer(gameObject);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void OnStartAuthority()
    {
        m_PlayerCamera = GetComponentInChildren<Camera>(true);
        m_PlayerCamera.enabled = true;

        m_PlayerCanvas.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if(!hasAuthority) { return; }

        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = m_CanMove ? (isRunning ? m_RunningSpeed : m_WalkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = m_CanMove ? (isRunning ? m_RunningSpeed : m_WalkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = m_MoveDirection.y;
        m_MoveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && m_CanMove && m_CharacterController.isGrounded)
        {
            m_MoveDirection.y = m_JumpSpeed;
        }
        else
        {
            m_MoveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!m_CharacterController.isGrounded)
        {
            m_MoveDirection.y -= m_Gravity * Time.deltaTime;
        }

        // Move the controller
        m_CharacterController.Move(m_MoveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (m_CanMove)
        {
            m_RotationX += -Input.GetAxis("Mouse Y") * m_LookSpeed;
            m_RotationX = Mathf.Clamp(m_RotationX, -m_LookXLimit, m_LookXLimit);
            m_PlayerCamera.transform.localRotation = Quaternion.Euler(m_RotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * m_LookSpeed, 0);
        }
    }
}
