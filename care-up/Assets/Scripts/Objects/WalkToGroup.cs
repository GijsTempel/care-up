using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WalkToGroup : MonoBehaviour
{

    public Vector3 position;
    public Vector3 rotation;

    //public List<InteractableObject> items = new List<InteractableObject>();

    CameraMode cameraMode;
    Controls controls;

    public void HighlightGroup(bool value)
    {
        /*foreach (InteractableObject i in items)
        {
            i.Highlight(value);
        }*/
    }

    private void Start()
    {
        GameObject gameLogic = GameObject.Find("GameLogic");

        cameraMode = gameLogic.GetComponent<CameraMode>();
        controls = gameLogic.GetComponent<Controls>();
    }

    protected void Update()
    {
        if (cameraMode.CurrentMode == CameraMode.Mode.Free)
        {
            if (controls.SelectedObject == gameObject && !cameraMode.animating)
            {
                HighlightGroup(true);
            }
            else
            {
                HighlightGroup(false);
            }
        }
    }
}