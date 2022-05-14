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
    [SerializeField] Animator animator;

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
        public float rotation;
    }

    Rigidbody2D rigidBody;
    Vector2 moveInput;
    int currentHealth = 5;
    float timeInvincible = 0.0f;
    float timeReloading = 0.0f;
    SpriteRenderer sprite;
    Dictionary<FiringDirection.Direction, FiringDirection> firingDirectionMap = new Dictionary<Direction, FiringDirection>();
    bool isDead = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        foreach (FiringDirection firingDirection in firingDirections)
        {
            firingDirectionMap.Add(firingDirection.direction, firingDirection);
        }
    }

    void Update()
    {
        Move();

        if (timeInvincible > 0)
        {
            timeInvincible += Time.deltaTime;
            if (timeInvincible > invincibilitySeconds)
            {
                sprite.color = new Color(1f, 1f, 1f, 1f);
                timeInvincible = 0.0f;
            }
        }

        if (timeReloading > 0)
        {
            timeReloading += Time.deltaTime;
            if (timeReloading > timeToReload)
            {
                timeReloading = 0.0f;
            }
        }
    }

    void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
        animator.SetFloat("XInput", moveInput.x);
        animator.SetFloat("YInput", moveInput.y);
        animator.SetBool("IsMoving", moveInput != Vector2.zero);

        if (moveInput.x > 0)
        {
            if (moveInput.y > 0)
            {
                currentAimingDirection = Direction.up_right;
            }
            else if (moveInput.y == 0)
            {
                currentAimingDirection = Direction.right;
            }
            else
            {
                currentAimingDirection = Direction.down_right;
            }
        }
        else if (moveInput.x == 0)
        {
            if (moveInput.y > 0)
            {
                currentAimingDirection = Direction.up;
            }
            else if (moveInput.y == 0)
            {
                //Do nothing
            }
            else
            {
                currentAimingDirection = Direction.down;
            }
        }
        else
        {
            if (moveInput.y > 0)
            {
                currentAimingDirection = Direction.up_left;
            }
            else if (moveInput.y == 0)
            {
                currentAimingDirection = Direction.left;
            }
            else
            {
                currentAimingDirection = Direction.down_left;
            }
        }
    }

    void OnFire(InputValue inputValue)
    {
        if (!isDead)
        {

            Vector3 spawningPosition = Vector3.zero;
            Quaternion spawningRotation;

            if (timeReloading == 0.0f)
            {
                if (projectilePrefab)
                {
                    Debug.LogError("Spawned projectile.");
                    spawningPosition = firingDirectionMap[currentAimingDirection].spawningPosition.transform.position;
                    spawningRotation = Quaternion.Euler(0, 0, firingDirectionMap[currentAimingDirection].rotation);
                    Instantiate(projectilePrefab, spawningPosition, spawningRotation, GameManager.Instance.playerProjectileContainer.transform);

                }
            }
            timeReloading += Time.deltaTime;
        }
    }

    void Move()
    {
        if (!isDead)
        {
            var delta = moveInput * moveSpeed;
            rigidBody.velocity = delta;
        }
    }

    public void takeDamage(int damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                timeInvincible += Time.deltaTime;
                sprite.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
        }
    }

    private void Die()
    {
        isDead = true;
        GetComponent<CapsuleCollider2D>().enabled = false;
        rigidBody.velocity = Vector2.zero;
        animator.SetTrigger("IsDead");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyDamage enemyDamage;
        if ((collision.gameObject.layer << GameManager.Instance.enemyDamageLayerMask) > 0)
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
        if ((collision.gameObject.layer << GameManager.Instance.enemyDamageLayerMask) > 0)
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
