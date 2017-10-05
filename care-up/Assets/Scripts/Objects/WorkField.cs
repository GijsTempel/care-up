using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkField : UsableObject {
    
    public List<GameObject> objects = new List<GameObject>();
    public List<GameObject> postObjects = new List<GameObject>();

    private int toggleTime = 0;
    private bool toggle = false;

    protected override void Start()
    {
        base.Start();

        toggleTime = 0;
        toggle = false;

        foreach (GameObject obj in objects)
        {
            obj.SetActive(toggle);
        }

        foreach (GameObject obj in postObjects)
        {
            obj.SetActive(toggle);
        }
    }

    public override void Use()
    {
        if (!ViewModeActive())
        {
            if (actionManager.CurrentUseObject != name)
            {
                actionManager.OnGameOver();
            }

            tutorial_used = true;
            PlayerAnimationManager.PlayAnimation("Use WorkField", transform);
            actionManager.OnUseAction(gameObject.name);
            controls.ResetObject();
            Reset();

            if ( toggleTime == 1 )
            {
                ToggleObjects();
            }

            tutorial_used = true;
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
        else
        {
            foreach (GameObject obj in postObjects)
            {
                if (obj)
                {
                    obj.SetActive(true);
                }
            }
        }
    }
}
