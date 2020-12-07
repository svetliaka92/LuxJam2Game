using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Game";

    public void StartGame()
    {
        if (PersistentObjects.Instance)
            PersistentObjects.Instance.LoadScene(gameSceneName);
    }
}
