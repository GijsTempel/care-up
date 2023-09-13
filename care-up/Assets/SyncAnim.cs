using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncAnim : MonoBehaviour
{

    protected Animator PlayerAnimator;
    public Animator MasterObject;
    public Animator SlaveObject;
    public string AnimName;
    public bool IsSyncing;
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GameObject.FindObjectOfType<PlayerAnimationManager>().GetComponent<Animator>();
        if (MasterObject==null)
        {
            MasterObject = PlayerAnimator;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        if(IsSyncing)
        {
            if (SlaveObject)
            {
                float nTime = Mathf.Repeat(MasterObject.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f);
                if (MasterObject.GetAnimatorTransitionInfo(0).normalizedTime == 0f)
                    SlaveObject.Play(AnimName, -1, nTime);
            }
        }
        
    }

}
