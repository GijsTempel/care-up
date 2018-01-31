using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace LoginProAsset
{
    /*
    public class UIAnimation_Alert : UIAnimation
    {
        public static string Message = "";
        public Text MessageField = null;



        [Range(1, 1000)]
        public int Speed = 1;

        [Range(0, 100)]
        public int MessageDuration = 5;

        public UIAnimation_Alert HideAnimation = null;

        public static Guid IdOfLastCoroutine = Guid.Empty;

        public Coroutine Show(string message, int duration)
        {
            // Set message
            UIAnimation_Alert.Message = message;

            if (duration >= 0 && duration <= 100)
                this.MessageDuration = duration;

            // Start animation
            return StartCoroutine(Play());
        }

        protected override IEnumerator Play()
        {
            Guid instanceId = Guid.NewGuid();
            IdOfLastCoroutine = instanceId;

            //if (this.MessageField == null)
              //  Debug.LogError("[UIAnimation_Alert] MessageField is null.");

            // Set the text of the message
            //this.MessageField.text = UIAnimation_Alert.Message;

            int counter = 1;
            int totalSteps = 1000 / Speed;


            // Finish when the percent to add is more than one
            while (counter < totalSteps)
            {
                yield return new WaitForEndOfFrame();

                counter++;
            }

            // Launch all animations configured in editor
            if (this.AnimationToLaunchWhenFinish != null)
            {
                foreach (UIAnimation anim in this.AnimationToLaunchWhenFinish)
                {
                    anim.Launch();
                }
            }

            // Leave the message shown for some seconds
            yield return new WaitForSeconds(this.MessageDuration);

            // Hide it ONLY IF the id of the last coroutine showing a message is our instance
            // Otherwise the last coroutine will take care of the hiding
            if (this.HideAnimation != null && IdOfLastCoroutine == instanceId)
                this.HideAnimation.Show("", 0);

            // End
            yield return null;
        }
    }*/
}