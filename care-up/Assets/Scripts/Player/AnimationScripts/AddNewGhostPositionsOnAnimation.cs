using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AddNewGhostPositionsOnAnimation : StateMachineBehaviour
{
    [System.Serializable]
    public struct GhostPositionData
    {
        public string objectName;
        public string TargetName;
        public int id;
    };

    private List<HandsInventory.GhostPosition> list = new List<HandsInventory.GhostPosition>();
    public List<GhostPositionData> dataList = new List<GhostPositionData>();
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (GhostPositionData d in dataList)
        {
            if (GameObject.Find(d.TargetName) != null)
            {
                Transform targetTransform = GameObject.Find(d.TargetName).transform;

                Vector3 pos = targetTransform.position;
                Vector3 rot = targetTransform.rotation.eulerAngles;
                HandsInventory.GhostPosition listItem = new HandsInventory.GhostPosition();
                listItem.id = d.id;
                listItem.position = pos;
                listItem.rotation = rot;
                listItem.objectName = d.objectName;
                list.Add(listItem);
            }
        }
        
        HandsInventory inv = GameObject.FindObjectOfType<HandsInventory>();
        
        foreach (HandsInventory.GhostPosition i in list)
        {
            inv.customGhostPositions.Add(i);
        }
    }
}
