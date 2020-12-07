using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberPuzzle : Puzzle
{
    [SerializeField] private NumberBlock[] blocks;
    [SerializeField] private int[] blockCorrectStates;
    [SerializeField] private NumberHint numberHint;
    [SerializeField] private GameObject passwordPane;
    [SerializeField] private Transform passwordPaneDownLocation;
    [SerializeField] private float passwordPaneTweenTime = 1f;
    [SerializeField] private LeanTweenType passwordPaneTweenEasing = LeanTweenType.easeOutBounce;
    [SerializeField] private float passwordPaneTweenDelay = 1f;

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

    protected override void OnComplete()
    {
        base.OnComplete();

        LeanTween.move(passwordPane, passwordPaneDownLocation.position, passwordPaneTweenTime)
                 .setEase(passwordPaneTweenEasing)
                 .setDelay(passwordPaneTweenDelay);
    }
}
