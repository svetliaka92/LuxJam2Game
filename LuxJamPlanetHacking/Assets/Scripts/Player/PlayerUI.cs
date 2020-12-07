using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject interactionUI;

    private void Awake()
    {
        interactionUI.SetActive(false);
    }

    private void Update()
    {
        if (Game.Instance == null
            || !Game.Instance.IsStarted
            || Game.Instance.IsPaused)
        {
            interactionUI.SetActive(false);
            return;
        }

        if (CameraRaycaster.Instance)
        {
            if (!Game.Instance.IsInteracting)
                interactionUI.SetActive(CameraRaycaster.Instance.RaycastHandler != null);
            else
                interactionUI.SetActive(false);
        }
    }
}
