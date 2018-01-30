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
                if (GameObject.FindObjectOfType<PlayerPrefsManager>() != null &&
                    GameObject.FindObjectOfType<PlayerPrefsManager>().practiceMode == false)
                {
                    actionManager.OnGameOver();
                }
                else
                {
                    actionManager.OnUseAction(gameObject.name);
                    controls.ResetObject();
                    Reset();
                }
            }
            else
            {
                if (handsInventory.LeftHandEmpty() && handsInventory.RightHandEmpty())
                {
                    tutorial_used = true;
                    PlayerAnimationManager.PlayAnimation("Use WorkField", transform);
                    actionManager.OnUseAction(gameObject.name);
                    controls.ResetObject();
                    Reset();

                    if (toggleTime == 1)
                    {
                        ToggleObjects();
                    }

                    tutorial_used = true;
                }
                else
                {
                    string message = "Sometimes hands should be empty in order to perform certain actions. For example you need to have both hands free to clean the workfield.";
                    Camera.main.transform.Find("UI").Find("EmptyHandsWarning").
                            GetComponent<TimedPopUp>().Set(message);
                }
            }
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
