using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;

    private Animator _animator;
    private Vector2 _movePos;
    private Rigidbody2D _rb;

    void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
            Debug.LogError("Animator is NULL on Player.");

        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
            Debug.LogError("Rigidbody is NULL on Player.");
    }

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.CurrentActionState != ActionState.None)
        {
            _movePos.x = 0;
            _movePos.y = 0;
            return;
        }
           

        MovementInput();
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movePos * _speed * Time.fixedDeltaTime);
    }

    void MovementInput()
    {
        _movePos.x = Input.GetAxis("Horizontal");
        _movePos.y = Input.GetAxis("Vertical");

        _animator.SetFloat("Horizontal", _movePos.x);
        _animator.SetFloat("Vertical", _movePos.y);
        _animator.SetFloat("Speed", _movePos.sqrMagnitude);
    }
}
