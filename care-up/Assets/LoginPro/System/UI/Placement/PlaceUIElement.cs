using System;
using System.Collections.Generic;
using UnityEngine;

namespace LoginProAsset
{
    
    public class PlaceUIElement : UIElement
    {
        /*
        public float horizontalPosition = 0;
        public float verticalPosition = 0;
        public float horizontalSize = 0;
        public float verticalSize = 0;

        public bool AllowPortrait = false;

        public float portraitHorizontalPosition = 0;
        public float portraitVerticalPosition = 0;
        public float portraitHorizontalSize = 0;
        public float portraitVerticalSize = 0;

        private float oldHorizontalPosition = 0;
        private float oldVerticalPosition = 0;
        private float oldHorizontalSize = 0;
        private float oldVerticalSize = 0;

        private DateTime lastRefreshDate;

        private UIElement parent;

        // If the UI element is set as "AllowPortrait = true" and "IsLandscape = false" : show portrait UI
        // Otherwise : show landscape
        public float GetHorizontalPosition { get { return !this.AllowPortrait || PlaceCanvas.IsLandscape() ? this.horizontalPosition : this.portraitHorizontalPosition; } }
        public float GetVerticalPosition { get { return !this.AllowPortrait || PlaceCanvas.IsLandscape() ? this.verticalPosition : this.portraitVerticalPosition; } }
        public float GetHorizontalSize { get { return !this.AllowPortrait || PlaceCanvas.IsLandscape() ? this.horizontalSize : this.portraitHorizontalSize; } }
        public float GetVerticalSize { get { return !this.AllowPortrait || PlaceCanvas.IsLandscape() ? this.verticalSize : this.portraitVerticalSize; } }

        // Initialization of the PlaceUIElement component
        protected override void Init()
        {
            if (!this.initiated)
            {
                // Call Init of the extended class
                base.Init();

                this.lastRefreshDate = DateTime.MinValue;

                // Get the parent UIElement
                this.parent = transform.parent.GetComponentInParent<UIElement>();
                if (this.parent == null)
                    Debug.LogError(string.Format("No UIElement component has been found in any parent of {0}.", transform.name));

                // Register in the parent to be refreshed when its refreshed
                this.parent.RefreshMeToo(this);
            }
        }

        void Awake()
        {
            // Initiate
            this.Init();
        }

        void OnDrawGizmos()
        {
            if ((DateTime.Now - this.lastRefreshDate).TotalMilliseconds > 10000)
                this.initiated = false;

            if (oldHorizontalPosition != GetHorizontalPosition || oldVerticalPosition != GetVerticalPosition || oldHorizontalSize != GetHorizontalSize || oldVerticalSize != GetVerticalSize)
            {
                // Remember the last position when placed
                oldHorizontalPosition = GetHorizontalPosition;
                oldVerticalPosition = GetVerticalPosition;
                oldHorizontalSize = GetHorizontalSize;
                oldVerticalSize = GetVerticalSize;

                PlaceCanvas.ForceRefresh();
            }

            // Initiate
            this.Init();
        }

        // This method is only called by the parent of the element
        // The parent is the window where the element is
        public void Place()
        {
            // Initiate
            this.Init();

            this.Width = parent.Width * GetHorizontalSize / 100;
            this.Height = parent.Height * GetVerticalSize / 100;
            this.rectTransform.sizeDelta = new Vector2(this.Width, this.Height);

            // Place the element in the window
            Vector3 newPosition = new Vector3(parent.rectTransform.position.x + (PlaceCanvas.ScreenWidth * GetHorizontalPosition / 100),
                                        parent.rectTransform.position.y - (PlaceCanvas.ScreenHeight * GetVerticalPosition / 100),
                                        0);

            if (float.IsInfinity(newPosition.x) || float.IsInfinity(newPosition.y) || float.IsInfinity(newPosition.z))
                newPosition = Vector3.zero;

            this.rectTransform.position = newPosition;

            // Place all UI element that registered to this window
            List<PlaceUIElement> placeUIElementsWithoutErrors = new List<PlaceUIElement>();
            foreach (PlaceUIElement elementToRefresh in this.uiElementsToRefresh)
            {
                try
                {
                    elementToRefresh.Place();

                    // Only add the UI element to place in the list if it's its direct child
                    if (elementToRefresh.transform.parent == this.transform)
                        placeUIElementsWithoutErrors.Add(elementToRefresh);
                }
                catch (Exception) { }
            }

            // Affect the list to the new one to keep only children no causing any error
            this.uiElementsToRefresh = placeUIElementsWithoutErrors;
        }*/
    }
}