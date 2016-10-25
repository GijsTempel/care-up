using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class Crosshair {

    public Texture2D crosshairTexture;
    public bool enabled = true;

    private Rect position;

    public void OnGUI() {

        if (crosshairTexture == null || !enabled)
            return;

        position = new Rect((Screen.width - crosshairTexture.width) / 2.0f,
                           (Screen.height - crosshairTexture.height) / 2.0f,
                           crosshairTexture.width, crosshairTexture.height);

        GUI.DrawTexture(position, crosshairTexture);

	}
}
