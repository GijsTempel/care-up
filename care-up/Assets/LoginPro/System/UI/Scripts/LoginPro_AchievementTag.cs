using System;
using UnityEngine;
using UnityEngine.UI;

namespace LoginProAsset
{
    /*
    [RequireComponent(typeof(LayoutElement))]
    [RequireComponent(typeof(RectTransform))]
    public class LoginPro_AchievementTag : MonoBehaviour
    {
        [HideInInspector]
        //public LoginPro_Achievement Achievement;

        public float RatioHeightWidth = 0.225f;
        public float ImageRatio = 0.8f;
        public float ImageOffest = 30f;

        public int rowIndex = 0;

        [HideInInspector]
        public bool MobileUI = false;
        private RectTransform listParent;

        private LayoutElement layout;

        private RectTransform rectTransform;
        private RectTransform list;
        private RectTransform message;

        private VerticalLayoutGroup layoutGroup;

        private bool initiated = false;

        private Image achievementImage;
        private Image childImage;
        private Sprite oldSprite;

        private Text achievementText;
        private Text childText;
        private string oldText;

        
        public void Init(LoginPro_Achievement achievement)
        {
            this.Achievement = achievement;
            Image image = achievement.transform.GetComponent<Image>();
            Text text = achievement.transform.Find("Message").GetComponent<Text>();

            this.achievementImage = image;
            this.achievementText = text;
            this.childImage = this.gameObject.transform.GetComponent<Image>();
            this.childText = this.gameObject.transform.Find("Message").GetComponent<Text>();

            this.oldSprite = childImage.sprite;
            this.oldText = childText.text;

            this.initiated = true;

            this.RefreshDisplay();
        }

        void Start()
        {
            Refresh();
        }

        public void RefreshDisplay()
        {
            if (!this.initiated)
                return;

            if (this.Achievement.Unlocked && this.Achievement != null)
            {
                if (childImage != null)
                    childImage.sprite = achievementImage.sprite;
                if (childText != null)
                    childText.text = achievementText.text;
            }
            else
            {
                if (childImage != null)
                    childImage.sprite = oldSprite;
                if (childText != null)
                    childText.text = oldText;
            }
        }

        public void Refresh()
        {
            this.layout = this.transform.GetComponent<LayoutElement>();
            this.rectTransform = this.transform.parent.GetComponent<RectTransform>();
            this.list = this.transform.parent.GetComponent<RectTransform>();
            if (this.list == null)
                Debug.LogError("No RectTransform in parent achievement tag.");

            if (this.MobileUI)
                this.listParent = this.list.parent.gameObject.GetComponent<RectTransform>();
            this.layoutGroup = this.list.gameObject.GetComponent<VerticalLayoutGroup>();
            if (this.layoutGroup == null)
                Debug.LogError("No VerticalLayoutGroup in parent achievement tag.");

            this.message = this.transform.Find("Message").GetComponent<RectTransform>();
        }

        void OnGUI()
        {
            float listWidth = MobileUI ? Math.Abs(this.listParent.sizeDelta.x) + 30 : Math.Abs(this.list.sizeDelta.x);
            this.layout.preferredHeight = listWidth * this.RatioHeightWidth;

            
            // Resize the image
            float imageHeight = layout.preferredHeight * ImageRatio;
            this.image.sizeDelta = new Vector2(imageHeight, imageHeight);

            // Place the image
            float x = this.rectTransform.position.x + (this.layout.preferredHeight * 0.52f);
            float y = this.rectTransform.position.y - (this.layout.preferredHeight * 0.53f);
            float z = this.rectTransform.position.z;

            // Before apply position, multiply it by its verticalIndex
            y -= this.rowIndex * (this.layout.preferredHeight + this.layoutGroup.spacing);

            //this.image.position = new Vector3(x, y, z);

            // Resize the text
            this.message.sizeDelta = new Vector2(listWidth * 0.65f, layout.preferredHeight);
            // Place the image
            x = this.rectTransform.position.x + listWidth * 0.55f;
            this.message.position = new Vector3(x, y, z);
        }
    }*/
}