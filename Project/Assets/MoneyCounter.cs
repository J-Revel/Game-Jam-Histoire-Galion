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
    private TextMeshProUGUI textComponent;
    [SerializeField]
    private TextMeshProUGUI textInfoComponent;
    [SerializeField]
    private Vector3 infoAnimationStartingOffset = Vector3.up * 50f;

    private string[] goodProvenances = new string[5] { "Impressions", "Diffusions", "Distributions", "Livraisons", "Impots" };
    private string[] badProvenances = new string[5] { "Milices", "Militaires", "Nationalistes", "Paramilitaires", "Anonymes" };

    Queue<DisplayCountData> dataQueue = new Queue<DisplayCountData>();

    public class DisplayCountData
    {
        public string Description;
        public float Amount;
    }

    private float displayedCount;
    private bool animating;

    private void Start()
    {
        this.displayedCount = this.startingCount;
        this.textComponent.text = this.displayedCount.ToString();
    }

    public void EnqueueChanges(float amount)
    {
        string description = string.Empty;
        if (amount > 0)
        {
            description = badProvenances[UnityEngine.Random.Range(0, badProvenances.Length)];
        }
        else
        {
            description = goodProvenances[UnityEngine.Random.Range(0, goodProvenances.Length)];
        }

        dataQueue.Enqueue(
            new DisplayCountData
            {
                Description = description,
                Amount = amount,
            });

        this.TryDequeueChanges();
    }

    public void TryDequeueChanges()
    {
        if (!this.animating)
        {
            if(this.dataQueue.Count > 0)
            {
                StartCoroutine(this.ChangeTo(this.dataQueue.Dequeue()));
            }
        }
    }

    private IEnumerator ChangeTo(DisplayCountData data)
    {
        this.animating = true;
        float initialCount = this.displayedCount;
        this.displayedCount += data.Amount;
        this.textInfoComponent.text = data.Description + ": " + (data.Amount > 0? "+" : "") + data.Amount.ToString();

        float timeElasped = 0f;
        float duration = 1f;
        float elapsedRatio = 0f;

        // Wait before animation
        while (timeElasped < duration)
        {
            timeElasped += Time.deltaTime;
            elapsedRatio = Mathf.Pow(timeElasped / duration, 0.1f);
            this.textComponent.text = Mathf.Round(Mathf.Lerp(initialCount, this.displayedCount, elapsedRatio)).ToString();

            this.textInfoComponent.color = new Color(this.textInfoComponent.color.r, this.textInfoComponent.color.g, this.textInfoComponent.color.b, 1f - Mathf.Pow(Mathf.Abs(elapsedRatio - 0.5f) * 2f, 7f));
            this.textInfoComponent.rectTransform.anchoredPosition = infoAnimationStartingOffset * Mathf.Pow(1f-elapsedRatio, 2f);
            yield return null;
        }

        this.textComponent.text = this.displayedCount.ToString();
        this.animating = false;
        this.TryDequeueChanges();
    }
}
