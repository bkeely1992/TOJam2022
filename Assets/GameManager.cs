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
    public LayerMask dialogueTriggerLayerMask;
    public GameObject playerProjectileContainer;
    public GameObject effectContainer;
    public GameObject enemiesContainer;
    public GameObject enemyProjectileContainer;
    public Player player;
    public List<Door> questDoors;
    public GameObject pauseObject;

    [SerializeField] List<HealthBarIndicator> healthBarIndicators = new List<HealthBarIndicator>();

    public enum GameState
    {
        running, paused
    }
    public GameState gameState = GameState.running;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlaySound("GameMusic");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (gameState)
            {
                case GameState.running:
                    Time.timeScale = 0;
                    gameState = GameState.paused;
                    pauseObject.SetActive(true);
                    break;
                case GameState.paused:
                    Time.timeScale = 1;
                    gameState = GameState.running;
                    pauseObject.SetActive(false);
                    break;
            }
        }
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

    public void pauseGame()
    {
        Time.timeScale = 1;
        gameState = GameState.running;
        pauseObject.SetActive(false);
    }

    public void returnToMenu()
    {

    }

    public void quitGame()
    {
        Application.Quit();
    }
}
