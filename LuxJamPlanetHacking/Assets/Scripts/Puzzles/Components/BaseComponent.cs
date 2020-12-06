using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseComponent : MonoBehaviour, IRaycastHandler
{
    [SerializeField] private Collider componentCollider;
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
        if (!IsEnabled)
            return;
    }

    public void Enable(bool flag = true)
    {
        isEnabled = flag;
        if (componentCollider)
            componentCollider.enabled = flag;
    }
}
