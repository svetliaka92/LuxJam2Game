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

        rend.material = connectedMat;
    }

    public void Disconnect()
    {
        isConnected = false;

        rend.material = normalMat;
    }
}
