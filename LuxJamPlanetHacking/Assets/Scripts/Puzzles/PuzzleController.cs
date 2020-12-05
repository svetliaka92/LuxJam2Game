using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private Puzzle[] puzzles;
    [SerializeField] private GameObject puzzleUI;

    private Puzzle _puzzle;

    public void Init()
    {
        foreach (Puzzle puzzle in puzzles)
            puzzle.Init();

        puzzleUI.SetActive(false);
    }

    public void OnPlayerInteracted(InteractionType type)
    {
        if (type == InteractionType.Puzzle)
        {
            _puzzle.StartPuzzle();

            puzzleUI.SetActive(true);
        }
    }

    public void OnPlayerExit()
    {
        puzzleUI.SetActive(false);
    }

    internal void PuzzleSelected(Puzzle puzzle)
    {
        _puzzle = puzzle;
    }
}
