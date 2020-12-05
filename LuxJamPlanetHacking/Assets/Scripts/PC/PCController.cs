using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCController : MonoBehaviour
{
    [SerializeField] private GameObject UICanvas;

    private void Awake()
    {
        UICanvas.SetActive(false);
    }

    public void OnPlayerInteracted(InteractionType type)
    {
        if (type == InteractionType.PC)
        {
            UICanvas.SetActive(true);
        }
        else
        {
            UICanvas.SetActive(false);
        }
    }
}
