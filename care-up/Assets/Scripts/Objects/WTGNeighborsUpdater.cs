using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WTGNeighborsUpdater : MonoBehaviour
{
    [System.Serializable]
    public class WTGState
    {
        public WalkToGroup WTG;
        public WalkToGroup LeftNeighbord;
        public WalkToGroup RightNeighbord;
    }
    public List<WTGState> NextNeighborsStatus;

    public void Activate()
    {
        foreach(WTGState w in NextNeighborsStatus)
        {
            if (w.WTG != null)
            {
                w.WTG.LeftWalkToGroup = w.LeftNeighbord;
                w.WTG.RightWalkToGroup = w.RightNeighbord;
            }
        }
    }
   
}
