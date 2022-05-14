using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] int maxHealth = 5;
    [SerializeField] int currentHealth = 5;

    List<Collectible> collectibles = new List<Collectible>();

    Animator animator;
    Rigidbody2D rigidBody;
    Vector2 moveInput;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
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
        var delta = moveInput * moveSpeed;
        rigidBody.velocity = delta;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectible")
        {
            collectibles.Add(collision.gameObject.GetComponent<Collectible>());
            Destroy(collision.gameObject);
            Debug.Log(string.Join(',', collectibles.Select(x => x.collectibleType)));
        }
    }
}
