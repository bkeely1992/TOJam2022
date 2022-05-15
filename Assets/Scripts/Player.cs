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
    [SerializeField] float timeToReload = 0.25f;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] float controllerAimOffset = 0.35f;
    [SerializeField] string firingSound = "";
    [SerializeField] string damageSound = "";

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject upgradedProjectilePrefab;
    [SerializeField] float UpgradeDuration = 3f;
    float timeUpgraded = 0.0f;

    public bool isInvincible
    {
        get
        {
            return timeInvincible > 0 || isDodging;
        }
    }
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
    Vector2 lookInput;
    int currentHealth = 5;
    float timeInvincible = 0.0f;
    float timeReloading = 0.0f;

    [SerializeField] float dodgeSpeed = 20f;
    bool isDodging = false;

    Dictionary<FiringDirection.Direction, FiringDirection> firingDirectionMap = new Dictionary<Direction, FiringDirection>();
    bool isDead = false;
    bool isFiring = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        foreach (FiringDirection firingDirection in firingDirections)
        {
            firingDirectionMap.Add(firingDirection.direction, firingDirection);
        }
    }

    void Update()
    {
        Move();
        FireProjectile();

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

        if (timeUpgraded > 0)
        {
            timeUpgraded += Time.deltaTime;
            if (timeUpgraded > UpgradeDuration)
            {
                timeUpgraded = 0.0f;
            }
        }
    }

    void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
        SetLookDirection();
        animator.SetBool("IsMoving", moveInput != Vector2.zero);
    }

    void OnLook(InputValue inputValue)
    {
        if (!isDead)
        {
            lookInput = inputValue.Get<Vector2>();
            SetLookDirection();

            if (lookInput.x > 0)
            {
                if (lookInput.y > 0)
                {
                    currentAimingDirection = Direction.up_right;
                }
                else if (-controllerAimOffset < lookInput.y && lookInput.y < controllerAimOffset)
                {
                    currentAimingDirection = Direction.right;
                }
                else
                {
                    currentAimingDirection = Direction.down_right;
                }
            }
            else if (-controllerAimOffset < lookInput.x && lookInput.x < controllerAimOffset)
            {
                if (lookInput.y > 0)
                {
                    currentAimingDirection = Direction.up;
                }
                else if (lookInput.y == 0)
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
                if (lookInput.y > 0)
                {
                    currentAimingDirection = Direction.up_left;
                }
                else if (-controllerAimOffset < lookInput.y && lookInput.y < controllerAimOffset)
                {
                    currentAimingDirection = Direction.left;
                }
                else
                {
                    currentAimingDirection = Direction.down_left;
                }
            }

            if (lookInput == Vector2.zero)
                isFiring = false;
            else
                isFiring = true;
        }
    }

    void OnDodge(InputValue inputValue)
    {
        if (inputValue.isPressed && !isDead && !isDodging && moveInput != Vector2.zero)
            StartCoroutine(Dodge());
    }

    private IEnumerator Dodge()
    {
        isDodging = true;

        var originalScale = gameObject.transform.localScale;

        if (moveInput.x < 0)
            gameObject.transform.localScale = new Vector2(originalScale.x * -1, originalScale.y);

        animator.SetBool("IsDodging", true);

        rigidBody.AddForce(moveInput * dodgeSpeed);

        yield return new WaitForSeconds(0.5f);

        gameObject.transform.localScale = originalScale;
        animator.SetBool("IsDodging", false);
        isDodging = false;
    }

    private void FireProjectile()
    {
        if (isFiring)
        {
            Vector3 spawningPosition = Vector3.zero;
            Quaternion spawningRotation;

            if (timeReloading == 0.0f)
            {
                var projectile = timeUpgraded > 0 ? upgradedProjectilePrefab : projectilePrefab;

                if (projectile != null)
                {
                    AudioManager.Instance.PlaySound(firingSound, true);
                    spawningPosition = firingDirectionMap[currentAimingDirection].spawningPosition.transform.position;
                    spawningRotation = Quaternion.Euler(0, 0, firingDirectionMap[currentAimingDirection].rotation);
                    Instantiate(projectile, spawningPosition, spawningRotation, GameManager.Instance.playerProjectileContainer.transform);
                }
            }
            timeReloading += Time.deltaTime;
        }
    }

    private void SetLookDirection()
    {
        if (lookInput != Vector2.zero)
        {
            animator.SetFloat("XInput", lookInput.x);
            animator.SetFloat("YInput", lookInput.y);
        }
        else if (moveInput != Vector2.zero)
        {
            animator.SetFloat("XInput", moveInput.x);
            animator.SetFloat("YInput", moveInput.y);
        }
        else
        {
            animator.SetFloat("XInput", 0);
            animator.SetFloat("YInput", -1);
        }
    }

    void Move()
    {
        if (!isDead && !isDodging)
        {
            var delta = moveInput * moveSpeed;
            rigidBody.velocity = delta;
        }
    }

    public void takeDamage(int damage)
    {
        if (!isDead)
        {
            AudioManager.Instance.PlaySound(damageSound);
            currentHealth -= damage;

            GameManager.Instance.updateUIHealthBar(currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                if (!isInvincible)
                {
                    timeInvincible += Time.deltaTime;
                    sprite.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                }
            }
        }
    }

    private void Die()
    {
        isDead = true;
        isFiring = false;
        isDodging = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        rigidBody.velocity = Vector2.zero;
        animator.SetTrigger("IsDead");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyDamage enemyDamage;
        if ((GameManager.Instance.enemyDamageLayerMask & (1 << collision.gameObject.layer)) != 0)
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
        if ((GameManager.Instance.enemyDamageLayerMask & (1 << collision.gameObject.layer)) != 0)
        {
            enemyDamage = collision.gameObject.GetComponent<EnemyDamage>();
            if (enemyDamage)
            {
                takeDamage(enemyDamage.damage);
            }
        }
        else if ((GameManager.Instance.collectibleLayerMask & (1 << collision.gameObject.layer)) != 0)
        {
            Debug.Log($"collected: {collision.gameObject.name}, {collision.gameObject.layer}");

            var collectible = collision.gameObject.GetComponent<Collectible>();

            switch (collectible.collectibleType)
            {
                case CollectibleType.WeaponUpgrade:
                    timeUpgraded += Time.deltaTime;
                    break;
                case CollectibleType.Stomach_Key:
                    GameManager.Instance.unlockDoor("stomach");
                    break;
                case CollectibleType.Brain_Key:
                    GameManager.Instance.unlockDoor("brain");
                    break;
                case CollectibleType.HealthPack:
                    currentHealth = currentHealth + 1 >= maxHealth ? maxHealth : currentHealth + 1;
                    GameManager.Instance.updateUIHealthBar(currentHealth);
                    break;
            }

            Destroy(collision.gameObject);
        }
    }
}
