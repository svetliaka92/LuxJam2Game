using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private CursorController _cursorController;

    private void Awake()
    {
        _cursorController.Init();
        _game.Init();
    }
}
