using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.Animations;

#endif

public class AddClipToAnimator : MonoBehaviour {


    StreamWriter writer;

    public enum ActionType
    {
        Combine,
        Use
    };
#if UNITY_EDITOR
    public string subMachine = "";
    public AnimatorController animationController;
	public AnimationClip leftClip;
    public AnimationClip rightClip;
	public int LeftObjectsID = -1;
	public int RightbjectsID = -1;
    public ActionType actionType;

    [ContextMenu("Add animations")]
    public void Run()
    {
        string triger_base = "Combine";
        if (actionType == AddClipToAnimator.ActionType.Use)
        {
            triger_base = "Use";
        }

        string sm = "";
		if (subMachine != "")
			sm = subMachine;
        if (leftClip != null) 
        { 
            Motion LeftMotion = (Motion)leftClip as Motion;
            if (subMachine != "")
            {
                AddActionToMachine(0, "Combine Animations/" + sm, LeftMotion, LeftObjectsID, RightbjectsID, triger_base);
                AddActionToMachine(1, "Combine Animations/" + sm, LeftMotion, LeftObjectsID, RightbjectsID, "S " + triger_base);
            }
            else
            {
                AddActionToMachine(0, "", LeftMotion, LeftObjectsID, RightbjectsID, triger_base);
                AddActionToMachine(1, "", LeftMotion, LeftObjectsID, RightbjectsID, "S " + triger_base);
            }
	    }
        if (rightClip != null)
        {
            Motion RightMotion = (Motion)rightClip as Motion;
            if (subMachine != "")
            {
                AddActionToMachine(0, "Combine Animations/" + sm, RightMotion, RightbjectsID, LeftObjectsID, triger_base);
                AddActionToMachine(1, "Combine Animations/" + sm, RightMotion, RightbjectsID, LeftObjectsID, "S " + triger_base);
            }
            else
            {
                AddActionToMachine(0, "", RightMotion, RightbjectsID, LeftObjectsID, triger_base);
                AddActionToMachine(1, "", RightMotion, RightbjectsID, LeftObjectsID, "S " + triger_base);
            }
        }
    }

    // Прохід по всьому списку анімації в машині станів та заміна анімації на відповідно з іншого файлу
    // Go thru animation controller and replace animations with the same name from another file
    public GameObject testObject;

    [ContextMenu("Switch animation file")]
    public void SwitchAnimationFile()
    {
        ChildAnimatorState[] ch_animStates;
        AnimatorStateMachine stateMachine;



        foreach (AnimatorControllerLayer i in animationController.layers) //for each layer
        {
            stateMachine = i.stateMachine;
            ch_animStates = null;
            ch_animStates = stateMachine.states;
            foreach (ChildAnimatorState j in ch_animStates) //for each state
            {
                if (j.state.motion != null)
                {

                    Debug.Log((j.state.motion as AnimationClip).name);
                }
            }
        }

    }


    //-------------------------------------------------

    // Прохід по всьому списку анімації в машині станів та заміна анімації на відповідно з іншого файлу
    // Go thru animation controller and replace animations with the same name from another file

    [ContextMenu("Copy Right to Left")]
    public void CopyStatesFromLeftToRight()
    {
        ChildAnimatorState[] ch_animStates;
        AnimatorStateMachine stateMachine;

        stateMachine = animationController.layers[0].stateMachine;
        ch_animStates = null;
        ch_animStates = stateMachine.states;
        foreach (ChildAnimatorState j in ch_animStates) //for each state
        {
            string _name = j.state.name;
            UnityEditor.Animations.AnimatorState leftState = GetAnimationClip(_name, 1);
            if (leftState != null) {
                //leftState.
                print(j.position);
            }
            
        }
        

    }




    //-------------------------------------------------

    [ContextMenu("Test Animation Existence")]
    public void CheckAnimations()
    {
        // never used stuff
        //string[] AnimStateNames;
        //AnimatorControllerLayer[] acLayers;
        ChildAnimatorState[] ch_animStates;

        //Animator animator;
        //AnimatorController ac = animationController;
        AnimatorStateMachine stateMachine;
      

        foreach (AnimatorControllerLayer i in animationController.layers) //for each layer
        {

            Debug.Log("\nLayer : " + i.name);
            stateMachine = i.stateMachine;
            ch_animStates = null;
            ch_animStates = stateMachine.states;

            foreach (ChildAnimatorState j in ch_animStates) //for each state
            {
                if (j.state.motion == null)
                {
                    Debug.Log("No animation!!   " + j.state.name);
                }

            }

            foreach (ChildAnimatorStateMachine c1 in i.stateMachine.stateMachines)
            {
                CheckMachine(c1, c1.stateMachine.name + ".");

            }
        }

    }




