using UnityEngine;
using System.Collections;

/// <summary>
/// Handle animation changes of the player
/// </summary>
public class PlayerAnimationManager : MonoBehaviour {

    public float ikWeight = 1.0f;

    public static bool ikActive = false;

    private static Transform leftInteractObject;
    private static Transform rightInteractObject;

    private static Animator animationController;
    private static CameraMode cameraMode;

    void Start()
    {
        animationController = GetComponent<Animator>();
        if (animationController == null) Debug.LogError("Animator not found");

        cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
        if (cameraMode == null) Debug.LogError("No camera mode");
    }

    private void Update()
    {
        if (animationController.GetNextAnimatorStateInfo(0).fullPathHash
            == Animator.StringToHash("Base Layer.Armature|IdleN"))
            cameraMode.animationEnded = true;
    }

    public static void PlayAnimation(string name, Transform target = null)
    {
        animationController.SetTrigger(name);

        if (target)
        {
            cameraMode.SetCinematicMode(target);
        }
    }
}
