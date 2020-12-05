using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseComponent : MonoBehaviour, IRaycastHandler
{
    protected Puzzle _parent;
    protected bool isEnabled = false;

    public bool IsEnabled => isEnabled;

    public void HandleRaycast()
    {
        if (!isEnabled)
            return;

        OnComponentInteract();
    }

    public virtual void Init(Puzzle parent)
    {
        _parent = parent;
    }

    protected virtual void OnComponentInteract()
    {
        //..
    }

    public void Enable(bool flag = true)
    {
        isEnabled = flag;
    }
}
