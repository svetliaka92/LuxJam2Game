using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInteractable : MonoBehaviour, IRaycastHandler
{
    [SerializeField] private Transform location;
    [SerializeField] private Puzzle parentPuzzle;
    [SerializeField] private PuzzleController puzzleController;

    public void HandleRaycast()
    {
        Game.Instance.OnPlayerInteract(InteractionType.Puzzle, location);
        puzzleController.PuzzleSelected(parentPuzzle);
    }
}
