using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBlock : BaseComponent
{
    [SerializeField] private bool isActive = true;
    [SerializeField] private int blockType = 0;
    [SerializeField] private int[] validConnectionsLeft;
    [SerializeField] private int[] validConnectionsUp;
    [SerializeField] private int[] validConnectionsRight;
    [SerializeField] private int[] validConnectionsDown;

    [SerializeField] private float connectionTweenTime = 0.3f;
    [SerializeField] private LeanTweenType connectionTweenEasing = LeanTweenType.easeInOutCubic;
    [SerializeField] private Renderer rend;
    [SerializeField] private Material normalMat;
    [SerializeField] private Material connectedMat;

    private bool isConnected = false;
    public bool IsConnected => isConnected;

    public int BlockType => blockType;
    public int[] ValidConnectionsLeft => validConnectionsLeft;
    public int[] ValidConnectionsUp => validConnectionsUp;
    public int[] ValidConnectionsRight => validConnectionsRight;
    public int[] ValidConnectionsDown => validConnectionsDown;

    public bool IsActive => isActive;

    private Vector2Int coordinates;

    private int connectionTweenId = -1;
    private float connectionTweenValue = 0f;

    public void SetCoordinates(Vector2Int newCoordinates)
    {
        coordinates = newCoordinates;
    }

    public Vector2Int GetCoordinates() => coordinates;

    protected override void OnComponentInteract()
    {
        base.OnComponentInteract();

        _parent.OnComponentInteract(this, coordinates);
    }

    public void Connect()
    {
        isConnected = true;

        CancelConnectAnimation();

        connectionTweenId = LeanTween.value(connectionTweenValue, 1, connectionTweenTime)
                                     .setEase(connectionTweenEasing)
                                     .setOnUpdate(UpdateWireMat)
                                     .setOnComplete(CompleteConnectAnimation)
                                     .uniqueId;
    }

    public void Disconnect()
    {
        isConnected = false;

        connectionTweenId = LeanTween.value(connectionTweenValue, 0, connectionTweenTime)
                                     .setEase(connectionTweenEasing)
                                     .setOnUpdate(UpdateWireMat)
                                     .setOnComplete(CompleteConnectAnimation)
                                     .uniqueId;
    }

    private void UpdateWireMat(float value)
    {
        rend.material.Lerp(normalMat, connectedMat, value);
    }

    private void CompleteConnectAnimation()
    {
        connectionTweenId = -1;
    }

    private void CancelConnectAnimation()
    {
        if (connectionTweenId == -1)
            return;

        LeanTween.cancel(connectionTweenId);
        connectionTweenId = -1;
    }
}
