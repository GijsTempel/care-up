using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class syringPack : PickableObject
{

    public float OpeningState = 0.0f;
    public float OpeningStateSaved = 0.0f;

    public bool UpdateOpeningState = false;

    Animator animator;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        animator.speed = 0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (UpdateOpeningState)
        {
            OpeningStateSaved = OpeningState;
            animator.Play("syringPackArm|0001_open", 0, OpeningStateSaved);
        }
    }

    public override bool Drop(bool force = false)
    {
        base.Drop(force);
        UpdateOpeningState = false;

        return false;
    }

    public override void Pick()
    {
        base.Pick();
        UpdateOpeningState = true;
    }
}
