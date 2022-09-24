using System;
using System.Collections;
using UnityEngine;

public class AnimableNews : MonoBehaviour
{
    public static float ShowAnimationDuration = 1f;
    public static float HideAnimationDuration = 1f;

    public static LogicDelegate LogicDelegate;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private MapAnimationType animationType;

    public void Show()
    {
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
        if (this.animationType != MapAnimationType.None)
        {
            LogicDelegate.TriggerMapAnimation(this.animationType);
        }
    }

    public void Hide()
    {
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
        this.gameObject.SetActive(false);
        LogicDelegate.OnAnimableNewsHideEnded();
    }
}
