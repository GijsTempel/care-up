using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClothObject : PickableObject {

    public Mesh basic;
    public Mesh inHand;
    public Mesh folded;

    protected override void Start()
    {
        base.Start();

        if (SceneManager.GetActiveScene().name == "Injection Subcutaneous_ampoule")
        {
            holdAnimationID = 8;
        }
        else
        {
            holdAnimationID = 6;
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

        if (SceneManager.GetActiveScene().name == "Injection Subcutaneous_ampoule")
        {
            if (folded)
            {
                GetComponent<MeshFilter>().mesh = folded;
                holdAnimationID = 8;
            }
        }
        else if (inHand)
        {
            GetComponent<MeshFilter>().mesh = inHand;
            holdAnimationID = 6;
        }
    }
}
