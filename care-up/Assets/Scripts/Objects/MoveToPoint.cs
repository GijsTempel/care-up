using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPoint : MonoBehaviour {

    public float speed = 1.0f;
    public float delay = 0f;
    public List<GameObject> KyePoints;
    int NextPointIndex = 0;
    public bool toWalk = false;
    public string EndTriggerObjName = "";
    public List<string> EndTriggers;
    public string StartTrigger = "walk";
    public string StopTrigger = "stopwalk";
    public Animator personToMove;
   // public Animator playerToMove;
   // public string Trigger1ForPlayer;
   // public string Trigger2ForPlayer;
    bool currentwalk = false;


    public void SetEndTriggers(string ETObjName, List<string> ETriggers)
    {
        EndTriggerObjName = ETObjName;
        EndTriggers.Clear();
        foreach(string t in ETriggers)
        {
            EndTriggers.Add(t);
        }
    }

    public void SetKeyPoints(string PointHolderName)
    {
        if (GameObject.Find(PointHolderName) != null)
        {
            GameObject PointHolder = GameObject.Find(PointHolderName);
            if (PointHolder.GetComponent<ExtraObjectOptions>() != null)
            {
                KyePoints.Clear();
                foreach(GameObject g in PointHolder.GetComponent<ExtraObjectOptions>().hidenObjects)
                {
                    KyePoints.Add(g);
                }
                NextPointIndex = 0;
                toWalk = true;
                //if (NextPointIndex < KyePoints.Count)
                    //transform.LookAt(KyePoints[NextPointIndex].transform.position);
            }
        }
    }

    public void StartWalking()
    {
        //print("sssssssssss");
        NextPointIndex = 0;
        toWalk = true;
        //if (NextPointIndex < KyePoints.Count)
        //    transform.LookAt(KyePoints[NextPointIndex].transform.position);
       // playerToMove =  GameObject.FindObjectOfType<PlayerAnimationManager>().GetComponent<Animator>();
        personToMove.SetTrigger(StartTrigger);
       // playerToMove.SetTrigger(Trigger1ForPlayer);
       // playerToMove.SetTrigger(Trigger2ForPlayer);
    }

    // Update is called once per frame
    void Update ()
    {
        if(delay >0 && toWalk)
        {
            delay -= Time.deltaTime;
        }
        if (toWalk && delay < 0)
        {
           

            GameObject nextPoint = null;
            if (NextPointIndex < KyePoints.Count)
                nextPoint = KyePoints[NextPointIndex];

            if (currentwalk != toWalk) 
            {
                
                StartWalking();
                currentwalk = toWalk;
                return;

            }
          

            float step = speed * Time.deltaTime;

            if (nextPoint != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPoint.transform.position, step);
                Vector3 dir = (nextPoint.transform.position - transform.position).normalized;

                Quaternion angleTarget = Quaternion.LookRotation(dir);
                float pointDistance = Vector3.Distance(transform.position, nextPoint.transform.position);
                if ((NextPointIndex + 1 == KyePoints.Count) && pointDistance < 0.8f)
                {
                    angleTarget = nextPoint.transform.rotation;
                }

               
                transform.rotation = Quaternion.RotateTowards(transform.rotation, angleTarget, speed * 2f);

                if (Vector3.Distance(transform.position, nextPoint.transform.position) < 0.001f)
                {

                    transform.position = nextPoint.transform.position;
                    NextPointIndex++;
                    if (NextPointIndex < KyePoints.Count)
                    {
                        //    transform.LookAt(KyePoints[NextPointIndex].transform.position);
                    }
                    else
                    {
                        //if ((NextPointIndex - 1) < KyePoints.Count)
                        //    transform.rotation = KyePoints[NextPointIndex - 1].transform.rotation;
                        toWalk = false;

                        personToMove.SetTrigger(StopTrigger);
                        
                        if (GameObject.Find(EndTriggerObjName) && EndTriggers.Count > 0)
                        {
                            if (GameObject.Find(EndTriggerObjName).GetComponent<Animator>() != null)
                            {
                                Animator actor = GameObject.Find(EndTriggerObjName).GetComponent<Animator>();
                                int j = -999;

                                foreach (string t in EndTriggers)
                                {
                                    if (t != "")
                                    {
                                        string[] strArr = t.Split(char.Parse(" "));
                                        int.TryParse(strArr[0], out j);
                                        if (j != 0)
                                        {
                                            string intName = "";
                                            for (int i = 1; i < strArr.Length; i++)
                                            {
                                                if (i != 1)
                                                    intName += " ";
                                                intName += strArr[i];
                                            }
                                            //print(intName);
                                            actor.SetInteger(intName, j);
                                        }
                                        else
                                        {
                                            actor.SetTrigger(t);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }

        }
    }
}
