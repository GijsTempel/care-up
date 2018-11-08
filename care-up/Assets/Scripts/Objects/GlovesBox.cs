using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlovesBox : UsableObject
{
    public bool oneTimeUse = false;

    public override void Use()
    {
        if (!ViewModeActive())
        {
            if (actionManager.CompareUseObject(name))
            {
                PlayerAnimationManager.PlayAnimation("Use gloveBox");
                tutorial_used = true;

                if (oneTimeUse)
                {
                    Destroy(this.gameObject);
                }
            }

            actionManager.OnUseAction(gameObject.name);
            Reset();
        }
    }
}
