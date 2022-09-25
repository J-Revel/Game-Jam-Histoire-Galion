using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField]
    private float startingCount;
    [SerializeField]
    private GameController observedController;
    [SerializeField]
    private TextMeshProUGUI textComponent;

    private float displayedCount = 2000f;

    private void Start()
    {
        this.displayedCount = startingCount;
        this.textComponent.text = this.displayedCount.ToString();
    }

    private void Update()
    {
        if (this.displayedCount != this.observedController.state.Money)
        {
            this.StopAllCoroutines();
            StartCoroutine(ChangeTo(this.observedController.state.Money));
        }
    }

    private IEnumerator ChangeTo(float amount)
    {
        float initialCount = this.displayedCount;
        this.displayedCount = amount;

        float timeElasped = 0f;
        float duration = 3f;
        float elapsedRatio = 0f;

        // Wait before animation
        while (timeElasped < duration)
        {
            timeElasped += Time.deltaTime;
            elapsedRatio = Mathf.Pow(timeElasped / duration, 0.1f);
            this.textComponent.text = Mathf.Max(Mathf.Round(Mathf.Lerp(initialCount, amount, elapsedRatio)),0).ToString();
            yield return null;
        }

        this.textComponent.text = amount.ToString();
    }
}
