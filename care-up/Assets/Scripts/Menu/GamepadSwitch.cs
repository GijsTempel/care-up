using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamepadSwitch : MonoBehaviour {

    public static bool gamepad = false;
    
    public static bool HandleUpdate(Selectable first)
    {
        if (!gamepad && Input.GetAxisRaw("Vertical") != 0.0f)
        {
            gamepad = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            first.GetComponent<Button>().Select();
        }
        else if (gamepad && (Input.GetAxisRaw("Mouse X") != 0 ||
            Input.GetAxisRaw("Mouse Y") != 0))
        {
            gamepad = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        return gamepad;
    }
}
