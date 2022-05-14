using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static Player.FiringDirection;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] int maxHealth = 5;
    [SerializeField] float invincibilitySeconds = 5f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float timeToReload = 0.25f;

    public Direction currentAimingDirection;
    public List<FiringDirection> firingDirections;
    List<Collectible> collectibles = new List<Collectible>();

    [Serializable]
    public class FiringDirection
    {
        public enum Direction
        {
            up, up_left, up_right, left, right, down_left, down_right, down
        }

        public Direction direction;
        public GameObject spawningPosition;
        public Vector3 orientation;
    }

    Animator animator;
    Rigidbody2D rigidBody;
    Vector2 moveInput;
    int currentHealth = 5;
    float timeInvincible = 0.0f;
    float timeReloading = 0.0f;
    SpriteRenderer sprite;
    

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Move();

        if(timeInvincible > 0)
        {
            timeInvincible += Time.deltaTime;
            if(timeInvincible > invincibilitySeconds)
            {
                sprite.color = new Color(1f,1f,1f, 1f);
                timeInvincible = 0.0f;
            }
        }
    }

    void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
        animator.SetFloat("XInput", moveInput.x);
        animator.SetFloat("YInput", moveInput.y);
        animator.SetBool("IsMoving", moveInput != Vector2.zero);
    }

    void OnFire(InputValue inputValue)
    {
        Vector3 spawningPosition;
        Quaternion spawningRotation;
        if(timeReloading == 0.0f)
        {
            if (projectilePrefab)
            {
                GameObject newProjectile = Instantiate(projectilePrefab, spawningPosition, Quaternion.LookRotation(new Vector3(1, 0, 0)), GameManager.Instance.playerProjectileContainer); 
            }
        }
        timeReloading += Time.deltaTime;

        if(timeReloading > timeToReload)
        {
            timeReloading = 0.0f;
        }
        
    }

    void Move()
    {
        var delta = moveInput * moveSpeed;
        rigidBody.velocity = delta;
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        timeInvincible += Time.deltaTime;
        sprite.color = new Color(0.5f,0.5f,0.5f, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyDamage enemyDamage;
        if((collision.gameObject.layer << GameManager.Instance.enemyDamageLayerMask) > 0)
        {
            enemyDamage = collision.gameObject.GetComponent<EnemyDamage>();
            if (enemyDamage)
            {
                takeDamage(enemyDamage.damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyDamage enemyDamage;
        if((collision.gameObject.layer << GameManager.Instance.enemyDamageLayerMask) > 0)
        {
            enemyDamage = collision.gameObject.GetComponent<EnemyDamage>();
            if (enemyDamage)
            {
                takeDamage(enemyDamage.damage);
            }
        }

        if (collision.tag == "Collectible")
        {
            collectibles.Add(collision.gameObject.GetComponent<Collectible>());
            Destroy(collision.gameObject);
            Debug.Log(string.Join(',', collectibles.Select(x => x.collectibleType)));
        }
    }
}
