using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceAnswerMovement : MonoBehaviour
{
    public float forceIntensity;
    public float attractionIntensity;
    public float friction;
    public Vector2 velocity;
    public Vector2 targetPosition;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        targetPosition = rectTransform.anchoredPosition;

    }

    private void Update()
    {
        velocity += Random.insideUnitCircle * Time.deltaTime * forceIntensity;
        Vector2 direction = (targetPosition - rectTransform.anchoredPosition);
        float distance = direction.magnitude;
        velocity += direction * attractionIntensity * Time.deltaTime;
        velocity = velocity * Mathf.Pow(friction, Time.deltaTime);
        rectTransform.anchoredPosition += velocity * Time.deltaTime;
    }
    
}