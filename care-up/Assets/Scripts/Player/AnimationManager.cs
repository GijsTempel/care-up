using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour {

    private Animation anim;

    public void PlayAnimation(string name)
    {
        anim.Play(name, PlayMode.StopSameLayer);
    }
}
