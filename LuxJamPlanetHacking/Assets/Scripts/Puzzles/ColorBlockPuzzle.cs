using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlockPuzzle : Puzzle
{
    [SerializeField] private GameObject puzzleUI;

    [SerializeField] private ColorBlock[] blocks;
    [SerializeField] private ColorButton[] buttons;
    [SerializeField] private int[] blockStartStates;
    [SerializeField] private int gridSizeX = 3;
    [SerializeField] private int gridSizeY = 3;

    private ColorBlock[,] blockGrid;
    private int[,] blockStates;

    public override void Init(PuzzleController controller, string puzzleId)
    {
        base.Init(controller, puzzleId);

        blockGrid = new ColorBlock[gridSizeX, gridSizeY];
        blockStates = new int[gridSizeX, gridSizeY];

        ShuffleArray(blockStartStates);
        SetBlocksOnStartPosition();
    }

    private void SetBlocksOnStartPosition()
    {
        for (int i = 0; i < gridSizeX; ++i)
        {
            for (int j = 0; j < gridSizeY; ++j)
            {
                blockGrid[i, j] = blocks[i * gridSizeX + j];
                blockStates[i, j] = blockStartStates[i * gridSizeX + j];

                blockGrid[i, j].SetState(blockStates[i, j], true);
            }
        }
    }

    public void OnPuzzleReset()
    {
        SetBlocksOnStartPosition();
    }

    public void EnableUI(bool flag = true)
    {
        puzzleUI.SetActive(flag);
    }

    public override void OnPlayerExit()
    {
        base.OnPlayerExit();

        // reset blocks to start position

    }

    private void ShuffleArray(int[] array)
    {
        int shuffleTimes = 10;
        for (int i = 0; i < shuffleTimes; ++i)
        {
            for (int index = 0; index < array.Length; ++index)
            {
                int randomIndex = UnityEngine.Random.Range(0, array.Length);
                int temp = array[index];
                array[index] = array[randomIndex];
                array[randomIndex] = temp;
            }
        }
    }

    public override void OnComponentInteract(BaseComponent component, object data = null)
    {
        base.OnComponentInteract(component, data);

        ColorButton button = component as ColorButton;

        RotateRowColumn(button.ButtonType, button.ID, button.ButtonDirection);

        CheckPuzzleComplete();
    }

    private void RotateRowColumn(ColorButton.Type type,
                                 int index,
                                 ColorButton.Direction direction)
    {
        int startIndex;
        int endIndex;

        if (direction == ColorButton.Direction.right || direction == ColorButton.Direction.down)
        {
            startIndex = 2; // TODO - change when assets are imported
            endIndex = 0;
        }
        else
        {
            startIndex = 0;
            endIndex = 2; // TODO - change when assets are imported
        }

        if (type == ColorButton.Type.row)
        {
            int tempState = blockStates[index, startIndex];
            if (direction == ColorButton.Direction.right)
            {
                for (int i = startIndex; i > endIndex; --i)
                    blockStates[index, i] = blockStates[index, i - 1];
            }
            else if (direction == ColorButton.Direction.left)
            {
                for (int i = startIndex; i < endIndex; ++i)
                    blockStates[index, i] = blockStates[index, i + 1];
            }

            blockStates[index, endIndex] = tempState;

            for (int i = 0; i < 3; ++i)
                blockGrid[index, i].SetState(blockStates[index, i]);
        }
        else if (type == ColorButton.Type.column)
        {
            int tempState = blockStates[startIndex, index];
            if (direction == ColorButton.Direction.down)
            {
                for (int i = startIndex; i > endIndex; --i)
                    blockStates[i, index] = blockStates[i - 1, index];
            }
            else if (direction == ColorButton.Direction.up)
            {
                for (int i = startIndex; i < endIndex; ++i)
                    blockStates[i, index] = blockStates[i + 1, index];
            }

            blockStates[endIndex, index] = tempState;

            for (int i = 0; i < 3; ++i)
                blockGrid[i, index].SetState(blockStates[i, index]);
        }
    }

    private void CheckPuzzleComplete()
    {
        foreach (ColorBlock block in blocks)
            if (block.State != block.CorrectState)
                return;

        if (!isCompleted)
            OnComplete();
    }
}
