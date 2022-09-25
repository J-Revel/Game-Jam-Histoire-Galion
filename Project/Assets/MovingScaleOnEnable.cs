using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingScaleOnEnable : MonoBehaviour
{
    [SerializeField]
    private float offsetTime;
    [SerializeField]
    private float duration;
    [SerializeField]
    private Vector2 distanceMoved;
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
        Vector2 finalPosition = rectTransform.anchoredPosition;
        Vector2 initialPosition = rectTransform.anchoredPosition - distanceMoved;
        this.image.enabled = false;
        float timeElasped = 0f;
        float elapsedRatio = 0f;
        float smoothElapsedRatio = 0f;

        // Wait before animation
        while (timeElasped < this.offsetTime)
        {
            timeElasped += Time.deltaTime;
            yield return null;
        }
        this.image.enabled = true;

        // Animation
        timeElasped = 0f;
        while (timeElasped < this.duration)
        {
            timeElasped += Time.deltaTime;
            elapsedRatio = timeElasped / duration;
            smoothElapsedRatio = Mathf.Pow(elapsedRatio, 3);
            Vector3 position = Vector3.Lerp(initialPosition, finalPosition, elapsedRatio);
            this.rectTransform.anchoredPosition = position;
            yield return null;
        }
    }
}
