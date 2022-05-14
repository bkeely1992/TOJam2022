using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float timeAlive = 2f;

    // Start is called before the first frame update
    void Start()
    {
        WaitAndDestroy();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += Vector3.down * moveSpeed * Time.deltaTime;
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(timeAlive);
        Destroy(gameObject);
    }
}
