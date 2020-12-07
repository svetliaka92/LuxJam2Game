using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberHint : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] numberTexts;

    public void Init(int[] numbers)
    {
        for (int i = 0; i < numberTexts.Length; ++i)
            numberTexts[i].text = string.Format("{0:X}", numbers[i]);
    }
}
