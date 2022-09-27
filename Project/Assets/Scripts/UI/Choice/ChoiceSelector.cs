using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceSelector : MonoBehaviour
{
    public InteractiveNews interactiveNews;
    public ChoiceAnswerDisplay answerButtonPrefab;
    public ChoiceAnswerDisplay[] selectedAnswers;
    public TranslateTargetMovement[] selectedAnswerMovements;
    public TranslateTargetMovement[] buttonSlots;
    private ChoiceEventDataHolder dataHolder;
    public Button confirmButton;
    public RectTransform newspaperTransform;
    public TranslateTargetMovement[] additionalButtonsStartPoints;
    public Transform[] additionalButtonsTargetPoints;
    private List<TranslateTargetMovement> usedTranslateTargetMovements = new List<TranslateTargetMovement>();
    public Transform exitTarget;
    public CanvasGroup selectorBackgroundGroup;
    public TitleAnimatedWriter titleAnimatedWriter;
    public GameController gameController;
    

    private IEnumerator Start()
    {
        interactiveNews = GetComponentInParent<InteractiveNews>();
        dataHolder = GetComponentInParent<ChoiceEventDataHolder>();
        selectedAnswers = new ChoiceAnswerDisplay[dataHolder.choiceEvent.sections.Length];
        selectedAnswerMovements = new TranslateTargetMovement[dataHolder.choiceEvent.sections.Length];
        for(int i=0; i<dataHolder.choiceEvent.sections.Length; i++)
        {
            selectedAnswerMovements[i] = Instantiate(buttonSlots[i], exitTarget.position, exitTarget.rotation, buttonSlots[i].transform.parent);
            selectedAnswerMovements[i].SetTarget(buttonSlots[i].transform);
            selectedAnswerMovements[i].show = true;
            selectedAnswers[i] = Instantiate(answerButtonPrefab, selectedAnswerMovements[i].transform);
        }
        for(int i=0; i<selectedAnswers.Length; i++)
        {
            selectedAnswers[i].sectionIndex = i;
            int index = i;
            selectedAnswers[i].GetComponentInChildren<Button>().onClick.AddListener(() => {
                StartCoroutine(OpenVersionSelector(index));
            });
        }
        confirmButton.onClick.AddListener(() => {
            for(int i=0; i<selectedAnswers.Length; i++)
            {
                GaugeEffect[] effects = dataHolder.choiceEvent.sections[i].options[selectedAnswers[i].optionIndex].effects;
                if(effects != null)
                {
                    foreach(GaugeEffect effect in effects)
                    {
                        interactiveNews.gaugeEffects.Add(effect);
                    }
                }
                StartCoroutine(CloseMenuAnimation());
            }
        });

        yield return null;
        float duration = 1;
        for(float time=0; time < duration; time+= Time.deltaTime)
        {
            float t = time / duration;
            t = 1 - (1-t)*(1-t);
            newspaperTransform.pivot = new Vector2((1-t) * (1-t), 1-t);
            yield return null;
        }
        string title = "";
        for(int i=0; i<selectedAnswers.Length; i++)
        {
            if(i > 0)
                title += " ";
            title += dataHolder.choiceEvent.sections[selectedAnswers[i].sectionIndex].options[selectedAnswers[i].optionIndex].text;
        }
        titleAnimatedWriter.WriteText(title);
    }

    public IEnumerator OpenVersionSelector(int section)
    {
        int optionCount = dataHolder.choiceEvent.sections[section].options.Count;
        int cursor = 0;
        List<TranslateTargetMovement> answers = new List<TranslateTargetMovement>();
        List<Transform> exitTargets = new List<Transform>();
        Transform selectedTarget = buttonSlots[section].transform;
        for(int i=0; i<optionCount; i++)
        {
            if(selectedAnswers[section].optionIndex != i)
            {
                TranslateTargetMovement translateTargetMovement = Instantiate(additionalButtonsStartPoints[cursor], additionalButtonsStartPoints[cursor].transform.parent);
                translateTargetMovement.show = true;
                usedTranslateTargetMovements.Add(translateTargetMovement);
                ChoiceAnswerDisplay choiceAnswer = Instantiate(answerButtonPrefab, translateTargetMovement.transform);
                choiceAnswer.sectionIndex = section;
                choiceAnswer.optionIndex = i;
                int optionIndex = i;
                answers.Add(translateTargetMovement);
                exitTargets.Add(additionalButtonsStartPoints[cursor].transform);
                choiceAnswer.GetComponentInChildren<Button>().onClick.AddListener(() => {
                    StartCoroutine(CloseVersionSelector(section, optionIndex, answers, exitTargets, selectedTarget));
                });
                cursor++;
            }
            else
            {
                exitTargets.Add(exitTarget);
                answers.Add(selectedAnswerMovements[section]);
                selectedAnswerMovements[section].transform.SetParent(additionalButtonsStartPoints[0].transform.parent);
                Button button = selectedAnswerMovements[section].GetComponentInChildren<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    StartCoroutine(CloseVersionSelector(section, selectedAnswers[section].optionIndex, answers, exitTargets, selectedTarget));
                });
            }
        }
        float duration = 0.5f;
        selectorBackgroundGroup.blocksRaycasts = true;
        for(float t=0; t<duration; t += Time.deltaTime)
        {
            float f = t / duration;
            selectorBackgroundGroup.alpha = 1 - (1-f)*(1-f);
            yield return null;
        }
        // 
        // selectedAnswers[index].optionIndex = (selectedAnswers[index].optionIndex + 1) % optionCount;
        // selectedAnswers[index].UpdateDisplay();
    }

    public IEnumerator CloseVersionSelector(int section, int optionIndex, List<TranslateTargetMovement> answers, List<Transform> exitTargets, Transform selectedTarget)
    {
        for(int i=0; i<answers.Count; i++)
        {
            if(i != optionIndex)
            {
                answers[i].SetTarget(exitTargets[i]);
                answers[i].show = true;
                answers[i].destroyAtEnd = true;
            }
            else
            {
                answers[i].SetTarget(selectedTarget);
                selectedAnswerMovements[section] = answers[i];
                selectedAnswerMovements[section].transform.SetParent(buttonSlots[0].transform.parent);
                selectedAnswers[section] = selectedAnswerMovements[section].GetComponentInChildren<ChoiceAnswerDisplay>();
                Button button = selectedAnswerMovements[section].GetComponentInChildren<Button>();
                button.onClick.RemoveAllListeners();
                int index = selectedAnswers[section].sectionIndex;
                button.onClick.AddListener(() => {
                    StartCoroutine(OpenVersionSelector(index));
                });
            }
        }
        exitTargets.Clear();
        answers.Clear();
        float duration = 0.5f;
        selectorBackgroundGroup.blocksRaycasts = false;
        for(float t=0; t<duration; t += Time.deltaTime)
        {
            selectorBackgroundGroup.alpha = 1 - t / duration;
            yield return null;
        }
        string title = "";
        for(int i=0; i<selectedAnswers.Length; i++)
        {
            if(i > 0)
                title += " ";
            title += dataHolder.choiceEvent.sections[selectedAnswers[i].sectionIndex].options[selectedAnswers[i].optionIndex].text;
        }
        titleAnimatedWriter.WriteText(title);
    }

    private IEnumerator CloseMenuAnimation()
    {
        float duration = 1;
        for(int i=0; i<selectedAnswerMovements.Length; i++)
        {
            selectedAnswerMovements[i].SetTarget(exitTarget);
            selectedAnswerMovements[i].show = true;
            yield return new WaitForSeconds(0.1f);
        }
        for(float time=duration; time > 0; time -= Time.deltaTime)
        {
            float t = time / duration;
            t = 1 - (1-t)*(1-t);
            newspaperTransform.pivot = new Vector2((1-t) * (1-t), 1-t);;
            yield return null;
        }
        yield return null;
        interactiveNews.Hide();
    }

    public void ShowRecapPopup()
    {
        interactiveNews.gameController.ShowRecapPopup();
    }
}