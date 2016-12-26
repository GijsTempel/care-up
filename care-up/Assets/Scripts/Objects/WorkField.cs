using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkField : UsableObject {

    public List<GameObject> objects = new List<GameObject>();

    private bool toggle = false;

    protected override void Start()
    {
        base.Start();

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
            toggle = !toggle;
            foreach (GameObject obj in objects)
            {
                if (obj)
                {
                    obj.SetActive(toggle);
                }
            }

            actionManager.OnUseAction(gameObject.name);
            Reset();
        }
    }

}
