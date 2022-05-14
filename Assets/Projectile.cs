using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;

    private Vector2 movementDirection = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (Vector3)(speed * movementDirection * Time.deltaTime);
    }

    public void setMovementDirection(float x, float y)
    {
        movementDirection = new Vector2(x, y);
    }
}
