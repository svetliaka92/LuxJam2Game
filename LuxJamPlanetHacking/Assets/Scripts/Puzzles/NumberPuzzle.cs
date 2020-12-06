using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberPuzzle : Puzzle
{
    [SerializeField] private NumberBlock[] blocks;
    [SerializeField] private int[] blockCorrectStates;
    [SerializeField] private NumberHint numberHint;

    public override void Init(PuzzleController controller, string puzzleId)
    {
        base.Init(controller, puzzleId);

        blockCorrectStates = new int[blocks.Length];
        for (int i = 0; i < blockCorrectStates.Length; ++i)
            blockCorrectStates[i] = UnityEngine.Random.Range(0, 16);

        if (numberHint)
            numberHint.Init(blockCorrectStates);
    }

    public override void OnComponentInteract(BaseComponent component, object data = null)
    {
        base.OnComponentInteract(component, data);

        if (isCompleted || !CheckBlocksForCompletion())
            return;

        OnComplete();
    }

    private bool CheckBlocksForCompletion()
    {
        for (int i = 0; i < blocks.Length; ++i)
            if (blocks[i].State != blockCorrectStates[i])
                return false;

        return true;
    }
}
