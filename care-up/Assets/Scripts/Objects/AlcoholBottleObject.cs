﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlcoholBottleObject : PickableObject
{
    public Mesh basic;
    public Mesh inHand;

    public override bool Drop(bool force = false, Transform forcedTransform = null, bool forceParent = false)
    {
        if (basic)
        {
            GetComponent<MeshFilter>().mesh = basic;
        }

        return base.Drop(force, forcedTransform, forceParent);
    }

    public override void Pick()
    {
        base.Pick();

        if (inHand)
        {
            GetComponent<MeshFilter>().mesh = inHand;
        }
    }
}
