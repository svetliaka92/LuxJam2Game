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
        HandleInteract();
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

    private void HandleInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (raycastHandler != null)
            {
                print("Interacting with: " + raycastHandler);
                raycastHandler.HandleRaycast();
            }
        }
    }
}
