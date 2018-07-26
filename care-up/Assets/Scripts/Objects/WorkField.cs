using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkField : UsableObject {
    
    public List<GameObject> objects = new List<GameObject>();

    private int toggleTime = 0;
    private bool toggle = false;

    public bool cleaningLocked = true;

    protected override void Start()
    {
        base.Start();

        toggleTime = 0;
        toggle = false;

        foreach (GameObject obj in objects)
        {
            obj.SetActive(toggle);
        }
    }

    public override void Use()
    {
        if (!ViewModeActive())
        {
            if (toggleTime == 1 && cleaningLocked)
            {
                // lock 2nd cleaning
                return;
            }

            if (actionManager.CompareUseObject(name))
            {
                tutorial_used = true;
                PlayerAnimationManager.PlayAnimation("Use WorkField", transform);

                if (toggleTime == 0)
                {
                    PlayerScript.TriggerQuizQuestion(5f);
                }

                if (toggleTime == 1)
                {
                    ToggleObjects();
                }
            }

            actionManager.OnUseAction(gameObject.name);
            controls.ResetObject();
            Reset();
        }
    }

    public void ToggleObjects()
    {
        if (toggleTime < 2)
        {
            toggle = !toggle;
            foreach (GameObject obj in objects)
            {
                if (obj)
                {
                    obj.SetActive(toggle);
                }
            }
            
            ++toggleTime;
        }
    }
}
