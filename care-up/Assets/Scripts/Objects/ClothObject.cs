using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClothObject : PickableObject {

    public Mesh basic;
    public Mesh inHand;
    public Mesh folded;

    public enum ClothHoldState
    {
        Crumpled,
        Folded
    };

    public ClothHoldState state = ClothHoldState.Crumpled;

    protected override void Start()
    {
        base.Start();

        if (state == ClothHoldState.Crumpled)
        {
            holdAnimationID = 6;
        }
        else if (state == ClothHoldState.Folded)
        {
            holdAnimationID = 8;
        }
    }

    public override bool Drop(bool force = false)
    {
        if (basic)
        {
            GetComponent<MeshFilter>().mesh = basic;
        }

        return base.Drop(force);
    }

    public override void Pick()
    {
        base.Pick();

        if (state == ClothHoldState.Folded && folded != null)
        {
            GetComponent<MeshFilter>().mesh = folded;
        }
        else if (state == ClothHoldState.Crumpled && inHand != null)
        {
            GetComponent<MeshFilter>().mesh = inHand;
        }
    }
}
