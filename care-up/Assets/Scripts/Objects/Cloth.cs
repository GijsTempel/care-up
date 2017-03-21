using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloth : PickableObject {

    public Mesh basic;
    public Mesh inHand;

    public Vector3 basicScale;
    
    public override void Drop(bool force = false)
    {
        base.Drop(force);

        GetComponent<MeshFilter>().mesh = basic;
        transform.localScale = basicScale;
    }

    public override void Pick()
    {
        base.Pick();

        GetComponent<MeshFilter>().mesh = inHand;
        transform.localScale = Vector3.one;
    }
}
