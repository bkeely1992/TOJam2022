using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBloodCell : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    private Vector3 direction = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        //Choose a random direction
        direction = new Vector3(0, 0, Random.Range(-180, 180));
        transform.rotation = Quaternion.Euler(direction);
        direction = transform.up;
        transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 dir = (Vector3)((Vector3)collision.contacts[0].point - transform.position);
        direction = -dir.normalized;
    }
}
