using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnOnPlayerImpact : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            Destroy(gameObject);
        }
    }
}
