using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private GameObject quitGameButton;

    private void Awake()
    {
#if UNITY_WEBGL
        quitGameButton.SetActive(false);
#endif
    }

    public void StartGame()
    {
        if (PersistentObjects.Instance)
            PersistentObjects.Instance.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
