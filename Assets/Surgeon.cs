using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surgeon : MonoBehaviour
{
    public enum EnemyVariant
    {
        slide, sweep, follow
    }
    public EnemyVariant variant;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        switch (variant)
        {
            case EnemyVariant.sweep:
                animator.SetTrigger("sweep");
                break;
            case EnemyVariant.slide:
                animator.SetTrigger("slide");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
