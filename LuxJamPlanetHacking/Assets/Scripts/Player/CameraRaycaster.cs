using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRaycaster : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _maxDistance = 2f;

    IRaycastHandler raycastHandler;
    public IRaycastHandler RaycastHandler => raycastHandler;

    private void Update()
    {
        if (IsCursorOverUI())
            return;

        HandleHover();
        if (Game.Instance.IsInteracting)
            HandleInteractInPuzzle();
        else
            HandleInteractWorld();
    }

    private bool IsCursorOverUI()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult raycastResult in results)
        {
            if (raycastResult.gameObject.layer == 5)
                return true;
        }

        return false;
    }

    private void HandleHover()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Game.Instance.IsInteracting)
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                raycastHandler = hitInfo.collider.GetComponent<IRaycastHandler>();
                if (raycastHandler == null)
                    raycastHandler = hitInfo.collider.GetComponentInParent<IRaycastHandler>();
            }
            else
                raycastHandler = null;
        }
        else
        {
            if (Physics.Raycast(ray, out hitInfo, _maxDistance))
            {
                raycastHandler = hitInfo.collider.GetComponent<IRaycastHandler>();
                if (raycastHandler == null)
                    raycastHandler = hitInfo.collider.GetComponentInParent<IRaycastHandler>();
            }
            else
                raycastHandler = null;
        }
        
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
