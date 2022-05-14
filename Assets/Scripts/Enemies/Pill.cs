using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float timeAlive = 2f;
    [SerializeField] float rotateSpeed = 50f;

    Vector3 rotationDirection;

    // Start is called before the first frame update
    void Start()
    {
        rotationDirection = Random.Range(1, 3) % 2 == 0 ? Vector3.back : Vector3.forward;
        StartCoroutine(WaitAndDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        gameObject.transform.Rotate(rotationDirection * rotateSpeed * Time.deltaTime);
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(timeAlive);
        Destroy(gameObject);
    }
}
