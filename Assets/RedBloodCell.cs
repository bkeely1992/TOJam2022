using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBloodCell : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    private Vector3 direction = Vector3.zero;
    private bool initialized = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!initialized)
        {
            direction = new Vector3(0, 0, Random.Range(-180, 180));
            transform.rotation = Quaternion.Euler(direction);
            direction = transform.up;
            transform.rotation = Quaternion.identity;
            initialized = true;
        }
        transform.position = transform.position + ((Vector3)(((Vector2)direction).normalized) * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 dir = (Vector3)(collision.contacts[0].point - (Vector2)transform.position);
        direction = -dir;
    }
}
