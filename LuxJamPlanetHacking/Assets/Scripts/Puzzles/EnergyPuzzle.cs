using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPuzzle : Puzzle
{
    [SerializeField] private float blockMoveTime = 1f;
    [SerializeField] private LeanTweenType blockMoveEasing = LeanTweenType.easeInOutCubic;
    [SerializeField] EnergyBlock startBlock;
    [SerializeField] EnergyBlock endBlock;

    EnergyBlock[] blocks;
    
    EnergyBlock[,] blockGrid;

    int gridSizeX = 3;
    int gridSizeY = 3;

    private bool isBusy = false;

    public override void Init()
    {
        base.Init();

        blocks = new EnergyBlock[components.Length];
        for (int i = 0; i < components.Length; ++i)
        {
            blocks[i] = components[i] as EnergyBlock;

            if (!blocks[i].IsActive)
                blocks[i].gameObject.SetActive(false);
        }

        blockGrid = new EnergyBlock[gridSizeX, gridSizeY]; 
        int temp = 0;
        for (int col = 0; col < gridSizeY; ++col)
        {
            for (int row = 0; row < gridSizeX; ++row)
            {
                blocks[temp].SetCoordinates(new Vector2Int(row, col));

                blockGrid[row, col] = blocks[temp];
                ++temp;
            }
        }

        startBlock.Connect();
        endBlock.Disconnect();

        startBlock.SetCoordinates(new Vector2Int(-1, 0));
        endBlock.SetCoordinates(new Vector2Int(3, 1));

        ResetConnections();
        UpdateConnections(startBlock);
    }

    public override void StartPuzzle()
    {
        base.StartPuzzle();

        foreach (BaseComponent component in components)
            component.Enable();
    }

    public override void OnComponentInteract(BaseComponent component, object data = null)
    {
        base.OnComponentInteract(component);

        if (isBusy)
            return;

        Vector2Int blockCoordinates = (Vector2Int)data;

        EnergyBlock blockToMove = blockGrid[blockCoordinates.x, blockCoordinates.y];
        EnergyBlock freeBlock = GetFreeBlock(blockGrid[blockCoordinates.x, blockCoordinates.y], blockCoordinates);
        //print(freeBlock);
        if (freeBlock)
        {
            SwapBlocks(blockToMove, freeBlock);
        }
    }

    private EnergyBlock GetFreeBlock(EnergyBlock block, Vector2Int blockCoordinates)
    {
        // check left
        int leftIndex = blockCoordinates.x - 1;
        if (leftIndex >= 0 && leftIndex < gridSizeX)
        {
            if (!blockGrid[leftIndex, blockCoordinates.y].IsActive)
                return blockGrid[leftIndex, blockCoordinates.y];
        }
        
        // check up
        int upIndex = blockCoordinates.y + 1;
        if (upIndex >= 0 && upIndex < gridSizeY)
        {
            if (!blockGrid[blockCoordinates.x, upIndex].IsActive)
                return blockGrid[blockCoordinates.x, upIndex];
        }

        // check right
        int rightIndex = blockCoordinates.x + 1;
        if (rightIndex >= 0 && rightIndex < gridSizeX)
        {
            if (!blockGrid[rightIndex, blockCoordinates.y].IsActive)
                return blockGrid[rightIndex, blockCoordinates.y];
        }

        // check down
        int downIndex = blockCoordinates.y - 1;
        if (downIndex >= 0 && downIndex < gridSizeY)
        {
            if (!blockGrid[blockCoordinates.x, downIndex].IsActive)
                return blockGrid[blockCoordinates.x, downIndex];
        }

        return null;
    }

    private void SwapBlocks(EnergyBlock blockToMove, EnergyBlock swapBlock)
    {
        isBusy = true;

        blockGrid[blockToMove.GetCoordinates().x, blockToMove.GetCoordinates().y] = swapBlock;
        blockGrid[swapBlock.GetCoordinates().x, swapBlock.GetCoordinates().y] = blockToMove;

        Vector3 tempPosition = blockToMove.transform.position;
        MoveBlockToPosition(blockToMove, swapBlock.transform.position);
        MoveBlockToPosition(swapBlock, tempPosition);

        Vector2Int tempCoordinates = blockToMove.GetCoordinates();
        blockToMove.SetCoordinates(swapBlock.GetCoordinates());
        swapBlock.SetCoordinates(tempCoordinates);
    }

    private void MoveBlockToPosition(EnergyBlock block, Vector3 position)
    {
        //block.transform.position = position; // TODO - change to LeanTween movement
        LeanTween.move(block.gameObject, position, blockMoveTime)
                 .setEase(blockMoveEasing)
                 .setOnComplete(OnBlockMoveEnd);
    }

    private void OnBlockMoveEnd()
    {
        isBusy = false;

        ResetConnections();
        UpdateConnections(startBlock);
        CheckForCompletion();
    }

    private void ResetConnections()
    {
        foreach (EnergyBlock block in blocks)
            block.Disconnect();
    }

    private void UpdateConnections(EnergyBlock block)
    {
        if (block == startBlock)
        {
            EnergyBlock rightBlock = GetBlockAtPosition(Vector2Int.zero);

            if (rightBlock != null && CollectionContains(startBlock.ValidConnectionsRight, rightBlock.BlockType))
            {
                rightBlock.Connect();

                UpdateConnections(rightBlock);
            }
        }
        else
        {
            EnergyBlock leftBlock = GetBlockAtPosition(block.GetCoordinates() + Vector2Int.left);
            EnergyBlock upBlock = GetBlockAtPosition(block.GetCoordinates() + Vector2Int.down);
            EnergyBlock rightBlock = GetBlockAtPosition(block.GetCoordinates() + Vector2Int.right);
            EnergyBlock downBlock = GetBlockAtPosition(block.GetCoordinates() + Vector2Int.up);

            if (block.BlockType == 1)
            {
                CheckBlockValid(leftBlock, block.ValidConnectionsLeft);
                CheckBlockValid(rightBlock, block.ValidConnectionsRight);
            }
            else if (block.BlockType == 2)
            {
                CheckBlockValid(upBlock, block.ValidConnectionsUp);
                CheckBlockValid(downBlock, block.ValidConnectionsDown);
            }
            else if (block.BlockType == 3)
            {
                CheckBlockValid(leftBlock, block.ValidConnectionsLeft);
                CheckBlockValid(upBlock, block.ValidConnectionsUp);
            }
            else if (block.BlockType == 4)
            {
                CheckBlockValid(upBlock, block.ValidConnectionsUp);
                CheckBlockValid(rightBlock, block.ValidConnectionsRight);
            }
            else if (block.BlockType == 5)
            {
                CheckBlockValid(rightBlock, block.ValidConnectionsRight);
                CheckBlockValid(downBlock, block.ValidConnectionsDown);
            }
            else if (block.BlockType == 6)
            {
                CheckBlockValid(leftBlock, block.ValidConnectionsLeft);
                CheckBlockValid(downBlock, block.ValidConnectionsDown);
            }
        }
    }

    private void CheckBlockValid(EnergyBlock block, int[] validConnections)
    {
        if (block != null
            && !block.IsConnected
            && block.IsActive
            && CollectionContains(validConnections, block.BlockType))
        {
            block.Connect();
            UpdateConnections(block);
        }
    }

    private bool CollectionContains(int[] collection, int value)
    {
        foreach (int i in collection)
            if (i == value)
                return true;

        return false;
    }

    private EnergyBlock GetBlockAtPosition(Vector2Int position)
    {
        if (position.x >= 0 && position.y >= 0)
        {
            foreach (EnergyBlock block in blocks)
                if (block.GetCoordinates() == position)
                    return block;

            if (endBlock.GetCoordinates() == position)
                return endBlock;
        }

        return null;
    }

    private void CheckForCompletion()
    {
        if (!isCompleted && AreAllBlocksCorrect())
            OnComplete();
    }

    private bool AreAllBlocksCorrect()
    {
        if (endBlock.IsConnected)
            return true;

        return false;
    }
}
