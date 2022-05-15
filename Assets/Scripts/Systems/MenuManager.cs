using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] int loadMainMenuScene = 0;
    [SerializeField] int loadGameScene = 1;
    [SerializeField] int loadGameOverScene = 2;
    

    void Start() => AudioManager.Instance.PlaySound("Hospital Sounds");

    public void LoadMainMenu() => SceneManager.LoadScene(loadMainMenuScene);

    public void LoadGame() => SceneManager.LoadScene(loadGameScene);

    public void LoadGameOverScene() => SceneManager.LoadScene(loadGameOverScene);

    public void QuitGame() => Application.Quit();
}
