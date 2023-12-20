using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayerMask;

    [Header("Look")]
    [SerializeField] private Transform cameraContainer;
    private const float MinXLook = -85f;
    private const float MaxXLook = 85f;
    private float _camCurXRot;
    private const float LookSensitivity = 0.2f;

    [Header("Throw")]
    [SerializeField] private GameObject throwablePrefab;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float throwCooldown = 1f;
    private float _lastThrowTime;

    private bool _canMove = true;
    private bool _isStunned;
    private bool _wasStunned;
    private bool _slider = false;
    private float _pushForce;
    [SerializeField] private float gravity = 9.8f;
    private Vector3 _pushDir;
    public Vector3 checkPoint = new(0f, 4f, 0f);
    private Vector2 _mouseDelta;
    private Coroutine _buffCoroutine;

    private Rigidbody _rigidBody;
    private Animator _animator;
    private static readonly int Throw = Animator.StringToHash("Throw");

    public Vector2 CurMovementInput { get; set; }

    private bool _isSpeedBuffActive = false;
    private bool _isJumpBuffActive = false;


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        if (_canMove) Move();
        // 넉백
        else
        {
            _rigidBody.velocity = _pushDir * _pushForce;
            _rigidBody.AddForce(new Vector3(0, -gravity * _rigidBody.mass, 0));
        }
    }

    private void LateUpdate()
    {
        if (Main.Game.CanLook) CameraLook();
    }

    private void Move()
    {
        Vector3 dir = transform.forward * CurMovementInput.y + transform.right * CurMovementInput.x;

        dir *= moveSpeed;
        dir.y = _rigidBody.velocity.y;
        _rigidBody.velocity = dir;
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        CurMovementInput = context.phase switch 
        {
            InputActionPhase.Performed => context.ReadValue<Vector2>(),
            InputActionPhase.Canceled => Vector2.zero,
            _ => CurMovementInput
        };
    }

    private void CameraLook()
    {
        _camCurXRot += _mouseDelta.y * LookSensitivity;
        _camCurXRot = Mathf.Clamp(_camCurXRot, MinXLook, MaxXLook);
        cameraContainer.localEulerAngles = new Vector3(-_camCurXRot, 0, 0);
        transform.eulerAngles += new Vector3(0, _mouseDelta.x * LookSensitivity, 0);
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        _mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started) return;
        if (IsGrounded())_rigidBody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
    }

    public void OnThrowInput(InputAction.CallbackContext context)
    {
        // 쿨타임 체크
        if (!(Time.time - _lastThrowTime >= throwCooldown)) return;
        // 쿨타임 초기화
        _lastThrowTime = Time.time;
        _animator.SetTrigger(Throw);
        // 플레이어 기준 공 발사 위치
        var transform1 = transform;
        Vector3 throwPosition = transform1.position + transform1.forward * 0.8f + transform1.right * 0.7f + Vector3.up * 1.2f;
        // 오브젝트 풀링 사용하면 더 좋음
        GameObject throwableInstance = Instantiate(throwablePrefab, throwPosition, Quaternion.identity);
        Rigidbody throwRigidBody = throwableInstance.GetComponent<Rigidbody>();

        throwRigidBody.AddForce(transform.forward * throwForce, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        var transform1 = transform;
        var forward = transform1.forward;
        var position = transform1.position;
        var right = transform1.right;
        
        Ray[] rays = 
        {
            new(position + forward * 0.2f + (Vector3.up * 0.1f) , Vector3.down),
            new(position + -forward * 0.2f+ (Vector3.up * 0.1f), Vector3.down),
            new(position + right * 0.2f + (Vector3.up * 0.1f), Vector3.down),
            new(position + -right * 0.2f + (Vector3.up * 0.1f), Vector3.down),
        };

        foreach (var ray in rays)
        {
            if (!Physics.Raycast(ray, out var hit, 0.2f, groundLayerMask)) continue;
            return true;
        }
        return false;
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        Main.Game.CanLook = !toggle;
    }

    public void LoadCheckPoint()
    {
        transform.position = checkPoint;
    }

    public void HitPlayer(Vector3 velocityF, float time)
    {
        _rigidBody.velocity = velocityF;
        _pushForce = velocityF.magnitude;
        _pushDir = Vector3.Normalize(velocityF);
        StartCoroutine(Decrease(velocityF.magnitude, time));
    }

    private IEnumerator Decrease(float value, float duration)
    {
        if (_isStunned)
        {
            _wasStunned = true;
            _isStunned = true;
            _canMove = false;
        }
        var delta = value / duration;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            yield return null;
            if (!_slider) // 땅이 slide가 아니면 감소된 pushForce 적용
            {
                _pushForce -= Time.deltaTime * delta;
                _pushForce = _pushForce < 0 ? 0 : _pushForce;
            }
            _rigidBody.AddForce(new Vector3(0, -gravity * _rigidBody.mass, 0));
        }

        if (_wasStunned)_wasStunned = false;
        else
        {
            _isStunned = false;
            _canMove = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Item")) return;
        Item item = other.GetComponent<Item>();
        float itemPower = item.Power;
        float itemDuration = item.Duration;
        _buffCoroutine = item.Id switch 
        {
            "0"  => StartCoroutine(SpeedUp(itemPower, itemDuration)),
            "1"  => StartCoroutine(JumpUp(itemPower, itemDuration)),
            _ => _buffCoroutine
        };
        Destroy(item.gameObject);
    }

    private IEnumerator SpeedUp(float power, float duration)
    {
        if (_isSpeedBuffActive) yield break;
        _isSpeedBuffActive = true;
        float defaultMoveSpeed = moveSpeed;
        moveSpeed *= power;
        yield return new WaitForSeconds(duration);
        moveSpeed = defaultMoveSpeed;
        _isSpeedBuffActive = false;
    }

    private IEnumerator JumpUp(float power, float duration)
    {
        if (_isJumpBuffActive) yield break;
        _isJumpBuffActive = true;
        float defaultJumpForce = jumpForce;
        jumpForce *= power;
        yield return new WaitForSeconds(duration); 
        jumpForce = defaultJumpForce;
        _isJumpBuffActive = false;
    }
}