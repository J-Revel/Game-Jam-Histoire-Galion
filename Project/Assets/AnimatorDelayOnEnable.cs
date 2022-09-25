using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimatorDelayOnEnable : MonoBehaviour
{
    [SerializeField]
    private float offsetTime;
    [SerializeField]
    private Animator animator;
    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(this.DelayActivation());
    }

    IEnumerator DelayActivation()
    {
        //Init
        animator.enabled = false;
        float timeElasped = 0f;

        // Wait before animation
        while (timeElasped < this.offsetTime)
        {
            timeElasped += Time.deltaTime;
            yield return null;
        }

        animator.enabled = true;
    }
}
