using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyePointerElement : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void CorrectAction()
    {
        animator.SetTrigger("correct");
    }

    public void IncorrectAction()
    {
        animator.SetTrigger("incorrect");
    }
}
