using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

    public Texture2D crosshairTexture;

    private Rect position;

	void OnGUI () {

        if (crosshairTexture == null)
            return;

        position = new Rect((Screen.width - crosshairTexture.width) / 2.0f,
                           (Screen.height - crosshairTexture.height) / 2.0f,
                           crosshairTexture.width, crosshairTexture.height);

        GUI.DrawTexture(position, crosshairTexture);

	}
}
