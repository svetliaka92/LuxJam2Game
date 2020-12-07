using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI AIText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Transform AIResponseRoot;
    [SerializeField] private Transform choiceRoot;
    [SerializeField] private DialogueChoiceUI choicePrefab;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI conversantName;
    [SerializeField] private TextMeshProUGUI debugText;

    private PlayerConversant _player;

    public void Init(PlayerConversant player)
    {
        _player = player;
        _player.onConversationUpdated += UpdateUI;

        conversantName.text = _player.GetAIConversantName();

        if (nextButton)
            nextButton.onClick.AddListener(_player.Next);

        if (quitButton)
        {
            quitButton.onClick.AddListener(Game.Instance.ReturnPlayerToStandingPosition);
            quitButton.onClick.AddListener(_player.QuitDialogue);
        }
    }

    private void UpdateUI()
    {
        gameObject.SetActive(_player.IsActive());

        if (!_player.IsActive())
            return;

        choiceRoot.gameObject.SetActive(_player.IsChoosing());

        if (_player.IsChoosing())
        {
            BuildChoiceList();
            nextButton.gameObject.SetActive(false);
        }
        else
        {
            AIText.text = _player.GetText();
            nextButton.gameObject.SetActive(_player.HasNext());
        }

        //debugText.text = "Player has next: " + _player.HasNext() + ", Next button state: " + nextButton.gameObject.activeSelf;
    }

    private void BuildChoiceList()
    {
        foreach (Transform child in choiceRoot)
            Destroy(child.gameObject);

        foreach (DialogueNode node in _player.GetChoices())
        {
            DialogueChoiceUI dialogueChoice = Instantiate(choicePrefab, choiceRoot);
            dialogueChoice.SetText(node.Text);

            Button button = dialogueChoice.GetButton();
            button.onClick.AddListener(() => _player.SelectChoice(node));
        }
    }
}
