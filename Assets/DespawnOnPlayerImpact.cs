using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnOnPlayerImpact : MonoBehaviour
{
    [SerializeField] GameObject impactEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player &&
            !player.isInvincible)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity, GameManager.Instance.effectContainer.transform);
            Destroy(gameObject);
        }
    }
}
