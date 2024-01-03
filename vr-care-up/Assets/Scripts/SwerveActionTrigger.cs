using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


public class SwerveActionTrigger : MonoBehaviour
{
    public Image progressImage;
    public ActionTrigger actionTrigger;
    public int numberOfSwerves = 10;
    public ActionExpectant actionExpectant;
    public AudioSource smallBell;
    public float waitTime = 2;
    bool actionStarted = false;
    List<SwerveCollider> swerveColliders = new List<SwerveCollider>();
    Animator animator;

    public string swerveObjectName = "";
    int currentSwervColliderIndex = 0;
    int swervesCounted = 0;
    bool UIOpen = false;

    public ActionTrigger firstContactActionTrigger;

    void Start()
    {
        animator = GetComponent<Animator>();
        foreach(SwerveCollider s in transform.GetComponentsInChildren<SwerveCollider>())
        {
            swerveColliders.Add(s);
            s.gameObject.SetActive(false);
        }
    }

    void StartAction()
    {
        actionStarted = true;
        GameObject swerveObj = GameObject.Find(swerveObjectName);
        if (swerveObj == null)
            return;
        Vector3 newPos = swerveObj.transform.position;
        newPos.y = transform.position.y;
        transform.position = newPos;
        currentSwervColliderIndex = 0;
        ShowCurrentSwerveCollider();
    }


    void ShowCurrentSwerveCollider()
    {
        for(int i = 0; i < swerveColliders.Count; i++)
            swerveColliders[i].gameObject.SetActive(i == currentSwervColliderIndex);
    }


    public void SwerveCollision(SwerveCollider swerveCollider, string objName)
    {
        if (!actionStarted)
            return;
        if (objName != swerveObjectName)
            return;
        if (swerveCollider != swerveColliders[currentSwervColliderIndex])
            return;
        CountSwerve();
    }

    void CountSwerve()
    {

        if (!UIOpen)
        {
            VRCollarHolder vRCollarHolder = GameObject.FindObjectOfType<VRCollarHolder>();
            if (vRCollarHolder != null)
                vRCollarHolder.CloseTutorialShelf();                
            UIOpen = true;
            animator.SetTrigger("open");
        }
        if (swervesCounted == 0)
            firstContactActionTrigger.AttemptTrigger();
        swervesCounted++;
        int nextSwervColliderIndex = currentSwervColliderIndex + 1;
        if (nextSwervColliderIndex >= swerveColliders.Count)
            nextSwervColliderIndex = 0;
        currentSwervColliderIndex = nextSwervColliderIndex;
        progressImage.fillAmount = (float)swervesCounted / (float)(numberOfSwerves + 1);
        if (swervesCounted > numberOfSwerves)
        {
            if (actionTrigger.AttemptTrigger())
            {
                UIOpen = false;
                animator.SetTrigger("close");
                gameObject.SetActive(false);
                return;
            }
        }
        else
            smallBell.Play();
        ShowCurrentSwerveCollider();

    }

    // Update is called once per frame
    void Update()
    {
        if (!actionStarted && actionExpectant.isCurrentAction)
        {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                StartAction();
            }
        }
        
    }
}
