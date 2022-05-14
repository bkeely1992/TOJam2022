using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int damage;
    [SerializeField] GameObject impactPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (Vector3)(speed * transform.up * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            return;
        }
        Door door = collision.GetComponent<Door>();
        if (door)
        {
            Debug.LogError("Hit door.");
            door.TakeDamage(damage);
        }

        GameObject impactEffectObject = Instantiate(impactPrefab, transform.position, Quaternion.identity, GameManager.Instance.effectContainer.transform);
        Vector3 dir = (Vector3)((Vector3)collision.transform.position - transform.position);
        dir = -dir.normalized;
        impactEffectObject.transform.up = dir;
        Destroy(gameObject);
    }
}
