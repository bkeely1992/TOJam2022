using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidSpewer : MonoBehaviour
{
    private Animator animator;
    public void Activate()
    {
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }

        animator.enabled = true;
    }
}
