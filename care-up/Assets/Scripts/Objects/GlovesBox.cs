using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlovesBox : UsableObject
{
    public override void Use()
    {
        base.Use();

        handsInventory.GlovesToggle(true);
    }
}
