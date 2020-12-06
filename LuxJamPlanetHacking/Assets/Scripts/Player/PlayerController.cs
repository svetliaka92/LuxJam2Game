using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform player;
    [SerializeField] private Transform cameraArm;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [SerializeField] private PlayerConversant _playerConversant;
    [SerializeField] private PlayerPuzzleDialogueHandler _playerDialogueHandler;

    public PlayerConversant GetPlayerConversant => _playerConversant;
    public PlayerPuzzleDialogueHandler GetPlayerDialogueHandler => _playerDialogueHandler;

    private Vector3 movement;
    private Vector3 rotation;

    private Vector3 interactionStartPosition;
    private Vector3 interactionStartRotation;

    public void HandlePuzzleComplete(string puzzleId)
    {
        //print($"Puzzle id: {puzzleId}");
        switch (puzzleId)
        {
            case "NumberPuzzle":
                _playerDialogueHandler.OnNumberPuzzleComplete();
                break;
            case "EnergyPuzzle":
                _playerDialogueHandler.OnEnergyBlockPuzzleComplete();
                break;
            case "ColorBlocksPuzzle":
                _playerDialogueHandler.OnColorBlockPuzzleComplete();
                break;
        }
    }

    private void Update()
    {
        if (Game.Instance != null
            && !Game.Instance.IsPaused
            && !Game.Instance.IsInteracting)
        {
            //..
            HandleInput();
        }
    }

    private void HandleInput()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        movement = player.right * horizontal + player.forward * vertical;
        rotation = new Vector3(mouseY, mouseX, 0f);

        Vector3 rotationThisFrame = rotation * rotationSpeed * Time.deltaTime;

        player.eulerAngles = player.eulerAngles + new Vector3(0f, rotationThisFrame.y, 0f);
        cameraArm.eulerAngles = cameraArm.eulerAngles + new Vector3(-rotationThisFrame.x, 0f, 0f);
    }

    private void FixedUpdate()
    {
        if (Game.Instance != null
            && !Game.Instance.IsPaused
            && !Game.Instance.IsInteracting)
        {
            Vector3 movementThisFrame = movement * movementSpeed * Time.deltaTime;
            player.position += movementThisFrame;
        }
    }

    public void MoveToPosition(Vector3 position, Vector3 rotationEuler)
    {
        interactionStartPosition = player.position;
        interactionStartRotation = player.eulerAngles;

        rigidbody.isKinematic = true;
        rigidbody.detectCollisions = false;

        movement = Vector3.zero;
        rotation = Vector3.zero;

        LeanTween.move(player.gameObject, position, 2f)
                 .setEaseInOutCubic()
                 .setOnStart(OnTweenMovementStart)
                 .setOnComplete(OnTweenMovementEnd);

        LeanTween.rotate(cameraArm.gameObject, rotationEuler, 2f)
                 .setEaseInOutCirc();
    }

    public void ReturnToStandingPosition()
    {
        LeanTween.move(player.gameObject, interactionStartPosition, 2f)
                 .setEaseInOutCubic()
                 .setOnComplete(OnPlayerStandUpComplete);

        LeanTween.rotate(cameraArm.gameObject, interactionStartRotation, 2f)
                 .setEaseInOutCirc();
    }

    private void OnPlayerStandUpComplete()
    {
        rigidbody.isKinematic = false;
        rigidbody.detectCollisions = true;

        Game.Instance.OnPlayerEndInteractComplete();
    }

    private void OnTweenMovementStart()
    {
        Game.Instance.OnPlayerInteractStart();
    }

    private void OnTweenMovementEnd()
    {
        Game.Instance.OnPlayerInteractEnd();
    }
}
