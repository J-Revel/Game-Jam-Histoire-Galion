using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceAnswerMovement : MonoBehaviour
{
    public float forceIntensity;
    public float attractionIntensity;
    public float friction;
    private Vector2 velocity;
    public Vector2 maxTargetOffset;
    private RectTransform rectTransform;
    public float maxRotation;
    private float rotationSpeed;
    public float rotationForceIntensity;
    public float attractionRotationForce;
    public float rotationFriction;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        
    }

    private void Update()
    {
        Vector2 targetPosition = Vector2.zero;
        velocity += Random.insideUnitCircle * Time.deltaTime * forceIntensity;
        Vector2 direction = (targetPosition - rectTransform.anchoredPosition);
        if(direction.x > 0)
            direction.x = Mathf.Max(direction.x - maxTargetOffset.x, 0);
        if(direction.x < 0)
            direction.x = Mathf.Min(direction.x + maxTargetOffset.x, 0);
        if(direction.y > 0)
            direction.y = Mathf.Max(direction.y - maxTargetOffset.y, 0);
        if(direction.y < 0)
            direction.y = Mathf.Min(direction.y + maxTargetOffset.y, 0);
        float distance = direction.magnitude;
        velocity += direction * attractionIntensity * Time.deltaTime;
        velocity = velocity * Mathf.Pow(friction, Time.deltaTime);
        rotationSpeed += Random.Range(-1.0f, 1.0f) * Time.deltaTime * rotationForceIntensity;
        float angle = rectTransform.rotation.eulerAngles.z;
        if(angle > 180)
            angle -= 360;
        float distanceToMax = Mathf.Max(0, Mathf.Abs(angle) - maxRotation);
        rotationSpeed -= Time.deltaTime * distanceToMax * attractionRotationForce * Time.deltaTime * Mathf.Sign(angle);
        rotationSpeed = rotationSpeed * Mathf.Pow(rotationFriction, Time.deltaTime);
        rectTransform.rotation *= Quaternion.Euler(0, 0, rotationSpeed);
        rectTransform.anchoredPosition += velocity * Time.deltaTime;
    }
    
}