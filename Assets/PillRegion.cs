using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillRegion : MonoBehaviour
{
    [SerializeField] GameObject pillSpawner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            pillSpawner.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            pillSpawner.SetActive(false);
        }
    }
}
