using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateOnEnable : MonoBehaviour
{
    [SerializeField]
    private float offsetTime;
    [SerializeField]
    private float rotation;
    [SerializeField]
    private float duration;
    [SerializeField]
    private Image image;

    private RectTransform rectTransform;

    private void OnEnable()
    {
        this.rectTransform = transform as RectTransform;
        StartCoroutine(this.Rotate());
    }

    private float OutBackEase(float x)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;
        return 1f + c3* Mathf.Pow(x - 1f, 3f) + c1 * Mathf.Pow(x - 1f, 2f);
    }

    IEnumerator Rotate()
    {
        //Init
        float finalAngle = this.transform.rotation.eulerAngles.z;
        float initialAngle = finalAngle - rotation;
        this.rectTransform.rotation = Quaternion.Euler(0f, 0f, initialAngle); ;

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
            smoothElapsedRatio = this.OutBackEase(elapsedRatio);
            float rotationAngle = initialAngle + rotation * smoothElapsedRatio;
            this.rectTransform.rotation = Quaternion.Euler(0f,0f, rotationAngle);
            yield return null;
        }
    }
}
