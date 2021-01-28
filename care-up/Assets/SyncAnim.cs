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
        //SlaveObject.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsSyncing)
        {
            if (SlaveObject)
            {
                SlaveObject.Play(AnimName, -1, MasterObject.GetCurrentAnimatorStateInfo(0).normalizedTime);
            }
        }
        
    }

}
