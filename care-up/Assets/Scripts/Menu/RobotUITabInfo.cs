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

        // generate buttons
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
            }

            selectedButton = caller;

            set = true;
        }
    }
}
