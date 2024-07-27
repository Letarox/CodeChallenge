using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.CurrentActionState != ActionState.None)
            return;

        ApplyMovement();
    }

    private void ApplyMovement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        Vector2 direction = new(inputX, inputY);
        transform.Translate(_speed * Time.deltaTime * direction);
    }
}
