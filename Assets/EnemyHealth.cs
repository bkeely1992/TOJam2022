using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 1.0f;
    [SerializeField] GameObject deathEffect;

    private float currentHealth = 1.0f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer << GameManager.Instance.playerProjectileLayerMask) > 0)
        {
            currentHealth--;
            if (currentHealth <= 0)
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity, GameManager.Instance.effectContainer.transform);
                Destroy(gameObject);
            }
        }
    }
}
