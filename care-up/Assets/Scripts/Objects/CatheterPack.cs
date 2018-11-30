using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatheterPack : UsableObject
{
    public List<GameObject> objects = new List<GameObject>();
    
    protected override void Start()
    {
        base.Start();
        
        foreach (GameObject obj in objects)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    public override void Use()
    {
        if (!ViewModeActive() && actionManager.CompareUseObject(name))
        {
            if (actionManager.CompareUseObject(name))
            {
                PlayerAnimationManager.PlayAnimation("Use " + name);//, transform); add this with animation

                foreach (GameObject obj in objects)
                {
                    if (obj)
                    {
                        obj.SetActive(true);
                    }
                }

                Destroy(gameObject);
            }

            actionManager.OnUseAction(name);
            controls.ResetObject();
            Reset();
        }
    }
}
