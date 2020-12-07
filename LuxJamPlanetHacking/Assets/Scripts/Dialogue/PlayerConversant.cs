using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerConversant : MonoBehaviour
{
    [SerializeField] string playerName;

    TestDialogue currentDialogue;
    TestDialogueNode currentNode = null;
    AIConversant currentConversant = null;
    bool isChoosing = false;

    public event Action onConversationUpdated;

    public void StartDialogue(TestDialogue newDialogue, AIConversant newConversant)
    {
        currentConversant = newConversant;
        currentDialogue = newDialogue;
        currentNode = currentDialogue.GetRootNode();
        TriggerEnterAction();
        onConversationUpdated();
    }

    public void Quit()
    {
        currentDialogue = null;
        TriggerExitAction();
        currentNode = null;
        isChoosing = false;
        currentConversant = null;
        onConversationUpdated();
    }

    public bool IsActive()
    {
        return currentDialogue != null;
    }

    public bool IsChoosing()
    {
        return isChoosing;
    }

    public string GetText()
    {
        if (currentNode == null)
        {
            return "";
        }

        return currentNode.text;
    }

    public string GetCurrentConversantName()
    {
        if (isChoosing)
        {
            return playerName;
        }
        else
        {
            if (currentConversant)
                return currentConversant.GetAIName;
            else
                return "";
        }
    }

    public IEnumerable<TestDialogueNode> GetChoices()
    {
        return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode));
    }

    public void SelectChoice(TestDialogueNode chosenNode)
    {
        currentNode = chosenNode;
        TriggerEnterAction();
        isChoosing = false;
        Next();
    }

    public void Next()
    {
        int numPlayerResponses = FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();
        if (numPlayerResponses > 0)
        {
            isChoosing = true;
            TriggerExitAction();
            onConversationUpdated();
            return;
        }

        TestDialogueNode[] children = FilterOnCondition(currentDialogue.GetAllAIChildren(currentNode)).ToArray();
        int randomIndex = UnityEngine.Random.Range(0, children.Count());
        TriggerExitAction();
        currentNode = children[randomIndex];
        TriggerEnterAction();
        onConversationUpdated();
    }

    public bool HasNext()
    {
        return FilterOnCondition(currentDialogue.GetAllChildren(currentNode)).Count() > 0;
    }

    private IEnumerable<TestDialogueNode> FilterOnCondition(IEnumerable<TestDialogueNode> inputNode)
    {
        foreach (var node in inputNode)
        {
            if (node.CheckCondition(GetEvaluators()))
            {
                yield return node;
            }
        }
    }

    private IEnumerable<IPredicateEvaluator> GetEvaluators()
    {
        return GetComponents<IPredicateEvaluator>();
    }

    private void TriggerEnterAction()
    {
        if (currentNode != null)
        {
            TriggerAction(currentNode.onEnterAction);
        }
    }

    private void TriggerExitAction()
    {
        if (currentNode != null)
        {
            TriggerAction(currentNode.onExitAction);
        }
    }

    private void TriggerAction(string action)
    {
        if (action == "") return;

        foreach (DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
        {
            trigger.Trigger(action);
        }
    }
}
