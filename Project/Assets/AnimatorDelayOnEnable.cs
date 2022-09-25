using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimatorDelayOnEnable : MonoBehaviour
{
    [SerializeField]
    private bool shouldBeRepeated;
    [SerializeField]
    private float repeatDelay;

    [SerializeField]
    private bool shouldBeRandom;
    [SerializeField]
    private float randomPositionRange;

    [SerializeField]
    private float offsetTime;
    [SerializeField]
    private Animator animator;

    private Vector3 initialPosition;
    private RectTransform rectTransform;

    // Start is called before the first frame update
    private void Start()
    {
        this.rectTransform = transform as RectTransform;
        initialPosition = this.rectTransform.anchoredPosition;
        StartCoroutine(this.DelayActivation());
    }

    IEnumerator DelayActivation()
    {
        //Init
        if (shouldBeRandom)
        {
            float randomAngle = Random.Range(0f, 2f * Mathf.PI);
            float randomLength = Random.Range(0f, randomPositionRange);
            this.rectTransform.anchoredPosition = initialPosition + new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * randomLength;
        }

        animator.enabled = false;
        float timeElasped = 0f;

        // Wait before animation
        while (timeElasped < this.offsetTime)
        {
            timeElasped += Time.deltaTime;
            yield return null;
        }

        animator.enabled = true;
        animator.SetTrigger("Play");

        timeElasped = 0f;
        if (shouldBeRepeated)
        {
            // Wait before animation
            while (timeElasped < this.repeatDelay)
            {
                timeElasped += Time.deltaTime;
                yield return null;
            }

            StartCoroutine(this.DelayActivation());
        }
    }
}
