using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveNews : MonoBehaviour
{
    public static float ShowAnimationDuration = 1f;
    public static float HideAnimationDuration = 1f;

    public static LogicDelegate LogicDelegate;
    public List<GaugeEffect> gaugeEffects = new List<GaugeEffect>();

    public GameController gameController;

    public void Show()
    {
        this.gameObject.SetActive(true);
        LogicDelegate.OnInteractiveNewsShowStart();
        this.AnimateShow(ShowAnimationDuration);
    }

    void AnimateShow(float duration)
    {
        this.OnShowAnimationEnd();
    }

    private void OnShowAnimationEnd()
    {
        LogicDelegate.OnInteractiveNewsShowEnded();
        //TODO active interactive elements (all clickable area)
    }

    public void Hide()
    {
        LogicDelegate.OnInteractiveNewsHideStart();
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
    public List<GaugeEffect> GetGaugeEffects()
    {
        return gaugeEffects;
    } // TODO get structure influencing the world gauges
}
