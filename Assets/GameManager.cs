using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public LayerMask enemyDamageLayerMask;
    public LayerMask playerProjectileLayerMask;
    public LayerMask playerLayerMask;
    public LayerMask collectibleLayerMask;
    public LayerMask brainLayerMask;
    public GameObject playerProjectileContainer;
    public GameObject effectContainer;
    public GameObject enemiesContainer;
    public GameObject enemyProjectileContainer;
    public Player player;
    public List<Door> questDoors;

    [SerializeField] List<HealthBarIndicator> healthBarIndicators = new List<HealthBarIndicator>();

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlaySound("GameMusic");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void unlockDoor(string doorName)
    {
        
        foreach(Door door in questDoors)
        {
            if(door.identifier == doorName)
            {
                
                door.OpenDoor();
            }
        }
    }

    public void updateUIHealthBar(int healthValue)
    {
        foreach(HealthBarIndicator healthbarIndicator in healthBarIndicators)
        {
            if(healthbarIndicator.healthBarIndex <= healthValue)
            {
                healthbarIndicator.turnOnSprite();
            }
            else
            {
                healthbarIndicator.turnOffSprite();
            }
        }
    }
}
