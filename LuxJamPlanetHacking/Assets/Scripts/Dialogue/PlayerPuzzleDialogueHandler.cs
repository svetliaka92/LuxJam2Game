using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPuzzleDialogueHandler : MonoBehaviour, IPredicateEvaluator
{
    private bool isNumberPuzzleComplete = false;
    private bool isEnergyBlockPuzzleComplete = false;
    private bool isColorBlockPuzzleComplete = false;

    public void OnNumberPuzzleComplete()
    {
        isNumberPuzzleComplete = true;
    }

    public void OnEnergyBlockPuzzleComplete()
    {
        isEnergyBlockPuzzleComplete = true;
    }

    public void OnColorBlockPuzzleComplete()
    {
        isColorBlockPuzzleComplete = true;
    }

    public bool? Evaluate(string predicate, string[] paremeters)
    {
        //print($"Predicate: {predicate}");
        switch (predicate)
        {
            case "HasPassword":
                return isNumberPuzzleComplete;
            case "NumberPuzzle":
                return isNumberPuzzleComplete;
            case "EnergyPuzzle":
                return isEnergyBlockPuzzleComplete;
            case "ColorBlocksPuzzle":
                return isColorBlockPuzzleComplete;
            case "ColorBlockPuzzleComplete":
                return isColorBlockPuzzleComplete;
        }

        return null;
    }
}
