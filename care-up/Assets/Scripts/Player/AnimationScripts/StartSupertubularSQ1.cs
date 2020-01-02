using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSupertubularSQ1 : StateMachineBehaviour
{
    public string objectName;
    GameObject obj = null;
    Animator PlayerAnim;
   // private GameObject InteractableObjects;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        obj = GameObject.Find(objectName);
        PlayerAnim = GameObject.FindObjectOfType<PlayerAnimationManager>().GetComponent<Animator>();
       // InteractableObjects = GameObject.Find("Interactable Objects");

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (obj != null)
        {
            if (obj.transform.parent.name == "Interactable Objects") 
            {
                if (PlayerAnim.GetBool("AllowSQ1"))
                {
                    PlayerAnim.SetTrigger("SQ1Sequence");
                    Debug.Log("IsParent" + obj.transform.parent.ToString() + " " + "AnimatorTrigger " + PlayerAnim.GetBool("AllowSQ1").ToString());
                }
                else
                {
                    Debug.Log("Error!!!" + "IsParent: " + obj.transform.parent.ToString() + " " + "AnimatorTrigger: " + animator.GetBool("AllowSQ1").ToString());
                }
            }
        }

       
    }

    
}
