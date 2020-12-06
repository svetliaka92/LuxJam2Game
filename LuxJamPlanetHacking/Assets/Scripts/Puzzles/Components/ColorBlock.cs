using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlock : MonoBehaviour
{
    [SerializeField] private int state = 0;
    [SerializeField] private int correctState = 0;

    [SerializeField] private GameObject objectToRotate;
    [SerializeField] private float rotationTime = 0.3f;
    [SerializeField] private LeanTweenType easing = LeanTweenType.easeInOutCirc;
    [SerializeField] private Vector3 rotationState0 = Vector3.zero;
    [SerializeField] private Vector3 rotationState1 = new Vector3(0f, 0f, 180f);

    public int State => state;
    public int CorrectState => correctState;

    private int rotationTweenId = -1;

    public void SetState(int state, bool instant = false)
    {
        this.state = state;
        RotateToState(instant);
    }

    private void RotateToState(bool instant)
    {
        // TODO - rotate with tweening
        CancelRotationTween();

        rotationTweenId = LeanTween.rotateLocal(objectToRotate,
                                                (state == 0) ? rotationState0 : rotationState1,
                                                (instant) ? 0f : 0.3f)
                                   .setEase(easing)
                                   .setOnComplete(CompleteRotationTween)
                                   .uniqueId;
    }

    private void CompleteRotationTween()
    {
        rotationTweenId = -1;
    }

    private void CancelRotationTween()
    {
        if (rotationTweenId == -1)
            return;

        LeanTween.cancel(rotationTweenId);
        rotationTweenId = -1;
    }
}
