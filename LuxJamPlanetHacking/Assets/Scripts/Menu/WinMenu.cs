using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMenu : MonoBehaviour
{
    [SerializeField] private string mainSceneName = "MainMenu";
    [SerializeField] private string gameSceneName = "Game";

    public void ToMainScene()
    {
        if (PersistentObjects.Instance)
            PersistentObjects.Instance.LoadScene(mainSceneName);
    }

    public void PlayAgain()
    {
        if (PersistentObjects.Instance)
            PersistentObjects.Instance.LoadScene(gameSceneName);
    }
}
