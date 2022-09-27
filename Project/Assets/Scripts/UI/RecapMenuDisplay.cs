using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecapMenuDisplay : MonoBehaviour
{
    public TMPro.TextMeshProUGUI presentationTitleText;
    public TMPro.TextMeshProUGUI presentationDescriptionText;
    public TMPro.TextMeshProUGUI currentEventTitleText;
    public TMPro.TextMeshProUGUI currentEventDescriptionText;
    public CanvasGroup canvasGroup;
    public float duration = 0.5f;
    

    public void UpdateDisplay(EventData presentationEvent, EventData currentEvent)
    {
        if(presentationEvent.id.Length > 0)
        {
            presentationTitleText.text = presentationEvent.title;
            presentationDescriptionText.text = presentationEvent.text;
        }
        currentEventTitleText.text = "Événement en cours : " + currentEvent.title;
        currentEventDescriptionText.text = currentEvent.text;
    }

    public void Show()
    {
        StartCoroutine(ShowAnimation());
    }

    public void Hide()
    {
        StartCoroutine(HideAnimation());
    }
    
    private IEnumerator ShowAnimation()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        for(float time=0; time<1; time+=Time.deltaTime)
        {
            canvasGroup.alpha = time/duration;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }
    
    private IEnumerator HideAnimation()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        for(float time=0; time<1; time+=Time.deltaTime)
        {
            canvasGroup.alpha = 1 - time/duration;
            yield return null;
        }
        canvasGroup.alpha = 0;
    }
}
