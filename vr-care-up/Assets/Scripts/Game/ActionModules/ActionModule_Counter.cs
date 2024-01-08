using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionModule_Counter : MonoBehaviour
{
    public ActionTrigger actionTrigger;
    public int maxCountedValue = 1;
    public bool oneShot = true;
    private int counterValue = 0;

    public void Execute()
    {
        counterValue += 1;
        if (counterValue >= maxCountedValue)
            ExecuteTrigger();
    }

    void ExecuteTrigger()
    {
        actionTrigger.AttemptTrigger();
        if (oneShot)
            Destroy(gameObject);
    }
}
