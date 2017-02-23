using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour {

    private Animator animationController;

    void Start()
    {
        animationController = GetComponentInChildren<Animator>();
        if (animationController == null) Debug.LogError("Animator not found");
    }

    public void PlayAnimation(string name)
    {
        animationController.SetTrigger(name);
    }
}
