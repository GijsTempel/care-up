using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotUITabInfo : RobotUITabs {

    private RectTransform selectedButton;
    private RectTransform deselectButton;

    private bool set = false;
    private float timer = 0.0f;

    protected override void Start()
    {
        base.Start();

        Transform t = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        if ( t != null )
        {
            OnItemButtonClick(t.GetComponent<RectTransform>());
        }

        // generate buttons
        // done in playerspawn
    }

    private void Update()
    {
        if (set)
        {
            timer += Time.deltaTime;
            float maxTime = 0.3f;

            if (selectedButton != null)
            {
                float sX = (timer / maxTime) * 45.0f;
                selectedButton.sizeDelta = new Vector2(230 + sX, 50);
            }

            if (deselectButton != null)
            {
                float dX = ((maxTime - timer) / maxTime) * 45.0f;
                deselectButton.sizeDelta = new Vector2(230 + dX, 50);
            }

            if (timer >= maxTime)
            {
                timer = 0.0f;
                set = false;
            }
        }
    }

    public void OnItemButtonClick(RectTransform caller)
    {
        if (selectedButton != caller)
        {
            if (selectedButton != null)
            {
                deselectButton = selectedButton;
                deselectButton.GetComponent<RobotUIInfoButton>().Toggle(false);
            }

            selectedButton = caller;
            selectedButton.GetComponent<RobotUIInfoButton>().Toggle(true);

            set = true;
        }
    }

    protected override void SetTabActive(bool value)
    {
        base.SetTabActive(value);

        if (selectedButton != null)
        {
            selectedButton.GetComponent<RobotUIInfoButton>().Toggle(value);
        }
    }
}
