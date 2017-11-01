using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsulinPen : PickableObject {

    public bool animateButton = false;

    private Transform button;

    protected override void Start()
    {
        base.Start();

        animateButton = false;
        button = transform.Find("insulinPenButton");
    }

    protected override void Update()
    {
        base.Update();

        if (animateButton)
        {
            button.localPosition = new Vector3(
                button.localPosition.x,
                button.localPosition.y,
                -0.002f * controlBone.localPosition.y);

            button.localRotation = Quaternion.Euler(
                button.localRotation.eulerAngles.x,
                button.localRotation.eulerAngles.y,
                -14.5f * controlBone.localPosition.y);
        }
    }

}
