using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] int loadGameScene = 1;

    void Start() => AudioManager.Instance.PlaySound("Hospital Sounds");

    public void LoadGame() => SceneManager.LoadScene(loadGameScene);

    public void QuitGame() => Application.Quit();
}
