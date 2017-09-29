using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LoginProAsset
{
    public class UIAnimation : MonoBehaviour
    {
        public List<UIAnimation> AnimationToLaunchWhenFinish;

        public Coroutine Launch()
        {
            // Start animation
            return StartCoroutine(Play());
        }
        public Coroutine Stop()
        {
            // Stop animation
            return StartCoroutine(End());
        }

        protected virtual IEnumerator Play()
        {
            yield return null;
        }

        protected virtual IEnumerator End()
        {
            yield return null;
        }
    }
}
