using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour {

    public GameObject mountBone;

    private Animator animationController;

    void Start()
    {
        animationController = GetComponentInChildren<Animator>();
        if (animationController == null) Debug.LogError("Animator not found");
    }

    public void PlayAnimation(string name, GameObject interact = null)
    {
        animationController.SetTrigger(name);
        if ( interact != null )
        {
            mountBone.transform.position = interact.transform.position;
        }
    }
}
