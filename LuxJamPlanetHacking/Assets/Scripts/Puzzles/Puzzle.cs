using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    [SerializeField] protected BaseComponent[] components;
    protected bool isCompleted = false;

    public virtual void Init()
    {
        foreach (BaseComponent component in components)
        {
            // disable component
            component.Init(this);
            component.Enable(false);
        }
    }

    public virtual void StartPuzzle()
    {
        // enable components
    }

    public virtual void OnComponentInteract(BaseComponent component, object data = null)
    {
        //..
    }

    protected virtual void OnComplete()
    {
        isCompleted = true;
        print($"Puzzle: {gameObject} completed");
    }
}
