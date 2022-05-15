using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum Variant
    {
        destructible, quest
    }
    public enum Status
    {
        open, closed
    }
    public Variant variant = Variant.destructible;
    public Status status = Status.closed;

    [SerializeField] int health = 1;
    [SerializeField] string openingSound = "";
    [SerializeField] string identifier = "";
    private Animator animator;
    private BoxCollider2D collider;
    

    private void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }


    public void TakeDamage(int damage)
    {
        if (variant == Variant.destructible &&
            status == Status.closed)
        {
            health -= damage;
            if (health <= 0)
            {
                AudioManager.Instance.PlaySound(openingSound);
                status = Status.open;
                animator.SetTrigger("open");
                collider.enabled = false;
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    public void OpenDoor()
    {
        status = Status.open;
        animator.SetTrigger("open");
        collider.enabled = false;
    }
}
