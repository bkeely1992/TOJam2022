using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCellSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> cells;
    [SerializeField] float cellSpawnTime = 2f;

    void Start()
    {   
        StartCoroutine(InitialCellSpawn());
        StartCoroutine(CellSpawn());
    }

    IEnumerator InitialCellSpawn()
    {
        foreach(GameObject cell in cells)
        {
            Instantiate(cell, gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(cellSpawnTime);
        }
    }

    IEnumerator CellSpawn()
    {
        while (true)
        {
            foreach (GameObject cell in cells)
            {
                if (!cell)
                    Instantiate(cell, gameObject.transform.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(cellSpawnTime);
        }
    }
}
