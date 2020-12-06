using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static Game _instance;
    public static Game Instance => _instance;

    [SerializeField] private PlayerController _player;
    [SerializeField] private PCController _pcController;
    [SerializeField] private PuzzleController _puzzleController;
    [SerializeField] private DialogueUI _dialogueUI;

    private bool _isPaused = false;
    public bool IsPaused => _isPaused;

    private bool _isInteracting = false;
    public bool IsInteracting => _isInteracting;
    public PlayerController Player => _player;

    private InteractionType _type = InteractionType.None;

    public event Action<InteractionType> onPlayerInteractEvent;

    public void Init()
    {
        //..
        _instance = this;

        _puzzleController.Init();
        _dialogueUI.Init(_player.GetPlayerConversant);

        onPlayerInteractEvent += _pcController.OnPlayerInteracted;
        onPlayerInteractEvent += _puzzleController.OnPlayerInteracted;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
            ReturnPlayerToStandingPosition();
#endif
    }

    public void OnPlayerInteract(InteractionType type = InteractionType.None, Transform location = null)
    {
        if (location != null)
        {
            _type = type;
            _isInteracting = _type != InteractionType.None;
            _player.MoveToPosition(location.position, location.eulerAngles);
        }
    }

    public void ReturnPlayerToStandingPosition()
    {
        onPlayerInteractEvent?.Invoke(InteractionType.None);
        CursorController.Instance.Enable(false);

        if (_isInteracting)
            _player.ReturnToStandingPosition();
    }

    public void OnPlayerInteractStart()
    {
        // player starting movement
    }

    public void OnPlayerInteractEnd()
    {
        onPlayerInteractEvent?.Invoke(_type);
        // player ending movement
        if (_type == InteractionType.PC)
        {
            // open PC interface
        }
        else if (_type == InteractionType.Puzzle)
        {
            // start puzzle
        }

        CursorController.Instance.Enable(true);
    }

    internal void OnPlayerEndInteractComplete()
    {
        // player ending interaction
        _isInteracting = false;
    }

    public void OnPuzzleComplete(string id)
    {
        _player.HandlePuzzleComplete(id);
        _pcController.OnPuzzleComplete(id);
    }

    public void OpenWinScreen()
    {
        print("Game won!");
    }
}
