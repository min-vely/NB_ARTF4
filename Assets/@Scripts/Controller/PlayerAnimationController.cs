using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private PlayerController _playerController;
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        float moveMagnitude = _playerController.CurMovementInput.magnitude;
        // walk 애니메이션
        if (moveMagnitude > 0.01)
        {
            _animator.SetFloat(Speed, moveMagnitude);
        }
        // idle 애니메이션
        else
        {
            _animator.SetFloat(Speed, 0);
        }
    }
}
