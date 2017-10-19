using System;
using UnityEngine;
using UnityEngine.UI;

namespace LoginProAsset
{
    public class AchievementsListScroller : MonoBehaviour
    {
        public RectTransform Scroller;
        public PlaceUIElement ListLeft;
        public PlaceUIElement ListRight;
        public VerticalLayoutGroup ListLeftLayoutGroup;
        public RectTransform ListLeftRectTransform;
        public Scrollbar ScrollBar;

        private PlaceUIElement Frame;
        private RectTransform FrameRect;

        private float oldScrollBarValue = 0;
        private float scrollHeight = 0;

        private float initialScrollerPosition = 0;
        private float initialVerticalPosition = 0;

        private bool initiated = false;

        void Start()
        {
            // Check if everything is specified
            if (this.Scroller == null || this.ListLeft == null || this.ListRight == null || this.ListLeftLayoutGroup == null)
                Debug.LogError("Please specify all fields of AchievementsListScroller.");

            this.ListLeftRectTransform = this.ListLeft.GetComponent<RectTransform>();

            this.Frame = this.gameObject.GetComponent<PlaceUIElement>();
            this.FrameRect = this.gameObject.GetComponent<RectTransform>();

            this.initiated = true;

            this.Refresh();
        }

        public void Refresh()
        {
            if (!this.initiated)
                return;

            int achievementsRows = this.ListLeft.transform.childCount;
            scrollHeight = 0;
            // If there is at least one row, the size is the height of a row + the spacing multiplied by the number of rows
            if (achievementsRows > 0)
                scrollHeight = achievementsRows * this.ListLeft.transform.GetChild(0).GetComponent<LayoutElement>().preferredHeight + (achievementsRows - 1) * this.ListLeftLayoutGroup.spacing;

            // Do no change the scrollbar if the size of the content is smaller than the frame
            if (this.FrameRect.sizeDelta.y > scrollHeight)
                return;

            // Set Scroller size
            this.Scroller.sizeDelta = new Vector2(this.Scroller.sizeDelta.x, scrollHeight);

            // Place scroll only if size has changed
            this.Scroller.position = new Vector3(this.ListLeftRectTransform.position.x + (this.Scroller.sizeDelta.x * 0.5f),
                                                this.ListLeftRectTransform.position.y - (scrollHeight * 0.5f),
                                                this.ListLeftRectTransform.position.z);

            this.initialScrollerPosition = this.Scroller.position.y;
            this.initialVerticalPosition = this.ListLeftRectTransform.position.y;

            ApplyScroll();
        }

        void OnGUI()
        {
            // Apply scroll to the list if the scroll has changed
            if (Math.Round(this.ScrollBar.value, 2) != Math.Round(oldScrollBarValue, 2))
            {
                ApplyScroll();
                oldScrollBarValue = this.ScrollBar.value;
            }
        }

        private void ApplyScroll()
        {
            // The delta of the scroller from its initial position to its current
            float positionDelta = this.initialScrollerPosition - this.Scroller.position.y;

            // Scroll the lists
            float scrollToReach = this.GetScrollFromPosition(this.initialVerticalPosition - positionDelta);
            this.ListLeft.verticalPosition = scrollToReach;
            this.ListRight.verticalPosition = scrollToReach;
            PlaceCanvas.ForceRefresh();
        }

        private float GetScrollFromPosition(float positionWanted)
        {
            return ((this.Frame.rectTransform.position.y - positionWanted) * 100) / PlaceCanvas.ScreenHeight;
        }
    }
}