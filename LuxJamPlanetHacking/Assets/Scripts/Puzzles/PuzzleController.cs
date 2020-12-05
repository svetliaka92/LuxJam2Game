using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    private Puzzle _puzzle;

    public void Init()
    {

    }

    public void OnPlayerInteracted(InteractionType type)
    {
        if (type == InteractionType.Puzzle)
        {
            _puzzle.StartPuzzle();
        }
    }

    internal void PuzzleSelected(Puzzle puzzle)
    {
        _puzzle = puzzle;
    }
}
