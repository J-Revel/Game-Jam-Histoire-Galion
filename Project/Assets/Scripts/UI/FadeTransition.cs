using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTransition : MonoBehaviour
{
    public CanvasGroup canvasGroup1, canvasGroup2;

    public float transitionDuration = 1;
    private bool playing = false;
    private float transitionTime = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if(playing)
        {
            canvasGroup1.blocksRaycasts = false;
            canvasGroup1.interactable = false;
            canvasGroup2.blocksRaycasts = true;
            canvasGroup2.interactable = true;
            transitionTime += Time.deltaTime;
            float t = transitionTime / transitionDuration;
            canvasGroup2.alpha = t;
        }
    }

    public void StartTransition()
    {
        playing = true;
    }
}
