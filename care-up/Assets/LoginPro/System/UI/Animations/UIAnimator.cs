using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LoginProAsset
{
    public class UIAnimator : MonoBehaviour
    {
        public List<UIAnimation> Animations;

        public void OneAfterTheOther()
        {
            StartCoroutine(LaunchOneAfterTheOther(Animations));
        }

        private IEnumerator LaunchOneAfterTheOther(List<UIAnimation> launchThoseAnimationsOneAfterTheOther)
        {
            // Launch all animations passed as parameters
            foreach (UIAnimation anim in launchThoseAnimationsOneAfterTheOther)
            {
                yield return anim.Launch();
            }

            yield return null;
        }
    }
}