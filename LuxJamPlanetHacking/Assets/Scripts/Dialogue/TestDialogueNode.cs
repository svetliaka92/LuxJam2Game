using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestDialogueNode
{
    public string nodeName;
    public bool isPlayerSpeaking;
    public string text;
    public List<string> children;
    public string onEnterAction;
    public string onExitAction;
    public Condition condition;

    public bool CheckCondition(IEnumerable<IPredicateEvaluator> evaluators)
    {
        return condition.Check(evaluators);
    }
}
