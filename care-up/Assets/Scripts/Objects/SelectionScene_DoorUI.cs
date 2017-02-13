using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SelectionScene_DoorUI : MonoBehaviour {

    public string sceneName;
    public Color selectedColor = Color.green;

    private bool selected = false;
    private TextMesh displayName;
    private TextMesh description;
    private TextMesh result;

    static private Controls controls;

	void Start () {

		if ( controls == null )
        {
            controls = GameObject.Find("GameLogic").GetComponent<Controls>();
        }

        displayName = transform.GetChild(0).GetComponent<TextMesh>();
        description = transform.GetChild(1).GetComponent<TextMesh>();
        result = transform.GetChild(2).GetComponent<TextMesh>();
    }
	
	void Update () {
        if (controls.MouseClicked() && controls.CanInteract &&
            controls.SelectedObject == gameObject)
        {
            // turn all off
            SelectionScene_DoorUI[] other = transform.parent.GetComponentsInChildren<SelectionScene_DoorUI>();
            foreach (SelectionScene_DoorUI ui in other)
            {
                ui.SetSelected(false);
            }
            // turn this one on
            SetSelected(true);
        }
    }

    public void SetSelected(bool value)
    {
        // turn on
        if ( !selected && value )
        {
            SystemObject door = transform.parent.GetComponent<SystemObject>();
            door.sceneName = door.description = sceneName;

            displayName.color = selectedColor;
            description.color = selectedColor;
            result.color = selectedColor;

            selected = true;
        }
        else if ( selected ) // off
        {
            displayName.color = Color.white;
            description.color = Color.white;
            result.color = Color.white;

            selected = false;
        }
    }
}
