using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjects : MonoBehaviour
{
    private static PersistentObjects _instance;
    public static PersistentObjects Instance => _instance;

    [SerializeField] private SceneLoader _sceneLoader;

    private void Awake()
    {
        //print("Awake: Persistent Objects.cs");
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        _sceneLoader.LoadScene(sceneName);
    }
}
