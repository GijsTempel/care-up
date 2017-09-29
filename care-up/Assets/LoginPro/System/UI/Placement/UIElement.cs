using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LoginProAsset
{
    public class UIElement : MonoBehaviour
    {
        [HideInInspector]
        public float Width;
        [HideInInspector]
        public float Height;
        public RectTransform rectTransform;
        protected bool initiated = false;

        public List<PlaceUIElement> uiElementsToRefresh;

        protected virtual void Init()
        {
            this.Width = 0;
            this.Height = 0;
            this.rectTransform = transform.GetComponent<RectTransform>();
        }

        public void RefreshMeToo(PlaceUIElement element)
        {
            // Initiate the children list to refresh (only if not already done)
            if (this.uiElementsToRefresh == null)
                this.uiElementsToRefresh = new List<PlaceUIElement>();

            // Add the child to the list of children to place
            if (!this.uiElementsToRefresh.Contains(element))
                this.uiElementsToRefresh.Add(element);
        }
    }
}