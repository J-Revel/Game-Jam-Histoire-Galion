using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceSelector : MonoBehaviour
{
    public InteractiveNews interactiveNews;
    public ChoiceAnswerDisplay[] selectedAnswers;
    private ChoiceEventDataHolder dataHolder;
    public Button confirmButton;

    private void Awake()
    {
        interactiveNews = GetComponentInParent<InteractiveNews>();
        dataHolder = GetComponentInParent<ChoiceEventDataHolder>();
        for(int i=0; i<selectedAnswers.Length; i++)
        {
            selectedAnswers[i].sectionIndex = i;
            int index = i;
            selectedAnswers[i].GetComponentInChildren<Button>().onClick.AddListener(() => {
                int optionCount = dataHolder.choiceEvent.sections[index].options.Count;
                selectedAnswers[index].optionIndex = (selectedAnswers[index].optionIndex + 1) % optionCount;
                selectedAnswers[index].UpdateDisplay();
            });
        }
        confirmButton.onClick.AddListener(() => {
            for(int i=0; i<selectedAnswers.Length; i++)
            {
                foreach(GaugeEffect effect in dataHolder.choiceEvent.sections[i].options[selectedAnswers[i].optionIndex].effects)
                {
                    interactiveNews.gaugeEffects.Add(effect);
                }
                interactiveNews.Hide();
            }
        });
    }
}