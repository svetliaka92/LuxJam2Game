using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _maxDistance = 2f;

    IRaycastHandler raycastHandler;
    public IRaycastHandler RaycastHandler => raycastHandler;

    private void Update()
    {
        HandleHover();
        if (Game.Instance.IsInteracting)
            HandleInteractInPuzzle();
        else
            HandleInteractWorld();
    }

    private void HandleHover()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, _maxDistance))
            raycastHandler = hitInfo.collider.GetComponent<IRaycastHandler>();
        else
            raycastHandler = null;
    }

    private void HandleInteractWorld()
    {
        if (Input.GetKeyDown(KeyCode.E))
            InteractWithObject();
    }

    private void HandleInteractInPuzzle()
    {
        if (Input.GetMouseButtonDown(0))
            InteractWithObject();
    }

    private void InteractWithObject()
    {
        if (raycastHandler != null)
            raycastHandler.HandleRaycast();
    }
}
