using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace LoginProAsset
{
    public class UIAnimation_Rotate : UIAnimation
    {
        public bool currentlyLaunched = false;
        public RectTransform ElementToRotate;

        [Range(0.1f, 360f)]
        public float Speed = 0.1f;

        protected override IEnumerator Play()
        {
            if (!currentlyLaunched)
            {
                this.currentlyLaunched = true;

                if (this.ElementToRotate == null)
                    Debug.LogError(string.Format("No element to rotate in the UI animation of the element : ", transform.gameObject.name));

                float rotationCounter = 0;
                while (rotationCounter < 360)
                {
                    this.ElementToRotate.Rotate(Vector3.up, Speed);
                    yield return new WaitForEndOfFrame();
                    rotationCounter += Speed;
                }

                // Don't forget to set the animation state to "finished"
                this.currentlyLaunched = false;

                // Launch all animations configured in editor
                if (this.AnimationToLaunchWhenFinish != null)
                {
                    foreach (UIAnimation anim in this.AnimationToLaunchWhenFinish)
                    {
                        anim.Launch();
                    }
                }
                else
                {
                    // Reinitialize rotation of the element
                    this.ElementToRotate.rotation = Quaternion.identity;
                }
            }
            yield return null;
        }

        protected override IEnumerator End()
        {
            // Clear the loop
            this.AnimationToLaunchWhenFinish = null;
            yield return null;
        }
    }
}