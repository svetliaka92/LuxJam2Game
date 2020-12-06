using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBlock : BaseComponent
{
    [SerializeField] private int maxStates = 16;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float rotationTime = 0.4f;
    [SerializeField] private LeanTweenType rotationEasing = LeanTweenType.easeInOutCirc;

    private int state = 0;
    public int State => state;

    private bool isButy = false;

    private float rotationCorrection = 0f;

    public override void Init(Puzzle parent)
    {
        base.Init(parent);

        rotationCorrection = 360f / maxStates;
    }

    protected override void OnComponentInteract()
    {
        base.OnComponentInteract();

        if (isButy || !isEnabled)
            return;

        ++state;
        Rotate();
    }

    public void Rotate()
    {
        isButy = true;

        LeanTween.rotateAroundLocal(gameObject, rotationAxis, (rotationCorrection), rotationTime)
                 .setEase(rotationEasing)
                 .setOnComplete(OnRotationComplete);
    }

    private void OnRotationComplete()
    {
        if (state >= maxStates)
            state = 0;

        _parent.OnComponentInteract(this, state);

        isButy = false;
    }
}
