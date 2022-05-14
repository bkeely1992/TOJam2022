using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public LayerMask enemyDamageLayerMask;
    public LayerMask playerProjectileLayerMask;
    public LayerMask collectibleLayerMask;
    public GameObject playerProjectileContainer;
    public GameObject effectContainer;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
