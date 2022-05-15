using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> pills;
    [SerializeField] float pillSpawnTime = 2f;
    [SerializeField] float pillHeightOffset = 3.0f;


    float mapX;
    float mapY;

    float verticalExtent;
    float horizontalExtend;

    float minX = -143;
    float maxX = -134.5f;
    float minY;

    private float timeSinceLastSpawn = 0.0f;

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if(timeSinceLastSpawn > pillSpawnTime)
        {
            GetCameraCoordinates();
            Vector3 pillSpawn = new Vector3(Random.Range(minX, maxX), minY, 0);
            Instantiate(pills[Random.Range(0, pills.Count)], pillSpawn, Quaternion.identity);
            timeSinceLastSpawn = 0.0f;
        }
    }

    void GetCameraCoordinates()
    {
        mapX = Camera.main.transform.position.x;
        mapY = Camera.main.transform.position.y;

        verticalExtent = Camera.main.orthographicSize;
        horizontalExtend = verticalExtent * Screen.width / Screen.height;

        //minX = (float)(mapX - horizontalExtend / 2.0);
        //maxX = (float)(mapX + horizontalExtend / 2.0);
        minY = (float)(mapY + verticalExtent / 2.0) + pillHeightOffset;
    }
}
