using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceAnswerDisplay : MonoBehaviour
{
    private TMPro.TextMeshProUGUI text;
    private ChoiceEventDataHolder dataHolder;
    public int sectionIndex = 0;
    public int optionIndex = 0;

    private void Start()
    {
        text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        dataHolder = GetComponentInParent<ChoiceEventDataHolder>();
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        text.text = dataHolder.choiceEvent.sections[sectionIndex].options[optionIndex].text;
    }
}