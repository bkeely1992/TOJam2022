using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    Vector2 moveInput;

    void Update()
    {
        Move();
    }

    void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
        Debug.Log(inputValue);
    }

    void Move()
    {
        var delta = moveInput * moveSpeed * Time.deltaTime;
        transform.position = new Vector2(transform.position.x + delta.x, transform.position.y + delta.y);
    }
}
