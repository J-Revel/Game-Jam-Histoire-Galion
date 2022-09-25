using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopOnEnable : MonoBehaviour
{
    [SerializeField]
    private float offsetTime;
    [SerializeField]
    private float duration;

    private Vector3 initialScale;
    private RectTransform rectTransform;

    private void OnEnable()
    {
        this.rectTransform = transform as RectTransform;
        this.initialScale = this.rectTransform.localScale;
        StartCoroutine(this.Pop());
    }

    private float OutBackEase(float x)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;
        return 1f + c3 * Mathf.Pow(x - 1f, 3f) + c1 * Mathf.Pow(x - 1f, 2f);
    }

    IEnumerator Pop()
    {
        //Init
        float height = rectTransform.rect.height;
        Vector3 finalPosition = this.rectTransform.anchoredPosition;
        Vector3 initialPosition = finalPosition + Vector3.down * height;
        this.rectTransform.anchoredPosition = initialPosition;

        float timeElasped = 0f;
        float elapsedRatio = 0f;

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
            this.rectTransform.localScale = this.initialScale * Mathf.Pow(elapsedRatio, 0.3f);
            this.rectTransform.anchoredPosition = initialPosition + Vector3.up * height * this.OutBackEase(elapsedRatio);
            yield return null;
        }
    }
}
