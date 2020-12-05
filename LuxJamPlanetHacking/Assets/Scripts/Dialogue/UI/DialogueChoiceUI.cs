using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueChoiceUI : MonoBehaviour
{
    [SerializeField] private Button choiceButton;
    [SerializeField] private TextMeshProUGUI choiceText;

    public Button GetButton() => choiceButton;

    public void SetText(string text)
    {
        choiceText.text = text;
    }
}