    void CheckMachine(ChildAnimatorStateMachine m, string _path)
    {
        ChildAnimatorState[] ch_animStates;
        AnimatorStateMachine stateMachine;
        foreach (ChildAnimatorStateMachine c in m.stateMachine.stateMachines)
        {
            stateMachine = c.stateMachine;
            ch_animStates = null;
            ch_animStates = stateMachine.states;

            foreach (ChildAnimatorState j in ch_animStates) //for each state
            {
                if (!j.state.motion)
                {
                    Debug.Log("******* No animation!!   " + _path + c.stateMachine.name + "." + j.state.name);
                }
            }
            CheckMachine(c, _path + c.stateMachine.name +  ".");
        }
    }



    void AddActionToMachine(int layer, string machineName, Motion clip, int leftID, int rightID, string trigger)
    {
        AnimatorStateMachine am = animationController.layers[layer].stateMachine;
        if (machineName != "")
        {
            am = FindMachine(animationController.layers[layer].stateMachine, machineName);
        }
        
        UnityEditor.Animations.AnimatorState lm;
        lm = am.AddState(clip.name);
        lm.motion = clip;
        AnimatorStateMachine bm = animationController.layers[layer].stateMachine;
        bm.AddAnyStateTransition(lm);
        int l = bm.anyStateTransitions.Length;
        bm.anyStateTransitions[l - 1].AddCondition(AnimatorConditionMode.If, 1, trigger);
        if (leftID != -1)
            bm.anyStateTransitions[l - 1].AddCondition(AnimatorConditionMode.Equals, leftID, "leftID");
        if (rightID != -1)
            bm.anyStateTransitions[l - 1].AddCondition(AnimatorConditionMode.Equals, rightID, "rightID");
        if (machineName != "")
            lm.AddTransition(animationController.layers[layer].stateMachine);
        else
            lm.AddExitTransition();
        lm.transitions[lm.transitions.Length - 1].hasExitTime = true;
    }
    
    AnimatorStateMachine FindMachine(AnimatorStateMachine machine, string addr)
    {
        string n = addr;
        bool lastElement = true;
        string nextAddr = "";
        int addrLen = addr.Split('/').Length;
        if (addrLen > 1)
        {
            n = addr.Split('/')[0];
            lastElement = false;
            for (int i = 1; i < addrLen; i++)
            {
                nextAddr += addr.Split('/')[i];
                if (i < (addrLen - 1))
                    nextAddr += "/";
            }
        }
        foreach (ChildAnimatorStateMachine state in machine.stateMachines)
        {
            if (state.stateMachine.name == n)
            {
                AnimatorStateMachine sm = state.stateMachine;
                if (!lastElement)
                    return (FindMachine(sm, nextAddr));
                return sm;
            }
        }
        return null;
    }
    

    public UnityEditor.Animations.AnimatorState GetAnimationClip(string name, int _layer = 0)
    {
        if (!animationController) return null;
        foreach (UnityEditor.Animations.ChildAnimatorState state in animationController.layers[_layer].stateMachine.states)
        {
		if (state.state.name == name)
            {
		    return state.state;
            }
        }
        return null; // no clip by that name
    }


    //Output the all structure from animation controller to the file
    [ContextMenu("Print All Actions")]
    public void PrintAllActions()
    {

        string scneName = SceneManager.GetActiveScene().name;
        string path = "Assets/ListOfActions/full_" + scneName + ".txt";

        writer = new StreamWriter(path, false);

        if (!animationController)
            return;


        foreach (UnityEditor.Animations.ChildAnimatorState state in animationController.layers[0].stateMachine.states)
        {
            string[] aa = AssetDatabase.GetAssetPath(state.state.motion.GetInstanceID()).Split('/');
            writer.WriteLine(aa[aa.Length - 1] + " __ " + '"' + "RightHand." + state.state.name + '"' + ",");
        }

        foreach (UnityEditor.Animations.ChildAnimatorStateMachine machine in animationController.layers[0].stateMachine.stateMachines)
        {
            RecursPrintStates(machine, "RightHand");
        }
        writer.Close();
    }

    public void RecursPrintStates(UnityEditor.Animations.ChildAnimatorStateMachine chS, string prevAddr)
    {
        
        foreach (UnityEditor.Animations.ChildAnimatorState state in chS.stateMachine.states)
        {
            
            string[] aa = AssetDatabase.GetAssetPath(state.state.motion.GetInstanceID()).Split('/');
            writer.WriteLine(aa[aa.Length - 1] + " __ " + '"' + prevAddr + "." + chS.stateMachine.name + "." + state.state.name + '"' + ",");
        }
        foreach (UnityEditor.Animations.ChildAnimatorStateMachine machine in chS.stateMachine.stateMachines)
        {
            string pa = "";
            if (prevAddr != "")
                pa = prevAddr + ".";
            RecursPrintStates(machine, pa + chS.stateMachine.name);


        }

    }



#endif
}


