using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 1.0f;
    [SerializeField] GameObject deathEffect;
    [SerializeField] float timeToShowDamage = 0.25f;
    [SerializeField] List<SpriteRenderer> spritesToShowDamageOn = new List<SpriteRenderer>();
    [SerializeField] string damageSound = "";

    private float currentHealth = 1.0f;
    private float timeShowingDamage = 0.0f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if(timeShowingDamage > 0.0f)
        {
            timeShowingDamage += Time.deltaTime;
            if(timeShowingDamage > timeToShowDamage)
            {
                foreach (SpriteRenderer renderer in spritesToShowDamageOn)
                {
                    renderer.color = new Color(1f, 1f, 1f);
                }
                timeShowingDamage = 0.0f;
            }
        }
    }

private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((GameManager.Instance.playerProjectileLayerMask & (1 << collision.gameObject.layer)) != 0)
        {
            AudioManager.Instance.PlaySound(damageSound, true);
            timeShowingDamage += Time.deltaTime;
            foreach(SpriteRenderer renderer in spritesToShowDamageOn)
            {
                renderer.color = new Color(1, 0.5f, 0.5f);
            }

            currentHealth--;
            if (currentHealth <= 0)
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity, GameManager.Instance.effectContainer.transform);
                Destroy(gameObject);
            }
        }
    }
}
