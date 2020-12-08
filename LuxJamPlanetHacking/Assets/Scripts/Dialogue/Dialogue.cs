using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Game/Dialogue", order = 0)]
public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
    [SerializeField] Vector2 newNodeOffset = new Vector2(250, 0);
    [SerializeField] bool displayPlayerNodeTextOnChoice = false;

    public bool DisplayPlayerNodeTextOnChoice => displayPlayerNodeTextOnChoice;

    Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        nodeLookup.Clear();
        foreach (DialogueNode node in nodes)
            nodeLookup[node.name] = node;
    }

    public IEnumerable<DialogueNode> GetAllNodes()
    {
        return nodes;
    }

    public DialogueNode GetRootNode()
    {
        return nodes[0];
    }

    public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
    {
        foreach (string childID in parentNode.Children)
            if (nodeLookup.ContainsKey(childID))
                yield return nodeLookup[childID];
    }

    public IEnumerable<DialogueNode> GetPlayerChoices(DialogueNode parentNode)
    {
        foreach (DialogueNode node in GetAllChildren(parentNode))
            if (node.IsPlayerSpeaking)
                yield return node;
    }

    public IEnumerable<DialogueNode> GetAllAIChildren(DialogueNode parentNode)
    {
        foreach (DialogueNode node in GetAllChildren(parentNode))
            if (!node.IsPlayerSpeaking)
                yield return node;
    }

#if UNITY_EDITOR
    public void CreateNode(DialogueNode parentNode)
    {
        DialogueNode createdNode = MakeNode(parentNode);


        Undo.RegisterCreatedObjectUndo(createdNode, "Created dialogue node");
        Undo.RecordObject(this, "Create dialogue node");


        AddNode(createdNode);
    }

    private void AddNode(DialogueNode node)
    {
        nodes.Add(node);
        OnValidate();
    }

    private DialogueNode MakeNode(DialogueNode parentNode)
    {
        DialogueNode createdNode = CreateInstance<DialogueNode>();
        createdNode.name = Guid.NewGuid().ToString();

        if (parentNode != null)
        {
            parentNode.AddChild(createdNode.name);
            createdNode.SetIsPlayerSpeaking(!parentNode.IsPlayerSpeaking);

            createdNode.SetPosition(parentNode.GetRect.position + newNodeOffset);
        }

        return createdNode;
    }

    public void DeleteNode(DialogueNode nodeToDelete)
    {

        Undo.RecordObject(this, "Delete dialogue node");

        nodes.Remove(nodeToDelete);
        OnValidate();
        CleanDanglingChildren(nodeToDelete);

        Undo.DestroyObjectImmediate(nodeToDelete);

    }

    private void CleanDanglingChildren(DialogueNode nodeToDelete)
    {
        foreach (DialogueNode node in GetAllNodes())
            node.RemoveChild(nodeToDelete.name);
    }
#endif

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR

        if (nodes.Count <= 0)
        {
            DialogueNode createdNode = MakeNode(null);
            AddNode(createdNode);
        }

        if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(this)))
        {
            foreach (DialogueNode node in GetAllNodes())
                if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(node)))
                    AssetDatabase.AddObjectToAsset(node, this);
        }

#endif
    }

    public void OnAfterDeserialize()
    {
        //..
    }
}
