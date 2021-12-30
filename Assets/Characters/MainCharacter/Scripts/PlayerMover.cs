using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), (typeof(Animator)))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] [Range(0,10)] private float _speed;

    private CharacterController _characterController;
    private Animator _animator;
    
    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;

    private bool _isMovementPressed;

    private float _rotation = 10f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Movement();
        HandleAnimations();
        HandleRotation();
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        _currentMovementInput = value.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }
    
    private void Movement()
    {
        _characterController.Move(_currentMovement * _speed * Time.deltaTime);
    }

    private void HandleAnimations()
    {
        var isRunning = _animator.GetBool("isRunning");

        switch (_isMovementPressed)
        {
            case true when !isRunning:
                _animator.SetBool("isRunning", true);
                break;
            case false when isRunning:
                _animator.SetBool("isRunning", false);
                break;
        }
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = _currentMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = _currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (_isMovementPressed)
        {
            var targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotation * Time.deltaTime);
        }
    }
}
