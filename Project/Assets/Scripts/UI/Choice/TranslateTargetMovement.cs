using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateTargetMovement : MonoBehaviour
{
    public Vector3 startPos;
    public Transform target;
    public float transitionTime;
    public float transitionDuration = 0.5f;
    public bool show = false;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        transitionTime += Time.deltaTime * (show ? 1:-1);
        transitionTime = Mathf.Clamp(transitionTime, 0, transitionDuration);
        float t = transitionTime / transitionDuration;
        t = 1 - (1-t)*(1-t);
        transform.position = Vector3.Lerp(startPos, target.position, t);
    }
    
    public void SetTarget(Transform target)
    {
        startPos = transform.position;
        this.target = target;
        transitionTime = 0;
    }
}