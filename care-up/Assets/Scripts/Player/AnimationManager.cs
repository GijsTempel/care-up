using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimationManager : MonoBehaviour {

    private Animator animationController;

    void Start()
    {
        animationController = GetComponent<Animator>();
        if (animationController == null) Debug.LogError("Animator not found");
    }

    public void PlayAnimation(string name)
    {
        animationController.Play(name);
    }
}
