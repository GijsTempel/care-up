using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllMaterialOffsetOnAnimation : StateMachineBehaviour
{
    public string objectToControl;
    private GameObject obj;
   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        obj = GameObject.Find(objectToControl);
        if(obj!=null)
        {
            obj.GetComponent<MaterialOffsetController>().offsetControll = true;
        }
        //GameObject.GetComponent<MaterialOffsetController>()
    }
     
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

   
}
