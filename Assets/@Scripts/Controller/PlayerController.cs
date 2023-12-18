using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed = 5f;
    private Vector2 _curMovementInput;
    private float jumpForce = 500f;
    [SerializeField] private LayerMask groundLayerMask;

    [Header("Look")]
    [SerializeField] private Transform cameraContainer;
    private float minXLook = -85f;
    private float maxXLook = 85f;
    private float camCurXRot;
    private float lookSensitivity = 0.2f;

    [Header("Throw")]
    [SerializeField] private GameObject throwablePrefab;
    private float throwForce = 10f;
    private float throwCooldown = 1f;
    private float lastThrowTime;

    public Vector3 checkPoint = new Vector3(0f, 4f, 0f);

    private Vector2 mouseDelta;

    [HideInInspector]
    [SerializeField] private bool canLook = true;

    private Rigidbody _rigidbody;

    public static PlayerController instance;

    public Vector2 CurMovementInput
    {
        get { return _curMovementInput; }
        set { _curMovementInput = value; }
    }

    private void Awake()
    {
        instance = this;
        _rigidbody = GetComponent<Rigidbody>();
        checkPoint = transform.position;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * _curMovementInput.y + transform.right * _curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    private void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (IsGrounded())
                _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);

        }
    }

    public void OnThrowInput(InputAction.CallbackContext context)
    {
        // 쿨타임 체크
        if (Time.time - lastThrowTime >= throwCooldown)
        {
            // 쿨타임 초기화
            lastThrowTime = Time.time;

            GameObject throwableInstance = Instantiate(throwablePrefab, transform.position + transform.forward, Quaternion.identity);
            Rigidbody throwRigidbody = throwableInstance.GetComponent<Rigidbody>();

            throwRigidbody.AddForce(transform.forward * throwForce, ForceMode.Impulse);
            // 프리팹이 뭔가에 충돌하면 5초 후에 destory
        }
    }

    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (Vector3.up * 0.01f) , Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f)+ (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + (transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (transform.right * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.right * 0.2f), Vector3.down);
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    public void LoadCheckPoint()
    {
        transform.position = checkPoint;
    }
}