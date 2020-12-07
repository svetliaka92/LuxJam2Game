using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private Puzzle[] puzzles;
    [SerializeField] private string[] puzzleIds;
    [SerializeField] private GameObject puzzleUI;
    [SerializeField] private GameObject energyPuzzleCap;
    [SerializeField] private Transform energyPuzzleCapRemovedLocation;
    [SerializeField] private float energyPuzzleCapMoveTime = 2f;
    [SerializeField] private LeanTweenType energyPuzzleCapEasing = LeanTweenType.easeInOutCubic;
    [SerializeField] private float energyPuzzleCapMoveDelay = 3f;
    
    private Puzzle _puzzle;

    private Dictionary<string, Puzzle> puzzleLookup = new Dictionary<string, Puzzle>();

    public void Init()
    {
        for (int i = 0; i < puzzles.Length; ++i)
        {
            puzzleLookup[puzzleIds[i]] = puzzles[i];
        }

        for (int i = 0; i < puzzles.Length; ++i)
        {
            puzzles[i].Init(this, puzzleIds[i]);
            puzzles[i].gameObject.SetActive(false);
        }

        puzzleUI.SetActive(false);

        EnablePuzzle(puzzleIds[0]);
    }

    public void EnablePuzzle(string puzzleId)
    {
        if (puzzleLookup.ContainsKey(puzzleId))
            puzzleLookup[puzzleId].gameObject.SetActive(true); // TODO - update according to art
    }

    public void OpenEnergyPuzzleCap()
    {
        LeanTween.move(energyPuzzleCap, energyPuzzleCapRemovedLocation.position, energyPuzzleCapMoveTime)
                 .setEase(energyPuzzleCapEasing)
                 .setDelay(energyPuzzleCapMoveDelay);
    }

    public void OnPuzzleComplete(string puzzleId)
    {
        Game.Instance.OnPuzzleComplete(puzzleId);
    }

    public void StartColorBlockPuzzle()
    {
        _puzzle = puzzles[2];
        _puzzle.gameObject.SetActive(true);
        ((ColorBlockPuzzle)puzzles[2]).OnPuzzleReset();
        ((ColorBlockPuzzle)puzzles[2]).EnableUI();
    }

    public void ExitColorBlockPuzzle()
    {
        if (_puzzle)
            _puzzle.gameObject.SetActive(false);
        ((ColorBlockPuzzle)puzzles[2]).EnableUI(false);
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
        _puzzle.OnPlayerExit();
    }

    internal void PuzzleSelected(Puzzle puzzle)
    {
        _puzzle = puzzle;
    }

    public void SkipPuzzle()
    {
        if (_puzzle)
            _puzzle.SkipPuzzle();
    }
}
