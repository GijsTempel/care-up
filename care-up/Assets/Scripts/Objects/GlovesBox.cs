using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlovesBox : UsableObject
{

    public override void Use()
    {
        if (!ViewModeActive())
        {
            if (actionManager.CompareUseObject(name))
            {
                PlayerAnimationManager.PlayAnimation("Use " + name);
                tutorial_used = true;
            }

            actionManager.OnUseAction(gameObject.name);

            Reset();
        }
    }
}
