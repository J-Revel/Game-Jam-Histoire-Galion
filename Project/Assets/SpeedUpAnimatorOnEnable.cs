using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpAnimatorOnEnable : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float speed;

    private void OnEnable()
    {
        animator.speed = speed;
    }
}
