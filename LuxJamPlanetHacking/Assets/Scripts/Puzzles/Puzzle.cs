using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    [SerializeField] protected BaseComponent[] components;
    [SerializeField] protected Collider interactable;
    protected bool isCompleted = false;

    private PuzzleController controller;
    private string puzzleId = "";

    public virtual void Init(PuzzleController controller, string id)
    {
        this.controller = controller;
        puzzleId = id;

        foreach (BaseComponent component in components)
            component.Init(this);

        ResetPuzzle();
    }

    public virtual void ResetPuzzle()
    {
        foreach (BaseComponent component in components)
            component.Enable(false);
    }

    public virtual void OnPlayerExit()
    {
        foreach (BaseComponent component in components)
            component.Enable(false);

        if (!isCompleted && interactable)
            interactable.enabled = true;
    }

    public virtual void StartPuzzle()
    {
        if (isCompleted)
            return;

        if (interactable)
            interactable.enabled = false;

        // enable components
        foreach (BaseComponent component in components)
            component.Enable();
    }

    public virtual void OnComponentInteract(BaseComponent component, object data = null)
    {
        //..
    }

    public virtual void SkipPuzzle()
    {
        if (isCompleted)
            return;

        OnComplete();
    }

    protected virtual void OnComplete()
    {
        isCompleted = true;
        print($"Puzzle: {gameObject} completed");

        controller.OnPuzzleComplete(puzzleId);

        foreach (BaseComponent component in components)
            component.Enable(false);
    }
}
