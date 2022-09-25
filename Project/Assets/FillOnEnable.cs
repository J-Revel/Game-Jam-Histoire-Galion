using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillOnEnable : MonoBehaviour
{
    [SerializeField]
    private float offsetTime;
    [SerializeField]
    private float duration;
    [SerializeField]
    private Image image;
    // Start is called before the first frame update
    private void OnEnable()
    {

        StartCoroutine(this.Fill());
    }

    IEnumerator Fill()
    {
        //Init
        this.image.fillAmount = 0f;
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
            this.image.fillAmount = elapsedRatio;
            yield return null;
        }
    }
}
