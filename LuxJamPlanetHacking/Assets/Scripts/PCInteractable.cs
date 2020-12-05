using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCInteractable : MonoBehaviour, IRaycastHandler
{
    [SerializeField] private Transform location;
    
    public void HandleRaycast()
    {
        Game.Instance.OnPlayerInteract(InteractionType.PC, location);
    }
}
