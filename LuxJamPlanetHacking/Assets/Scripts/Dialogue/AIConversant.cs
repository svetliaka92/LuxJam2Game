using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConversant : MonoBehaviour
{
    [SerializeField] private string AIName = "Planet Terminal";

    public string GetAIName => AIName;
    DialogueTrigger[] dialogueTriggers;

    private void Awake()
    {
        dialogueTriggers = GetComponents<DialogueTrigger>();
    }

    public DialogueTrigger[] GetDialogueTriggers() => dialogueTriggers;
}
