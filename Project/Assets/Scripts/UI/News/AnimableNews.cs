using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AnimableNews : MonoBehaviour
{
    public static float ShowAnimationDuration = 1f;
    public static float HideAnimationDuration = 1f;

    public static LogicDelegate LogicDelegate;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private TextMeshProUGUI textComponent;

    [SerializeField]
    private Button buttonComponent;

    [SerializeField]
    private string animationEventID;

    private void AddEvent()
    {
        this.buttonComponent.onClick.AddListener(this.Hide);
    }

    private void RemoveEvent()
    {
        this.buttonComponent.onClick.RemoveListener(this.Hide);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
        LogicDelegate.OnAnimableNewsShowStart();
        StartCoroutine(this.AnimateShow(ShowAnimationDuration));
    }

    IEnumerator AnimateShow(float duration)
    {
        float timeElasped = 0f;
        float fadeRatio = 0f;
        while (timeElasped < duration)
        {
            timeElasped += Time.deltaTime;
            fadeRatio = timeElasped / duration;
            this.canvasGroup.alpha = fadeRatio;
            yield return null;
        }

        this.OnShowAnimationEnd();
    }


    private void OnShowAnimationEnd()
    {
        LogicDelegate.OnAnimableNewsShowEnded();
        LogicDelegate.TriggerMapAnimation();
        this.AddEvent();
    }

    public void Hide()
    {
        this.RemoveEvent();
        LogicDelegate.OnAnimableNewsHideStart();
        StartCoroutine(this.AnimateHide(HideAnimationDuration));
    }

    IEnumerator AnimateHide(float duration)
    {
        float timeElasped = 0f;
        float fadeRatio = 0f;
        while (timeElasped < duration)
        {
            timeElasped += Time.deltaTime;
            fadeRatio = 1f - timeElasped / duration;
            this.canvasGroup.alpha = fadeRatio;
            yield return null;
        }

        this.OnHideAnimationEnd();
    }

    private void OnHideAnimationEnd()
    {
        LogicDelegate.OnAnimableNewsHideEnded();
        this.gameObject.SetActive(false);
    }

    public void SetData(EventData data)
    {
        this.textComponent.text = data.text;
    }
}
