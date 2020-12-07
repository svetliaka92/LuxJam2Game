using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static Game _instance;
    public static Game Instance => _instance;

    [SerializeField] private CameraRaycaster _cameraRaycaster;
    [SerializeField] private PlayerController _player;
    [SerializeField] private PCController _pcController;
    [SerializeField] private PuzzleController _puzzleController;
    [SerializeField] private DialogueUI _dialogueUI;
    [SerializeField] private GameObject _pauseMenu;

    [SerializeField] private string winSceneName = "Win";

    private bool _isPaused = false;
    public bool IsPaused => _isPaused;

    private bool _isInteracting = false;
    public bool IsInteracting => _isInteracting;

    private bool isStarted = false;
    public bool IsStarted => isStarted;

    public PlayerController Player => _player;

    private InteractionType _type = InteractionType.None;

    public event Action<InteractionType> onPlayerInteractEvent;

    public void Init()
    {
        //..
        _instance = this;
        _isPaused = false;

        _cameraRaycaster.Init();
        _puzzleController.Init();
        _dialogueUI.Init(_player.GetPlayerConversant);

        onPlayerInteractEvent += _pcController.OnPlayerInteracted;
        onPlayerInteractEvent += _puzzleController.OnPlayerInteracted;
    }

    private void OnDestroy()
    {
        isStarted = false;
        _instance = null;
    }

    internal void StartGame()
    {
        isStarted = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // pause game
            // activate pause panel
            _isPaused = !_isPaused;
            UpdatePauseMenu();
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
            ReturnPlayerToStandingPosition();

        if (Input.GetKeyDown(KeyCode.T))
            StartGame();
#endif
    }

    public void Unpause()
    {
        _isPaused = false;
        UpdatePauseMenu();
    }

    public void GoToMainMenu()
    {
        //..
        // Call ToMainMenu on SceneLoader
        if (PersistentObjects.Instance)
            PersistentObjects.Instance.LoadScene("MainMenu");
    }

    private void UpdatePauseMenu()
    {
        CursorController.Instance.Enable(_isPaused);
        _pauseMenu.SetActive(_isPaused);

        if (!_isPaused
            && _isInteracting)
            CursorController.Instance.Enable(true);
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
        if (PersistentObjects.Instance)
        {
            PersistentObjects.Instance.LoadScene(winSceneName);
            CursorController.Instance.Enable(true);
        }
    }
}
