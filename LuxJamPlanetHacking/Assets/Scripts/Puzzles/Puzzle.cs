using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    [SerializeField] private bool isInUI;
    [SerializeField] private BaseComponent[] components;

    public bool IsInUI => isInUI;

    public void Init()
    {
        foreach (BaseComponent component in components)
        {
            // disable component
            // set component puzzle parent to this
        }
    }

    public void StartPuzzle()
    {
        // enable components
    }
}
