using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestDialogue
{
    [SerializeField] List<TestDialogueNode> nodes;

    Dictionary<string, TestDialogueNode> nodeLookup = new Dictionary<string, TestDialogueNode>();

    public void Init()
    {
        nodeLookup.Clear();
        foreach (TestDialogueNode node in nodes)
        {
            nodeLookup[node.nodeName] = node;
        }
    }

    public IEnumerable<TestDialogueNode> GetAllNodes()
    {
        return nodes;
    }

    public TestDialogueNode GetRootNode()
    {
        return nodes[0];
    }

    public IEnumerable<TestDialogueNode> GetAllChildren(TestDialogueNode parentNode)
    {
        foreach (string node in parentNode.children)
        {
            if (nodeLookup.ContainsKey(node))
            {
                yield return nodeLookup[node];
            }
        }
    }

    public IEnumerable<TestDialogueNode> GetPlayerChildren(TestDialogueNode parentNode)
    {
        foreach (TestDialogueNode node in GetAllChildren(parentNode))
        {
            if (node.isPlayerSpeaking)
                yield return node;
        }
    }

    public IEnumerable<TestDialogueNode> GetAllAIChildren(TestDialogueNode parentNode)
    {
        foreach (TestDialogueNode node in GetAllChildren(parentNode))
        {
            if (!node.isPlayerSpeaking)
                yield return node;
        }
    }
}
