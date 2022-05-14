using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] int maxHealth = 5;
    [SerializeField] int currentHealth = 5;

    Animator animator;
    Vector2 moveInput;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }

    void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
        animator.SetFloat("XInput", moveInput.x);
        animator.SetFloat("YInput", moveInput.y);
        animator.SetBool("IsMoving", moveInput != Vector2.zero);
    }

    void Move()
    {
        var delta = moveInput * moveSpeed * Time.deltaTime;
        transform.position = new Vector2(transform.position.x + delta.x, transform.position.y + delta.y);
    }
}
