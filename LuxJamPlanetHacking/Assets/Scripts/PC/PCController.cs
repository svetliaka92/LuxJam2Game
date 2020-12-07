using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCController : MonoBehaviour
{
    [SerializeField] private TestDialogue[] PCTestDialoguePaths;
    [SerializeField] private Dialogue[] PCDialoguePaths;
    [SerializeField] private string[] dialogueIds;
    [SerializeField] private string numberPuzzleId;
    [SerializeField] private string energyBlockPuzzleId;
    [SerializeField] private string colorBlockPuzzleId;

    [SerializeField] private AIConversant aiConversant;
    [SerializeField] private GameObject UICanvas;

    private TestDialogue PCTestDialogue;
    private Dialogue PCDialogue;

    private Dictionary<string, Dialogue> dialogueLookup = new Dictionary<string, Dialogue>();
    private Dictionary<string, TestDialogue> testDialogueLookup = new Dictionary<string, TestDialogue>();

    private void Awake()
    {
        UICanvas.SetActive(false);

        BuildDialogueLookup();

        //PCDialogue = dialogueLookup[dialogueIds[0]]; // load first dialogue
        //PCDialogue = dialogueLookup[dialogueIds[2]];

        foreach (TestDialogue testDialogue in PCTestDialoguePaths)
            testDialogue.Init();

        PCTestDialogue = PCTestDialoguePaths[0];
    }

    private void BuildDialogueLookup()
    {
        for (int i = 0; i < PCDialoguePaths.Length; ++i)
        {
            dialogueLookup[dialogueIds[i]] = PCDialoguePaths[i];
        }

        for (int i = 0; i < PCTestDialoguePaths.Length; ++i)
        {
            testDialogueLookup[dialogueIds[i]] = PCTestDialoguePaths[i];
        }
    }

    public void OnPlayerInteracted(InteractionType type)
    {
        if (type == InteractionType.PC)
        {
            UICanvas.SetActive(true);
            Game.Instance.Player.GetPlayerConversant.StartDialogue(PCTestDialogue, aiConversant);
        }
        else
        {
            UICanvas.SetActive(false);
        }
    }
    
    public void OnPasswordGiven()
    {
        // load energy not connected dialogue
        //PCDialogue = dialogueLookup[dialogueIds[1]];
        PCTestDialogue = testDialogueLookup[dialogueIds[1]];
    }

    public void OnPuzzleComplete(string puzzleId)
    {
        if (puzzleId.Equals(numberPuzzleId))
        {
            // can show password option in player responses
        }
        else if (puzzleId.Equals(energyBlockPuzzleId))
        {
            //PCDialogue = dialogueLookup[dialogueIds[2]];
            PCTestDialogue = testDialogueLookup[dialogueIds[2]];
        }
    }
}
