using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingWithScaleOnEnable : MonoBehaviour
{
    [SerializeField]
    private float offsetTime;
    [SerializeField]
    private float duration;
    [SerializeField]
    private float startingScale;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Image image;

    private RectTransform rectTransform;

    // Start is called before the first frame update
    private void OnEnable()
    {
        this.rectTransform = transform as RectTransform;
        StartCoroutine(this.Move());
    }

    IEnumerator Move()
    {
        //Init
        this.canvasGroup.alpha = 0f;

        float timeElasped = 0f;
        float elapsedRatio = 0f;
        float smoothElapsedRatio = 0f;

        // Wait before animation
        while (timeElasped < this.offsetTime)
        {
            timeElasped += Time.deltaTime;
            yield return null;
        }

        // Animation
        timeElasped = 0f;
        while (timeElasped < this.duration)
        {
            timeElasped += Time.deltaTime;
            elapsedRatio = timeElasped / duration;
            smoothElapsedRatio = Mathf.Pow(elapsedRatio, 3);

            float fadingValue = Mathf.Lerp(0f, 1f, smoothElapsedRatio);
            float scaleValue = Mathf.Lerp(startingScale, 1f, smoothElapsedRatio);

            this.rectTransform.localScale = Vector3.one * scaleValue;
            this.canvasGroup.alpha = fadingValue;
            yield return null;
        }
    }
}