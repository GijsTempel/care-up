using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatheterPack : UsableObject
{
    public List<GameObject> objects = new List<GameObject>();
    
    private bool toggle = false;

    protected override void Start()
    {
        base.Start();
        
        toggle = false;

        foreach (GameObject obj in objects)
        {
            if (obj != null)
                obj.SetActive(toggle);
        }
    }

    public override void Use()
    {
        if (!ViewModeActive() && actionManager.CompareUseObject(name))
        {
            if (actionManager.CompareUseObject(name))
            {
                PlayerAnimationManager.PlayAnimation("Use " + name, transform);

                ToggleObjects();
            }

            actionManager.OnUseAction(name);
            controls.ResetObject();
            Reset();
        }
    }

    public void ToggleObjects()
    {
        toggle = !toggle;
        foreach (GameObject obj in objects)
        {
            if (obj)
            {
                obj.SetActive(toggle);
            }
        }
    }
}
