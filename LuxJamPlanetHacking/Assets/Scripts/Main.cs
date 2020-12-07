using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private CursorController _cursorController;
    [SerializeField] private GameObject _instructionsScreen;

    private void Awake()
    {
        _cursorController.Init();
        _game.Init();

        _instructionsScreen.SetActive(true);
        _cursorController.Enable(true);
    }

    public void StartGame()
    {
        _instructionsScreen.SetActive(false);
        _game.StartGame();

        _cursorController.Enable(false);
    }
}
