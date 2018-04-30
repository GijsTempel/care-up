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

        ChangeState(state);
    }

    public override bool Drop(bool force = false)
    {
        if (GameObject.Find("ClothPackage") == null
            || name != "Cloth")
        {
            if (basic)
            {
                GetComponent<MeshFilter>().mesh = basic;
            }

            return base.Drop(force);
        }
        else
        {
            Destroy(gameObject);
            return true;
        }
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

    public void ChangeState(ClothHoldState s, bool forceChangeMesh = false)
    {
        state = s;
        if (state == ClothHoldState.Crumpled)
        {
            holdAnimationID = 6;
            if (forceChangeMesh && inHand != null)
            {
                GetComponent<MeshFilter>().mesh = inHand;
            }
        }
        else if (state == ClothHoldState.Folded)
        {
            holdAnimationID = 8;
            if (forceChangeMesh && folded != null)
            {
                GetComponent<MeshFilter>().mesh = folded;
            }
        }
    }
}
