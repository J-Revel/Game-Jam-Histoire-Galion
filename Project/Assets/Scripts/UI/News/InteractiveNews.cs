using System;
using System.Collections;
using UnityEngine;

public class InteractiveNews : MonoBehaviour
{
    public static float ShowAnimationDuration = 1f;
    public static float HideAnimationDuration = 1f;

    public static LogicDelegate LogicDelegate;

    [SerializeField]
    private CanvasGroup canvasGroup;

    public void Show()
    {
        this.gameObject.SetActive(true);
        LogicDelegate.OnInteractiveNewsShowStart();
        StartCoroutine(this.AnimateShow(ShowAnimationDuration));
    }

    IEnumerator AnimateShow(float duration)
    {
        float timeElasped = 0f;
        float fadeRatio = 0f;
        while(timeElasped < duration)
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
        LogicDelegate.OnInteractiveNewsShowEnded();
        //TODO active interactive elements
    }

    public void Hide()
    {
        LogicDelegate.OnInteractiveNewsHideStart();
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
        LogicDelegate.OnInteractiveNewsHideEnded();
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Compute the effect on the world variables.
    /// </summary>
    public void ComputeEffect()
    {

    } // TODO get structure influencing the world gauges
}
