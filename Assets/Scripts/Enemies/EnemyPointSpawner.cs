using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointSpawner : MonoBehaviour
{
    [SerializeField] int maxSpawn = 5;
    [SerializeField] float spawningTime = 5f;
    [SerializeField] GameObject spawnPrefab;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private float timeSinceLastSpawn = 0.0f;

    void Start()
    {   
        
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if(timeSinceLastSpawn > spawningTime)
        {
            for(int i = spawnedEnemies.Count - 1; i >= 0; i--)
            {
                if(spawnedEnemies[i] == null)
                {
                    spawnedEnemies.RemoveAt(i);
                }
            }

            if(spawnedEnemies.Count < maxSpawn)
            {
                spawnedEnemies.Add(Instantiate(spawnPrefab, transform.position, Quaternion.identity, GameManager.Instance.enemiesContainer.transform));
            }

            timeSinceLastSpawn = 0.0f;
        }
    }

}
