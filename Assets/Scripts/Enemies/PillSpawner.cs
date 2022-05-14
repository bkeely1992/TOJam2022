using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillSpawner : MonoBehaviour
{
    [SerializeField] GameObject pill;
    [SerializeField] float pillSpawnTime = 2f;

    float mapX;
    float mapY;

    float verticalExtent;
    float horizontalExtend;

    float minX;
    float maxX;
    float minY;
    float maxY;

    void Awake()
    {
        mapX = Camera.main.transform.position.x;
        mapY = Camera.main.transform.position.y;

        verticalExtent = Camera.main.orthographicSize;
        horizontalExtend = verticalExtent * Screen.width / Screen.height;
    }

    void Start()
    {
        minX = (float)(horizontalExtend - mapX / 2.0);
        maxX = (float)(mapX / 2.0 - horizontalExtend);
        minY = (float)(verticalExtent - mapY / 2.0);
        maxY = (float)(mapY / 2.0) - horizontalExtend;

        StartCoroutine(PillSpawn());
    }

    IEnumerator PillSpawn()
    {
        while (true)
        {
            Vector3 pillSpawn = new Vector3(Random.Range(minX, maxX), minY, 0);
            Instantiate(pill, pillSpawn, Quaternion.identity);

            yield return new WaitForSeconds(pillSpawnTime);
        }
    }
}
