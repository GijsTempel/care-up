using UnityEngine;
using System;
using System.Collections.Generic;

namespace LoginProAsset
{
    [RequireComponent(typeof(RectTransform))]
    [ExecuteInEditMode]
    public class PlaceCanvas : UIElement
    {
        public bool AllowScreenRotation = false;

        // All animations specified at startup
        public List<UIAnimation> AnimationToLaunchAtStartup;

        public AchievementsListScroller ListScrollerToRefresh;

        // Last time the UI refresh
        // Useful to register all PlaceUIElement in their parent
        private DateTime lastRefreshDate;

        // Screen dimensions
        public static int forceRefresh = 0;
        public int privateForceRefresh = 0;
        public static float ScreenWidth = 0;
        public static float ScreenHeight = 0;
        public static bool UICanRotate = false;

        // Canvas dimension
        private float currentWidth = 0;
        private float currentHeight = 0;
        private float previousWidth;
        private float previousHeight;

        public static bool IsLandscape()
        {
            return !UICanRotate || ScreenWidth >= ScreenHeight;
        }

        public static void ForceRefresh()
        {
            forceRefresh++;
        }

        // Initialization of the PlaceCanvas component
        protected override void Init()
        {
            if (!this.initiated)
            {
                base.Init();

                // Initiate the children list to refresh (only if not already done)
                if (this.uiElementsToRefresh == null)
                    this.uiElementsToRefresh = new List<PlaceUIElement>();
                this.lastRefreshDate = DateTime.MinValue;
            }
        }

        /// <summary>
        /// Awake
        /// </summary>
        void Awake()
        {
            UICanRotate = this.AllowScreenRotation;

            // Initiate
            this.Init();
            this.Refresh();
        }

        /// <summary>
        /// Launch all animations specified at startup
        /// </summary>
        /*void Start()
        {
            foreach (UIAnimation anim in this.AnimationToLaunchAtStartup)
            {
                anim.Launch();
            }
        }*/

        /// <summary>
        /// Everytime the UI refreshes in GAME
        /// </summary>
        void OnGUI()
        {
            this.Refresh(!Application.isPlaying);
        }

        /// <summary>
        /// Get screen dimensions, then refresh every direct children registered in the list
        /// </summary>
        /// <param name="editorMode"></param>
        private void Refresh(bool editorMode = false)
        {
            if (editorMode)
            {
                // Get the window size (in unity editor)
                Vector2 gameWindowSize = GetGameViewSizeInEditor();
                currentWidth = gameWindowSize.x;
                currentHeight = gameWindowSize.y;
            }
            else
            {
                currentWidth = this.rectTransform.rect.width;
                currentHeight = this.rectTransform.rect.height;
            }

            // If rectTransform transition don't do anything
            if (Math.Round(currentWidth, 0) == 0 || Math.Round(currentHeight, 0) == 0)
            {
                return;
            }

            ScreenWidth = currentWidth;
            ScreenHeight = currentHeight;

            // Refresh elements if screen size changes
            if (forceRefresh != privateForceRefresh || (Math.Round(previousWidth, 0) != Math.Round(currentWidth, 0) || Math.Round(previousHeight, 0) != Math.Round(currentHeight, 0)))
            {
                privateForceRefresh = forceRefresh;
                previousWidth = currentWidth;
                previousHeight = currentHeight;

                this.Width = currentWidth;
                this.Height = currentHeight;
                this.lastRefreshDate = DateTime.Now;
            }

            // Place the window on screen during a second after resizing
            // This is useful to avoid little glitches
            if ((DateTime.Now - this.lastRefreshDate).TotalMilliseconds < 1000)
            {
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
            }
        }

        // -------------------------------- EDITOR --------------------------------- //
        /// <summary>
        /// Useful only to get the size of the Game window (IN THE EDITOR ONLY)
        /// </summary>
        /// <returns>The size of the screen</returns>
        public static Vector2 GetGameViewSizeInEditor()
        {
            Type T = Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
            return (Vector2)Res;
        }
    }
}