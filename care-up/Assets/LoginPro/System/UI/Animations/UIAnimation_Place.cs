using UnityEngine;
using System.Collections;
using System;

namespace LoginProAsset
{
    /*
    public class UIAnimation_Place : UIAnimation
    {
        public bool currentlyLaunched = false;
        public PlaceUIElement ElementToMove;

        public float HorizontalDestination = 0;
        public float VerticalDestination = 0;
        public float PortraitHorizontalDestination = 0;
        public float PortraitVerticalDestination = 0;

        // Properties to access the right configuration depending on the configuration
        public float GetHorizontalDestination
        {
            get
            {
                return PlaceCanvas.IsLandscape() ? this.HorizontalDestination : this.PortraitHorizontalDestination;
            }
        }

        public float GetVerticalDestination
        {
            get
            {
                return PlaceCanvas.IsLandscape() ? this.VerticalDestination : this.PortraitVerticalDestination;
            }
        }

        [Range(1, 1000)]
        public int Speed = 1;
        public int Duration = 0;

        protected override IEnumerator Play()
        {
            if (!currentlyLaunched)
            {
                this.currentlyLaunched = true;

                // The distance to do to achieve the destination
                float horizontalDistanceToDo = Math.Abs(this.ElementToMove.horizontalPosition - GetHorizontalDestination);
                float verticalDistanceToDo = Math.Abs(this.ElementToMove.verticalPosition - GetVerticalDestination);

                // Factor to know if distance must be added or withdraw
                float horizontalFactor = GetHorizontalDestination < this.ElementToMove.horizontalPosition ? -1 : 1;
                float verticalFactor = GetVerticalDestination < this.ElementToMove.verticalPosition ? -1 : 1;

                int counter = 1;
                int totalSteps = 1000 / Speed;

                float distanceToApplyHorizontally = horizontalDistanceToDo / totalSteps;
                float distanceToApplyVertically = verticalDistanceToDo / totalSteps;

                // Finish when the percent to add is more than one
                while (counter < totalSteps)
                {
                    yield return new WaitForEndOfFrame();

                    // Place the UI element
                    this.ElementToMove.horizontalPosition += distanceToApplyHorizontally * horizontalFactor;
                    this.ElementToMove.verticalPosition += distanceToApplyVertically * verticalFactor;
                    this.ElementToMove.portraitHorizontalPosition += distanceToApplyHorizontally * horizontalFactor;
                    this.ElementToMove.portraitVerticalPosition += distanceToApplyVertically * verticalFactor;
                    this.ElementToMove.Place();

                    counter++;
                }

                // Place the UI element
                this.ElementToMove.horizontalPosition = HorizontalDestination;
                this.ElementToMove.verticalPosition = VerticalDestination;
                this.ElementToMove.portraitHorizontalPosition = PortraitHorizontalDestination;
                this.ElementToMove.portraitVerticalPosition = PortraitVerticalDestination;

                this.ElementToMove.Place();

                // Wait during some seconds
                yield return new WaitForSeconds(this.Duration);

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
            }
            yield return null;
        }
    }*/
}